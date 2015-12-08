using EuropeanChampionshipsUniversal.Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using EuropeanChampionshipsUniversal.Model;

namespace EuropeanChampionshipsUniversal.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private INavigationService _navigationService;

        private User currentUser;

        public User CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }


        [PreferredConstructor]
        public HomeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private ICommand _goToFavoriteCommand;

        public ICommand GoToFavoriteCommand
        {
            get
            {
                if (this._goToFavoriteCommand == null)
                    this._goToFavoriteCommand = new RelayCommand(() => GoToFavorite());
                return this._goToFavoriteCommand;
            }
        }

        private ICommand _goToChampionshipsCommand;

        public ICommand GoToChampionshipsCommand
        {
            get
            {
                if (this._goToChampionshipsCommand == null)
                    this._goToChampionshipsCommand = new RelayCommand(() => GoToChampionships());
                return this._goToChampionshipsCommand;
            }
        }

        public void OnNavigatedTo(User parameter)
        {
            currentUser = parameter;
        }

        private void GoToFavorite()
        {
            _navigationService.NavigateTo("FavoritePage", currentUser);
        }

        private void GoToChampionships()
        {
            _navigationService.NavigateTo("ChampionshipsPage", currentUser);
        }
    }
}
