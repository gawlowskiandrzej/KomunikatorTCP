using ClientWPF.Commands;
using ClientWPF.Models;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace ClientWPF.ViewModels
{
    internal class UsersSideBarVM : ViewModelBase
    {
        private IEnumerable<User> _users;
        public IEnumerable<User> Users 
        { 
            get => MainVM.Repository?.GetUsers();
            set { _users = value;  } 
        }
        private User _selectedUser;

        public User SelectedUser
        {
            get => MainVM.Repository?.GetSelectedUser(); 
            set
            {
                _selectedUser = value;
                MainVM.Repository.SetSelection(value);
                OnPropertyChanged();
            }

        }
        public ICommand SelectUserCommand { get; set; }

        public UsersSideBarVM()
        {
            SelectUserCommand = new RelayCommand(SelectUser);
            Users = MainVM.Repository?.GetUsers();
            SelectedUser = MainVM.Repository?.GetSelectedUser();
        }
        public void SelectUser(object obj)
        {
            SelectedUser = obj as User;
        }
    }
}
