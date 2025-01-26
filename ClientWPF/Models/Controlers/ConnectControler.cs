using ClientWPF.Models.Interfaces;
using System;
using System.Net;
using System.Net.Sockets;
using System.Windows;

namespace ClientWPF.Models.Controlers
{
    internal class ConnectControler : IConnector
    {
        public string DstAddr { get; }
        public int Port { get; }
        IPEndPoint Endpoint { get; set; }
        public Socket Client { get;  set; }

        public ConnectControler(string dstAddr = "192.168.0.102", int port = 8080)
        {
            DstAddr = dstAddr;
            Port = port;

            InitConnect();
        }

        public bool InitConnect()
        {
            try
            {
                IPAddress dstAd = IPAddress.Parse(DstAddr);

                Endpoint = new IPEndPoint(dstAd, Port);

                Client = new Socket(Endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                Client.Connect(Endpoint);

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"Some error: {e.Message}");

                return false;
            }
        }

        public bool IsConnected() => Client.Connected;

        public void Connect() {

            if (Client is null)
                InitConnect();
            else
                Client.Connect(Endpoint);
        }
    }
}
