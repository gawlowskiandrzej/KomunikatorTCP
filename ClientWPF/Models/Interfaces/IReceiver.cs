namespace ClientWPF.Models.Interfaces
{
    internal interface IReceiver
    {
        void Receive(string buffer);
        void Receive(Message message);
    }
}
