using ClientWPF.Commands;
using ClientWPF.Models;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System;
using ClientWPF.Models.Controlers;

namespace ClientWPF.ViewModels
{
    internal class HomeVM : ViewModelBase
    {
        public string MessageText { get; set; } 
        public User SelectedUser { get; set; }
        CancellationTokenSource _cancellationTokenSource;
        public ICommand SendMessgeCommand { get; set; }
        public ICommand SendCommand { get; set; }
        public MessagesVM MessagesVM { get; set; }

        public HomeVM()
        {
            if (SelectedUser == null)
                SelectedUser = MainVM.Repository.GetSelectedUser();
            MessagesVM = new MessagesVM();
            InitCommands();
        }
        public void ChangeSelection()
        {
            SelectedUser = MainVM.Repository.GetSelectedUser();
            MessagesVM = new MessagesVM();
            InitCommands();
            OnPropertyChanged(nameof(SelectedUser));
            OnPropertyChanged(nameof(MessagesVM));
            OnPropertyChanged(nameof(MessageText));
        }
        void InitCommands()
        {
            SendMessgeCommand = new RelayCommand(SendMessage);
            SendCommand = new RelayCommand(Send);
        }
        public void StartMessageListening()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        var user = MainVM.Repository.GetLoggedUser();
                        var message = user.MessageControler.Receive();
                        if (message != null)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                MainVM.Repository.Messages.Add(message);
                                MessagesVM.UpdateMessages();
                            });
                        }

                        // Opcjonalnie: dodaj opóźnienie, aby zmniejszyć obciążenie procesora
                        Task.Delay(500).Wait();
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
        public void StopMessageListening()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
        }
        public void SendMessage(object obj) 
        {
            var loggedUser = MainVM.Repository.GetLoggedUser();
            string message = $"{1}:{loggedUser.Name}:{SelectedUser.Name}:{MessageText}";
            if (loggedUser.MessageControler.Send(message))
            {
                Message mess = new Message(loggedUser.Name, SelectedUser.Name, MessageText);
                MainVM.Repository.Messages.Add(mess);
                MessagesVM.UpdateMessages();
            }
        }
        public void Send(object obj) => MessagesVM.Messages.Add(new Message("test2", "test1", "lalal"));
    }
}
