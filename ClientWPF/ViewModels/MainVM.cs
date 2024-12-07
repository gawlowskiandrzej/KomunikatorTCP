using ClientWPF.Commands;
using ClientWPF.Models;
using ClientWPF.Models.Interfaces;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Input;

namespace ClientWPF.ViewModels
{
    internal class MainVM : ViewModelBase
    {
        public static IUserRepository UserRepository { get; set; }

        private object currentView;

        public User SelectedUser { get; set; }

        public ICommand  HomeCommand { get; set; }
        public ICommand  LoginCommand { get; set; }
        public ICommand  ExitCommand { get; set; }
        public ICommand  MinimalizeCommand { get; set; }

        public UsersSideBarVM UsersSideBarVM { get; set; }

        public void Home(object obj) => CurrentView = new HomeVM();
        public void Login(object obj) => CurrentView = new LoginVM();
        public void Exit(object obj) => Application.Current.Shutdown();
        public void Minimalize(object obj) => Application.Current.MainWindow.WindowState = WindowState.Minimized;

        public object CurrentView
        {
            get { return currentView; }
            set
            {
                currentView = value;
                OnPropertyChanged();
            }
        }
        public MainVM()
        {
            HomeCommand = new RelayCommand(Home);
            LoginCommand = new RelayCommand(Login);
            ExitCommand = new RelayCommand(Exit);
            MinimalizeCommand = new RelayCommand(Minimalize);
            UserRepository = new Repository();
            SelectedUser = UserRepository.GetSelectedUser();

            UsersSideBarVM = new UsersSideBarVM();
            UsersSideBarVM.PropertyChanged += OnUsersSideBarChanged;

            CurrentView = new HomeVM();
        }

        public void OnUsersSideBarChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UsersSideBarVM.SelectedUser))
            {
                // Update CurrentView based on the new SelectedUser
                CurrentView = new HomeVM(UsersSideBarVM.SelectedUser);
            }
        }
    }
}
