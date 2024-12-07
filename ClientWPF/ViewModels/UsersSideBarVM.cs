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
            get => MainVM.UserRepository?.GetUsers();
            set { _users = value;  } 
        }
        private User _selectedUser;

        public User SelectedUser
        {
            get => MainVM.UserRepository?.GetSelectedUser(); 
            set
            {
                _selectedUser = value;
                MainVM.UserRepository.SetSelection(value);
                OnPropertyChanged();
            }

        }
        public ICommand SelectUserCommand { get; set; }

        public UsersSideBarVM()
        {
            SelectUserCommand = new RelayCommand(SelectUser);
            Users = MainVM.UserRepository?.GetUsers();
            SelectedUser = MainVM.UserRepository?.GetSelectedUser();
        }
        public void SelectUser(object obj)
        {
            SelectedUser = obj as User;
        }
    }
}
