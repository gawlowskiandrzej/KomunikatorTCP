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

        public Message Receive()
        {
            byte[] buff = new byte[4096];
            int bytesReceived = Client.Receive(buff);

            string data = Encoding.UTF8.GetString(buff, 0, bytesReceived);


            string[] splitted = data.Split(':');

            return new Message(splitted[1], splitted[2], splitted[3]);
        }


        public void Receive(Message message)
        {
            throw new NotImplementedException();
        }

        public void Send(string buffer = "1:userSource:userDest:message")
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
