using System.Collections.Generic;

namespace ClientWPF.ViewModels
{
    internal class MessagesVM
    {
        public IEnumerable<string> Messages { get; set; }

        public MessagesVM()
        {
            Messages = new List<string>() { "Wiadomosc 1", "Wiadomosc 2", "Wiadomosc 3" };
        }
    }
}
