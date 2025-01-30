using ClientWPF.Commands;
using System.Windows.Input;

namespace ClientWPF.ViewModels
{
    internal class UserVM : ViewModelBase
    {
        public string Username { get; set; } = "username";
        public ICommand AddUser { get; set; }
        public bool AddedUser { get; set; }

        public UserVM()
        {
            AddUser = new RelayCommand(AddUserAction);
            OnPropertyChanged(nameof(AddedUser));
        }
        public void AddUserAction(object obj)
        {
            MainVM.Repository.Users.Add(new Models.User(Username));
        }
    }
}
