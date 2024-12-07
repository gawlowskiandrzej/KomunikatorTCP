using System.Collections.Generic;

namespace ClientWPF.Models.Interfaces
{
    internal interface IUserRepository
    {
        IEnumerable<User> GetUsers();
    }
}