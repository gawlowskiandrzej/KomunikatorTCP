using ClientWPF.Models.Controlers;

namespace ClientWPF.Models
{
    internal class User
    {
        public MessageControler MessageControler { get; set; }
        public ConnectControler ConnectControler { get; set; }
        public bool IsConnected { get; set; }
        public User()
        {
            ConnectControler = new ConnectControler();
            MessageControler = new MessageControler(ConnectControler.Client);
        }

        public void Connect()
        {
            try
            {
                MessageControler.Send();
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

    }
}
