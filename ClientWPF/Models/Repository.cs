using ClientWPF.Models.Interfaces;
using System.Collections.Generic;

namespace ClientWPF.Models
{
    internal class Repository : IUserRepository
    {
        public IEnumerable<User> GetUsers()
        {
            return new List<User>
            {
                new User("UserTest"),
                new User("UserTest1"),
                new User("UserTest2")
            };
        }
    }
}
