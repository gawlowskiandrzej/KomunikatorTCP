using ClientWPF.Commands;
using ClientWPF.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ClientWPF.ViewModels
{
    internal class HomeVM
    {
        public User CurrentUser { get; set; }
        public ICommand SendMessgeCommand { get; set; }
        public ICommand SendCommand { get; set; }

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
        }

        public void SendMessage(object obj) => CurrentUser.MessageControler.Send();
        public void Send(object obj) => MessageBox.Show("Wysyłanie wiadomości");
    }
}
