using ClientWPF.Models.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
namespace ClientWPF.Models
{
    internal class Repository : IUserRepository, IMessageRepository
    {
        private List<User> users;
        private User LoggedUser;
        public List<User> Users
        {
            get
            {
                if (users?.Count() > 0)
                    return users;

                users = new List<User>();
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

                messages = new ObservableCollection<Message>();
                return messages;
            }
            set
            {
                messages = value;
            }
        }
        //public event PropertyChangedEventHandler PropertyChanged;

        public User GetSelectedUser() 
        {
            var usr = Users.Where(_ => _.IsSelected).FirstOrDefault();
            if (usr == null)
            {
                if (Users.Count > 0) return Users.First();
            }
            return usr;
        }

        // TODO: LOGGED USER SELECTION
        // IMplement logged user 

        public User GetLoggedUser() => LoggedUser;
        public void SetLoggedUser(User User) { User.IsConnected = true; LoggedUser = User;}

        public IEnumerable<User> GetUsers()
        {
            var loggedUser = GetLoggedUser();

            foreach (var message in this.Messages)
            {
                if (message.UserFrom != loggedUser.Name && !Users.Any(u => u?.Name == message.UserFrom))
                {
                    Users.Add(new User(message.UserFrom));
                }

                if (message.UserTo != loggedUser.Name && !Users.Any(u => u?.Name == message.UserTo))
                {
                    // Add the user that received the message (if not already added)
                    Users.Add(new User(message.UserTo));
                }
            }

            return Users;
        }

        public void SetSelection(User selectedUser)
        {
            foreach (var user in users)
            {
                user.IsSelected = false;
                if (selectedUser?.Name == user.Name)
                    user.IsSelected = true;
            }

        }

        public IEnumerable<Message> GetMessages() => messages;

    }
}
