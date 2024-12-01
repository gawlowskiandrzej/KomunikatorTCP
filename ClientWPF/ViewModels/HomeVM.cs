using ClientWPF.Commands;
using ClientWPF.Models;
using System.Windows.Input;

namespace ClientWPF.ViewModels
{
    internal class HomeVM
    {
        public ICommand SendMessgeCommand { get; set; }


        public HomeVM()
        {
            SendMessgeCommand = new RelayCommand(SendMessage);
        }

        public void SendMessage(object obj) => new User().Connect();
    }
}
