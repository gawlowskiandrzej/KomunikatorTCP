using System.Windows;

namespace ClientWPF.ViewModels
{
    internal class LoginVM : ViewModelBase
    {
        private Visibility _viewVisibility;
        public Visibility ViewVisibility
        {
            get => _viewVisibility;
            set
            {
                _viewVisibility = value;
                OnPropertyChanged();
            }
        }
        public LoginVM()
        {
            ViewVisibility = Visibility.Visible;
        }
        public void ToggleVisibility()
        {
            // Przełączanie widoczności
            ViewVisibility = ViewVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
