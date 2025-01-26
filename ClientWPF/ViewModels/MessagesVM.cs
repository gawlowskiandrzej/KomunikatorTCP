using ClientWPF.Models;
using System.Collections.Generic;

namespace ClientWPF.ViewModels
{
    internal class MessagesVM
    {
        public IEnumerable<Message> Messages { get; set; }

        public MessagesVM()
        {
            Repository repo = new Repository();
            Messages = repo.Messages;
        }
    }
}
