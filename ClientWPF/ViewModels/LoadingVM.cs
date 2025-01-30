using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ClientWPF.ViewModels
{
    internal class LoadingVM : ViewModelBase
    {
        public int AnimatedBarValue { get => _animatedBarValue; set { _animatedBarValue = value; OnPropertyChanged(); } }
        public Visibility ViewVisibility { get => _viewvisibility; set { _viewvisibility = value; OnPropertyChanged(); } }
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private int _animatedBarValue = 0;
        private Visibility _viewvisibility;

        public LoadingVM()
        {
            ViewVisibility = Visibility.Collapsed;
        }
        public void StartAnimation()
        {
            ViewVisibility = Visibility.Visible;
            StartAnimationLoop();
        }

        private async void StartAnimationLoop()
        {
            try
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    await Task.Delay(300);
                    AnimatedBarValue = (AnimatedBarValue + 1) % 101;
                }
            }
            catch (TaskCanceledException) { }
        }

        public void StopAnimation()
        {
            ViewVisibility = Visibility.Collapsed;
            _cts.Cancel();
        }

    }
}
