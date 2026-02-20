using System.Collections.Generic;

namespace ClientWPF.Models.Interfaces
{
    internal interface IMessageRepository
    {
        IEnumerable<Message> GetMessages();
    }
}
