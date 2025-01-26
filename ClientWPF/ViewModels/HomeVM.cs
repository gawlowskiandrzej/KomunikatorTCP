using ClientWPF.Commands;
using ClientWPF.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System;

namespace ClientWPF.ViewModels
{
    internal class HomeVM
    {
        public User CurrentUser { get; set; }
        CancellationTokenSource _cancellationTokenSource;
        public ICommand SendMessgeCommand { get; set; }
        public ICommand SendCommand { get; set; }
        public MessagesVM MessagesVM { get; set; }

        public HomeVM(User selectedUser):base()
        {
            CurrentUser = selectedUser;
        }
        public HomeVM()
        {
            SendMessgeCommand = new RelayCommand(SendMessage);
            SendCommand = new RelayCommand(SendMessage);
            if (CurrentUser == null)
                CurrentUser = MainVM.UserRepository.GetSelectedUser();
            MessagesVM = new MessagesVM();

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
                        var message = CurrentUser.MessageControler.Receive();
                        if (message != null)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                MessagesVM.Messages.Append(message);
                                MessageBox.Show($"Nowa wiadomość otrzymana: {message.Content}");
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

        public void SendMessage(object obj) => CurrentUser.MessageControler.Send();
        public void Send(object obj) => MessageBox.Show("Wysyłanie wiadomości");
    }
}
