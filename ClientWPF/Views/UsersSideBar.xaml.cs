using System;
using System.Windows.Controls;

namespace ClientWPF.Views
{
    /// <summary>
    /// Logika interakcji dla klasy UsersSideBar.xaml
    /// </summary>
    public partial class UsersSideBar : UserControl
    {
        public event SelectionChangedEvent SelectionChanged;
        public delegate void SelectionChangedEvent(object sender, EventArgs e);
        public UsersSideBar()
        {
            InitializeComponent();
        }
    }
}
