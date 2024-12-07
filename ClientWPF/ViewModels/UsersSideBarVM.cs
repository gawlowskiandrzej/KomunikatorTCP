using ClientWPF.Models;
using ClientWPF.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ClientWPF.ViewModels
{
    internal class UsersSideBarVM
    {
        public List<User> Users { get; set; }
        public IUserRepository UserRepo { get; }

        public UsersSideBarVM()
        {
            UserRepo = new Repository();
            Users = UserRepo.GetUsers().ToList();
        }
    }
}
