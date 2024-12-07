using ClientWPF.Models;
using ClientWPF.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ClientWPF.ViewModels
{
    internal class UsersSideBarVM
    {
        public List<User> Users { get; set; }

        public UsersSideBarVM()
        {
            Users = new Repository().GetUsers().ToList();
        }
    }
}
