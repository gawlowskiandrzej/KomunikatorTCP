namespace ClientWPF.Models.Interfaces
{
    internal interface IConnector
    {
        bool InitConnect();
        bool IsConnected();
        void Connect();
    }
}
