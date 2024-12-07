using ClientWPF.Models.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ClientWPF.Models
{
    internal class Repository : IUserRepository
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

        //public event PropertyChangedEventHandler PropertyChanged;

        public User GetSelectedUser() => Users.Where(_ => _.IsSelected).First();

        public IEnumerable<User> GetUsers() => Users;

        //protected void OnPropertyChanged([CallerMemberName] string name = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        //}

    }
}
