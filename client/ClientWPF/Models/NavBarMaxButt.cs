using System.Windows;
using System.Windows.Controls;

namespace ClientWPF.Models
{
    internal class NavBarMaxButt : Button
    {
        public bool IsMaxi = false;
        static NavBarMaxButt()
        {

        }
        protected override void OnClick()
        {
            base.OnClick();
            if (!IsMaxi)
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            if (IsMaxi)
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            IsMaxi = !IsMaxi;
        }
    }
}
