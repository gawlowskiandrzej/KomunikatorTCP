using ClientWPF.Models.Controlers;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ClientWPF.Models
{
    internal class User : INotifyPropertyChanged
    {
        private bool _isSelected = false;
        private bool _isConnected = false;

        public MessageControler MessageControler { get; set; }
        public ConnectControler ConnectControler { get; set; }
        public bool IsConnected { get => _isConnected; set => _isConnected = value; }
        public bool IsSelected 
        { 
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
}
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
        public User(string name, bool real)
        {
            ConnectControler = new ConnectControler();
            MessageControler = new MessageControler(ConnectControler.Client);
            Name = name;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void Logout()
        {
            if (ConnectControler != null && ConnectControler.Client != null)
            {
                try
                {
                    ConnectControler.Client.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                    ConnectControler.Client.Close();
                }
                catch (System.Exception e)
                {
                    MessageBox.Show($"Error during logout: {e.Message}");
                }
                finally
                {
                    ConnectControler.Client = null;
                    _isConnected = false;
                    OnPropertyChanged(nameof(IsConnected));
                }
            }
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
