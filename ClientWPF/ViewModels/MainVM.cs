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
        private Visibility _buttonVisibility;

        public User SelectedUser { get; set; }

        public Visibility buttonVisibility { get => _buttonVisibility; set { _buttonVisibility = value; OnPropertyChanged(); } }
        public ICommand HomeCommand { get; set; }
        public ICommand UserCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand MinimalizeCommand { get; set; }

        public UsersSideBarVM UsersSideBarVM { get => _usersSideBarVM; set { _usersSideBarVM = value; OnPropertyChanged(); } }
        public LoginVM LoginView { get; set; }
        public UserVM UserVM { get; set; }

        public HomeVM HomeVM { get; set; }

        public void Home(object obj)
        {
            if (!(CurrentView is HomeVM))
            {
                CurrentView = HomeVM;
            }
        }
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
            LogoutCommand = new RelayCommand(Logout);
            ExitCommand = new RelayCommand(Exit);
            MinimalizeCommand = new RelayCommand(Minimalize);
            Repository = new Repository();
            LoginView = new LoginVM();
            LoginView.PropertyChanged += LoginView_PropertyChanged1;
            buttonVisibility = Visibility.Hidden;
        }

        private void LoginView_PropertyChanged1(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LoginView.IsInitialized))
            {
                // Update CurrentView based on the new SelectedUser
                UsersSideBarVM = new UsersSideBarVM();
                UsersSideBarVM.PropertyChanged += OnUsersSideBarChanged;

                HomeVM = new HomeVM();
                CurrentView = HomeVM;

                (CurrentView as HomeVM).StartMessageListening();
                buttonVisibility = Visibility.Visible;
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
        public void Logout(object obj)
        {
            (CurrentView as HomeVM).StopMessageListening();
            LoginView.ViewVisibility = Visibility.Visible;
            buttonVisibility = Visibility.Hidden;
        }
        public void UserViewAction(object obj)
        {
            UserVM = new UserVM();
            UserVM.PropertyChanged += UserVM_PropertyChanged;
            buttonVisibility = Visibility.Hidden;
        }

        private void UserVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UserVM.AddedUser))
            {
                // TODO: make added visual to usersidebar
                UsersSideBarVM.Users = new System.Collections.ObjectModel.ObservableCollection<User>(Repository.Users);
                buttonVisibility = Visibility.Visible;
            }
        }
    }
}
