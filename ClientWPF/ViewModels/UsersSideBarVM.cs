using ClientWPF.Commands;
using ClientWPF.Models;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace ClientWPF.ViewModels
{
    internal class UsersSideBarVM
    {
        public IEnumerable<User> Users { get; set; }
        public User SelectedUser { get; set; }
        public ICommand SelectUserCommand { get; set; }

        public UsersSideBarVM()
        {
            Users = MainVM.UserRepository.GetUsers();
            SelectedUser = MainVM.UserRepository.GetSelectedUser();
            SelectUserCommand = new RelayCommand(SelectUser);
        }
        public void SelectUser(object obj)
        {
            MessageBox.Show("Selected");

        }
    }
}
