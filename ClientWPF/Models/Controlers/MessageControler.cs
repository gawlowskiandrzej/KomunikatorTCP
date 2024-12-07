using ClientWPF.Models.Interfaces;
using System;
using System.Net.Sockets;
using System.Text;

namespace ClientWPF.Models.Controlers
{
    internal class MessageControler : IReceiver, ISender
    {
        Socket Client { get; set; }

        public MessageControler(Socket client)
        {
            Client = client;
        }
        
        public void Receive(string buffer)
        {
            throw new NotImplementedException();
        }

        public void Receive(Message message)
        {
            throw new NotImplementedException();
        }

        public void Send(string buffer = "Test")
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(buffer);
            Client.Send(messageBytes);
        }

        public void Send(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
