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
        public static Repository Repository { get; set; }

        private object currentView;
        private UsersSideBarVM _usersSideBarVM;

        public User SelectedUser { get; set; }
        

        public ICommand  HomeCommand { get; set; }
        public ICommand  LoginCommand { get; set; }
        public ICommand  ExitCommand { get; set; }
        public ICommand  MinimalizeCommand { get; set; }

        public UsersSideBarVM UsersSideBarVM { get => _usersSideBarVM; set { _usersSideBarVM = value; OnPropertyChanged(); } }
        public LoginVM LoginView { get; set; }

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
            Repository = new Repository();
            LoginView = new LoginVM();
            LoginView.PropertyChanged += LoginView_PropertyChanged1;
        }

        private void LoginView_PropertyChanged1(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LoginView.IsInitialized))
            {
                // Update CurrentView based on the new SelectedUser
                UsersSideBarVM = new UsersSideBarVM();
                UsersSideBarVM.PropertyChanged += OnUsersSideBarChanged;

                CurrentView = new HomeVM();

                (CurrentView as HomeVM).StartMessageListening();
            }
        }

        public void OnUsersSideBarChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UsersSideBarVM.SelectedUser))
            {
                // Update CurrentView based on the new SelectedUser
                MainVM.Repository.SetSelection(UsersSideBarVM.SelectedUser);
                MainVM.Repository.SetLoggedUser(LoginView.User);
                (CurrentView as HomeVM).ChangeSelection();
            }
        }
    }
}
