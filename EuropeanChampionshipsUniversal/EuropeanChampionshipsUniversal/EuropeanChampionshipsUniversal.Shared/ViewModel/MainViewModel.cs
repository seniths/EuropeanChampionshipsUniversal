using EuropeanChampionshipsUniversal.DAL;
using EuropeanChampionshipsUniversal.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EuropeanChampionshipsUniversal.ViewModel
{
    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private IUsersDataAccess da;

        private string login;

        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                RaisePropertyChanged("Login");
            }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                RaisePropertyChanged("Password");
            }
        }

        private INavigationService _navigationService;

        [PreferredConstructor]
        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            da = new UsersAPIAccess();

            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (settings.Values["login"] != null)
                login = settings.Values["login"].ToString();
            if (settings.Values["password"] != null)
                password = settings.Values["password"].ToString();
        }

        private ICommand connexionCommand;

        public ICommand ConnexionCommand
        {
            get
            {
                if (this.connexionCommand == null)
                    this.connexionCommand = new RelayCommand(() => Connexion());
                return this.connexionCommand;
            }
        }

        private void Connexion()
        {   
            IsValidUser();
            
            //loading
        }

        private async void IsValidUser()
        {
            List<User> users = await da.GetUsers();

            foreach (var item in users)
            {
                if (item.login.Equals(login) && item.password.Equals(password))
                    GoToHome(item);
            }
        }

        private void GoToHome(User item)
        {
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
            settings.Values["login"] = login;
            settings.Values["password"] = password;
            _navigationService.NavigateTo("HomePage", item);
        }

        private ICommand goToSubscribeCommand;

        public ICommand GoToSubscribeCommand
        {
            get
            {
                if (this.goToSubscribeCommand == null)
                    this.goToSubscribeCommand = new RelayCommand(() => GoToSubscribe());
                return this.goToSubscribeCommand;
            }
        }

        private void GoToSubscribe()
        {
            _navigationService.NavigateTo("SubscriptionPage");
        }
    }
}
