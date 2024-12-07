using ClientWPF.Models.Controlers;

namespace ClientWPF.Models
{
    internal class User
    {
        public MessageControler MessageControler { get; set; }
        public ConnectControler ConnectControler { get; set; }
        public bool IsConnected { get; set; }
        public string Name { get; set; }
        public User()
        {
            
        }
        public User(string name)
        {
            //ConnectControler = new ConnectControler();
            //MessageControler = new MessageControler(ConnectControler.Client);
            Name = name;
        }
        public override string ToString()
        {
            return this.Name;
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
