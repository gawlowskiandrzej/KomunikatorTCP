using System.Collections.Generic;

namespace ClientWPF.Models.Interfaces
{
    interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        User GetSelectedUser();
        void SetSelection(User user);
    }
}