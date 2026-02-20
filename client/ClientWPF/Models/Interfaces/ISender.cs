namespace ClientWPF.Models.Interfaces
{
    interface ISender
    {
        bool Send(string buffer);
        void Send(Message message);
    }
}
