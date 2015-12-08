using EuropeanChampionshipsUniversal.DAL;
using EuropeanChampionshipsUniversal.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace EuropeanChampionshipsUniversal.ViewModel
{
    public class TeamViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private IChampionshipsDataAccess daTeams;
        private IUsersDataAccess daUsers;

        private TeamInfo _selectedTeam = new TeamInfo();

        public TeamInfo SelectedTeam
        {
            get { return _selectedTeam; }
            set
            {
                _selectedTeam = value;
                RaisePropertyChanged("SelectedTeam");
            }
        }

        private User currentUser;

        public User CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }


        private string imageVisibility;

        public string ImageVisibility
        {
            get { return imageVisibility; }
            set { imageVisibility = value; }
        }

        private string webViewVisibility;

        public string WebViewVisibility
        {
            get { return webViewVisibility; }
            set { webViewVisibility = value; }
        }

        private BitmapImage iconFav;

        public BitmapImage IconFav
        {
            get { return iconFav; }
            set
            {
                iconFav = value;
                RaisePropertyChanged("IconFav");
            }
        }

        private bool isFavorite;

        public bool IsFavorite
        {
            get { return isFavorite; }
            set { isFavorite = value; }
        }


        private INavigationService _navigationService;

        [PreferredConstructor]
        public TeamViewModel(INavigationService navigationService = null)
        {
            _navigationService = navigationService;
            daTeams = new ChampionshipsAPIAccess();
            daUsers = new UsersAPIAccess();
            isFavorite = false;
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            CurrentUserTeam cut = (CurrentUserTeam)e.Parameter;
            _selectedTeam = cut.CurrentTeam;
            currentUser = cut.CurrentUser;
            iconFav = new BitmapImage(new Uri("ms-appx:///Assets/Star-Empty.png"));
            foreach (var item in currentUser.favoriteteamsusers)
            {
                if (item.idTeam == _selectedTeam.id)
                {
                    iconFav = new BitmapImage(new Uri("ms-appx:///Assets/Star-Full.png"));
                    isFavorite = true;
                }
            }

            if (_selectedTeam.crestUrl.EndsWith("svg"))
            {
                webViewVisibility = "Visible";
                imageVisibility = "Collapsed";
            }
            else
            {
                webViewVisibility = "Collapsed";
                imageVisibility = "Visible";
            }
        }

        private ICommand _goToCompositionCommand;

        public ICommand GoToCompositionCommand
        {
            get
            {
                if (this._goToCompositionCommand == null)
                    this._goToCompositionCommand = new RelayCommand(() => GoToComposition());
                return this._goToCompositionCommand;
            }
        }

        private async void GoToComposition()
        {
            TeamPlayers tp = await daTeams.GetTeamComposition(_selectedTeam._links.players.href);
            _navigationService.NavigateTo("CompositionPage", tp);
        }

        private ICommand _addRemoveFavoriteCommand;

        public ICommand AddRemoveFavoriteCommand
        {
            get
            {
                if (this._addRemoveFavoriteCommand == null)
                    this._addRemoveFavoriteCommand = new RelayCommand(() => AddRemoveFavorite());
                return this._addRemoveFavoriteCommand;
            }
        }

        private async void AddRemoveFavorite()
        {
            //if (await daUsers.AddFavoriteTeam(currentUser.idUser, _selectedTeam.id))
            //    iconFav = "Assets/ic_favorite_white_24dp.png";

            //recharger user + unfav

            //if (iconFav.UriSource.ToString() == "ms-appx:/Assets/Star-Full.png")
            //    iconFav.UriSource = new Uri("ms-appx:///Assets/Star-Empty.png");
            //else
            //    iconFav.UriSource = new Uri("ms-appx:///Assets/Star-Full.png");

            if (isFavorite)
            {
                int idToDelete = 0;

                foreach (var item in currentUser.favoriteteamsusers)
                {
                    if (item.idTeam == _selectedTeam.id)
                        idToDelete = item.idFavoriteTeamsUser;
                }
                if (await daUsers.RemoveFavoriteTeam(idToDelete))
                {
                    iconFav.UriSource = new Uri("ms-appx:///Assets/Star-Empty.png");
                    isFavorite = false;
                }
            }
            else
            {
                if (await daUsers.AddFavoriteTeam(currentUser.idUser, _selectedTeam.id))
                {
                    iconFav.UriSource = new Uri("ms-appx:///Assets/Star-Full.png");
                    isFavorite = true;
                }
            }

            currentUser = await daUsers.GetUserById(currentUser.idUser);
        }
    }
}
