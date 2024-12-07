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
        static IEnumerable<User> Users { get; set; }
        public User SelectedUser { get; set; }
        public ICommand SendMessgeCommand { get; set; }

        public ICommand SendCommand { get; set; }


        public HomeVM()
        {
            SendMessgeCommand = new RelayCommand(SendMessage);
            SendCommand = new RelayCommand(Send);
            Users = new Repository().GetUsers();
            SelectedUser = Users.First();
            SelectedUser.IsSelected = true;
        }

        public void SendMessage(object obj) => new User().Connect();
        public void Send(object obj) => MessageBox.Show("Wysyłanie wiadomości");
    }
}
