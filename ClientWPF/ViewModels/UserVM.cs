using ClientWPF.Commands;
using System.Windows.Input;

namespace ClientWPF.ViewModels
{
    internal class UserVM : ViewModelBase
    {
        private bool _addedUser;

        public string Username { get; set; }
        public ICommand AddUser { get; set; }
        public bool AddedUser { get => _addedUser; set { _addedUser = value; OnPropertyChanged(); } }

        public UserVM()
        {
            AddUser = new RelayCommand(AddUserAction);
            OnPropertyChanged(nameof(AddedUser));
        }
        public void AddUserAction(object obj)
        {
            MainVM.Repository.Users.Add(new Models.User(Username));
            //TODO: ADD USER, UPDATE Side bar.
            AddedUser = true;
        }
    }
}
