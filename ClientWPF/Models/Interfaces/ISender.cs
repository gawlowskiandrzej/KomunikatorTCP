namespace ClientWPF.Models.Interfaces
{
    interface ISender
    {
        void Send(string buffer);
        void Send(Message message);
    }
}
