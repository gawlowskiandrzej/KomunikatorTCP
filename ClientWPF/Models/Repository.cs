using ClientWPF.Models.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
namespace ClientWPF.Models
{
    internal class Repository : IUserRepository, IMessageRepository
    {
        private IEnumerable<User> users;
        public IEnumerable<User> Users { 
            get 
            {
                if (users?.Count() > 0)
                    return users;

                users = new List<User>
                {
                    new User("UserTest"){IsSelected = true},
                    new User("UserTest1"),
                    new User("UserTest2")
                };
                return users;
            } 
            set 
            {
                users = value;
            }
        }
        private ObservableCollection<Message> messages;
        public ObservableCollection<Message> Messages
        {
            get
            {
                if (messages?.Count() > 0)
                    return messages;

                messages = new ObservableCollection<Message>
                {
                    new Message("UserTest","UserTest1", "wiadomosc1"),
                    new Message("UserTest1","UserTest2", "wiadomosc2"),
                    new Message("UserTest2","UserTest1", "wiadomosc3")
                };
                return messages;
            }
            set
            {
                messages = value;
            }
        }
        //public event PropertyChangedEventHandler PropertyChanged;

        public User GetSelectedUser() => Users.Where(_ => _.IsSelected).First();

        // TODO: LOGGED USER SELECTION
        // IMplement logged user 

        public User GetLoggedUser() => Users.Where(_ => _.IsConnected).FirstOrDefault();


        public IEnumerable<User> GetUsers() => Users;

        public void SetSelection(User selectedUser)
        {
            foreach (var user in users)
            {
                user.IsSelected = false;
                if (selectedUser.Name == user.Name)
                    user.IsSelected = true;
            }
            
        }

        public IEnumerable<Message> GetMessages() => messages;

        //protected void OnPropertyChanged([CallerMemberName] string name = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        //}

    }
}
