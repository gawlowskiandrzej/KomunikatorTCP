using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ClientWPF
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string SERVER_IPADDRESS = "192.168.0.102";
        public static int SERVER_PORT = 8080;
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                if (e.Args.Length >0) 
                {
                    SERVER_IPADDRESS = e.Args[0];
                    SERVER_PORT = int.Parse(e.Args[1]);
                }
                    
            }
            catch(Exception)
            {
                MessageBox.Show("Bład parametrow");
            }
            
        }
    }
}
