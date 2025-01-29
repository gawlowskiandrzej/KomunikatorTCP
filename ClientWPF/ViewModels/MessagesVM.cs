using ClientWPF.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace ClientWPF.ViewModels
{
    internal class MessagesVM
    {
        public string loggedUsername;
        public string selectedUsername;
        public ObservableCollection<Message> Messages { get; set; }

        public MessagesVM(string loggedUsername, string selectedUsername)
        {
            Messages = new ObservableCollection<Message>();
            this.loggedUsername = loggedUsername;
            this.selectedUsername = selectedUsername;
            UpdateMessages();
        }
        public void UpdateMessages()
        {
            MainVM.Repository.Messages.Where(_ => _.UserFrom == selectedUsername && _.UserTo == loggedUsername).ToList();
        }
    }
}
