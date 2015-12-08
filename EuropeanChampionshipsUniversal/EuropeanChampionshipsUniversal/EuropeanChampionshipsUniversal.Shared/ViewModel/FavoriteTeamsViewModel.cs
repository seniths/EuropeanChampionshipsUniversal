using EuropeanChampionshipsUniversal.DAL;
using EuropeanChampionshipsUniversal.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EuropeanChampionshipsUniversal.ViewModel
{
    public class FavoriteTeamsViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private IChampionshipsDataAccess daTeams;
        private IUsersDataAccess daUsers;

        private ObservableCollection<TeamInfo> teams = new ObservableCollection<TeamInfo>();

        public ObservableCollection<TeamInfo> Teams
        {
            get { return teams; }
            set
            {
                teams = value;
                RaisePropertyChanged("Teams");
            }
        }

        private User currentUser;

        public User CurrentUser
        {
            get { return currentUser; }
            set
            {
                currentUser = value;
                RaisePropertyChanged("CurrentUser");
            }
        }

        private INavigationService _navigationService;

        [PreferredConstructor]
        public FavoriteTeamsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            daTeams = new ChampionshipsAPIAccess();
            daUsers = new UsersAPIAccess();
        }

        private async void LoadUser()
        {
            currentUser = await daUsers.GetUserById(currentUser.idUser);
        }

        public void OnNavigatedTo(User parameter)
        {
            currentUser = parameter;
            LoadFavoriteTeams();
        }

        private async void LoadFavoriteTeams()
        {
            currentUser = await daUsers.GetUserById(currentUser.idUser);
            List<TeamInfo> userTeams = await daTeams.GetUserTeams(currentUser.favoriteteamsusers);


            teams.Clear();
            foreach (var item in userTeams)
            {
                teams.Add(item);
            }
        }

        public void GoToTeam(TeamInfo t)
        {
            CurrentUserTeam cut = new CurrentUserTeam() { CurrentUser = currentUser, CurrentTeam = t };
            _navigationService.NavigateTo("TeamPage", cut);
        }
    }
}
