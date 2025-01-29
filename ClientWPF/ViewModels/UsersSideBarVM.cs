using ClientWPF.Commands;
using ClientWPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ClientWPF.ViewModels
{
    internal class UsersSideBarVM : ViewModelBase
    {
        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users 
        {
            get => _users;
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
            Users = new ObservableCollection<User>(MainVM.Repository.Users);
            Users[1].IsSelected = true;
            SelectedUser = MainVM.Repository?.GetSelectedUser();

            OnPropertyChanged(nameof(SelectedUser));
            OnPropertyChanged(nameof(Users));
        }
        public void SelectUser(object obj)
        {
            SelectedUser = obj as User;
        }
    }
}
