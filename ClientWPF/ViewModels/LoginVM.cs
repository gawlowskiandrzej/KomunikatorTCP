using ClientWPF.Commands;
using ClientWPF.Models;
using ClientWPF.Models.Controlers;
using System;
using System.ComponentModel.Design;
using System.Windows;
using System.Windows.Input;

namespace ClientWPF.ViewModels
{
    internal class LoginVM : ViewModelBase
    {
        public string userInput { get; set; }
        public bool IsInitialized { get => _isInitialized; set { _isInitialized = value; OnPropertyChanged(); } }
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
            ViewVisibility = Visibility.Visible;
            User = new User();
            LoginCommand = new RelayCommand(LoginUser);
        }
        public void ToggleVisibility()
        {
            // Przełączanie widoczności
            ViewVisibility = ViewVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
        public void LoginUser(object obj)
        {
            try
            {
                User.Name = userInput;
                User.ConnectControler = new ConnectControler();
                User.MessageControler = new MessageControler(User.ConnectControler.Client);
                User.MessageControler.Send(User.Name);
                if (User.ConnectControler.Client.Connected)
                {
                    User.IsConnected = true;
                    MainVM.Repository.Users.Add(User);
                    var users = MainVM.Repository.GetUsers();
                    ViewVisibility = Visibility.Collapsed;
                    IsInitialized = true;
                    // Not working
                }

            }
            catch(Exception e)
            {
                MessageBox.Show($"Connect error: {e.Message}");
            }
            
        }
    }
}
