namespace ClientWPF.Models.Interfaces
{
    internal interface IReceiver
    {
        Message Receive();
        void Receive(Message message);
    }
}
