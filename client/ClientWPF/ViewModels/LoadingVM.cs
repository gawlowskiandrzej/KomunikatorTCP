using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ClientWPF.ViewModels
{
    internal class LoadingVM : ViewModelBase
    {
        public Visibility ViewVisibility { get => _viewvisibility; set { _viewvisibility = value; OnPropertyChanged(); } }
        private Visibility _viewvisibility;

        public LoadingVM()
        {
            ViewVisibility = Visibility.Collapsed;
        }
        public void StartAnimation()
        {
            ViewVisibility = Visibility.Visible;
        }

        public void StopAnimation()
        {
            ViewVisibility = Visibility.Collapsed;
        }

    }
}
