using ClientWPF.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ClientWPF.ViewModels
{
    internal class MessagesVM :ViewModelBase
    {
        private ObservableCollection<Message> _messages;

        public ObservableCollection<Message> Messages { get => _messages; set { _messages = value; OnPropertyChanged(); } }

        public MessagesVM()
        {
            Messages = new ObservableCollection<Message>();
            UpdateMessages();
        }

        public void UpdateMessages()
        {
            var selectedUser = MainVM.Repository.GetSelectedUser();
            var loggedUser = MainVM.Repository.GetLoggedUser();

            // Czyść starą listę
            Messages.Clear();

            // Dodaj nowe wiadomości do kolekcji
            var filteredMessages = MainVM.Repository.Messages
                .Where(_ => (_.UserFrom == selectedUser?.Name && _.UserTo == loggedUser?.Name) ||
                            (_.UserFrom == loggedUser?.Name && _.UserTo == selectedUser?.Name));

            foreach (var message in filteredMessages)
            {
                Messages.Add(message);
            }
        }
    }
}
