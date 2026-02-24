using ClientWPF.Commands;
using ClientWPF.Models;
using ClientWPF.Models.Controlers;
using System;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace ClientWPF.ViewModels
{
    internal class LoginVM : ViewModelBase
    {
        public string userInput { get; set; }
        public bool IsInitialized { get => _isInitialized; set { _isInitialized = value; OnPropertyChanged(); } }
        CancellationTokenSource _cancellationTokenSource;
        public LoadingVM LoadingView { get; set; }
        public User User { get; set; }

        //private bool _isConnected;
        //public bool IsConnected 
        //{ 
        //    get => _isConnected;
        //    set 
        //    { 
        //        _isConnected = value; 
        //        OnPropertyChanged(); 
        //    } 
        //}
        private Visibility _viewVisibility;
        private bool _isInitialized;

        public Visibility ViewVisibility
        {
            get => _viewVisibility;
            set
            {
                _viewVisibility = value;
                OnPropertyChanged();
            }
        }
        public ICommand LoginCommand { get; set; }
        public LoginVM()
        {
            LoadingView = new LoadingVM();
            ViewVisibility = Visibility.Visible;
            User = new User();
            LoginCommand = new RelayCommand(LoginUser);
        }
        public void ToggleVisibility()
        {
            // Przełączanie widoczności
            ViewVisibility = ViewVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
        public async void LoginUser(object obj)
        {
            try
            {
                User.Name = userInput;
                User.ConnectControler = new ConnectControler(App.SERVER_IPADDRESS, App.SERVER_PORT);
                User.MessageControler = new MessageControler(User.ConnectControler.Client);
                User.MessageControler.Send(User.Name);
                if (User.ConnectControler.Client.Connected)
                {
                    LoadingView.StartAnimation();
                    MainVM.Repository.Users.Clear();
                    MainVM.Repository.Messages.Clear();
                    MainVM.Repository.SetLoggedUser(User);
                    // TODO: Loading screen
                    await LoadMessageHistory();
                    var users = MainVM.Repository.GetUsers();
                    IsInitialized = true;
                    LoadingView.StopAnimation();
                }

            }
            catch(Exception e)
            {
                MessageBox.Show($"Connect error: {e.Message}");
            }
            
        }
        public Task LoadMessageHistory()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            return Task.Run(async () =>
            {
                var user = MainVM.Repository.GetLoggedUser();
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        var message = user.MessageControler.Receive();
                        if (message != null)
                        {
                            if (message.UserFrom == "") { _cancellationTokenSource.Cancel(); return; } // Stop receive load packets
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                MainVM.Repository.Messages.Add(message);
                            });
                        }

                        await Task.Delay(100, token);
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show($"Błąd podczas odbierania wiadomości: {ex.Message}");
                        });
                    }
                }
            }, token);
        }
    }
}
