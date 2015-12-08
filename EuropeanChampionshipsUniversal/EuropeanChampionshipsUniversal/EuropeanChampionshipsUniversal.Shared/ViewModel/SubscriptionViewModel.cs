using EuropeanChampionshipsUniversal.Common;
using EuropeanChampionshipsUniversal.DAL;
using EuropeanChampionshipsUniversal.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace EuropeanChampionshipsUniversal.ViewModel
{
    public class SubscriptionViewModel : ViewModelBase
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

        private string passwordRepeated;

        public string PasswordRepeated
        {
            get { return passwordRepeated; }
            set { passwordRepeated = value; }
        }

        private string isValidLogin;

        public string IsValidLogin
        {
            get { return isValidLogin; }
            set
            {
                isValidLogin = value;
                RaisePropertyChanged("IsValidLogin");
            }
        }


        private INavigationService _navigationService;

        [PreferredConstructor]
        public SubscriptionViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            da = new UsersAPIAccess();
            isValidLogin = "Collapsed";
        }

        private ICommand subscriptionCommand;

        public ICommand SubscriptionCommand
        {
            get
            {
                if (this.subscriptionCommand == null)
                    this.subscriptionCommand = new RelayCommand(() => Subscribe());
                return this.subscriptionCommand;
            }
        }

        private void Subscribe()
        {
            if(password == passwordRepeated)
                AvailableLogin();
        }

        private async void AvailableLogin()
        {
            List<User> users = await da.GetUsers();

            foreach (var item in users)
            {
                if (item.login.Equals(login))
                {
                    isValidLogin = "Visible";
                    return;
                }
            }

            SaveUser();
        }

        private async void SaveUser()
        {
            User user = new User() { login = login, password = password };
            if (await da.SaveUser(user))
                _navigationService.GoBack();
            else
                isValidLogin = "Visible";
        }
    }
}
