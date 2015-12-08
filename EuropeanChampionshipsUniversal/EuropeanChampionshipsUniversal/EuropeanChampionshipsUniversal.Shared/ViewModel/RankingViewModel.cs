using EuropeanChampionshipsUniversal.Common;
using EuropeanChampionshipsUniversal.DAL;
using EuropeanChampionshipsUniversal.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;

namespace EuropeanChampionshipsUniversal.ViewModel
{
    public class RankingViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private IChampionshipsDataAccess daTeams;
        private IUsersDataAccess daUsers;

        private INavigationService _navigationService;

        private ObservableCollection<LeagueTableTeam> _teams = new ObservableCollection<LeagueTableTeam>();

        public ObservableCollection<LeagueTableTeam> Teams
        {
            get { return _teams; }
            set
            {
                _teams = value;
                RaisePropertyChanged("Teams");
            }
        }

        private User currentUser;

        public User CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }

        private ICommand goToTeamCommand;

        public ICommand GoToTeamCommand
        {
            get
            {
                if (this.goToTeamCommand == null)
                    this.goToTeamCommand = new RelayCommand<string>(GoToTeam);
                return this.goToTeamCommand;
            }
        }

        [PreferredConstructor]
        public RankingViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            daTeams = new ChampionshipsAPIAccess();
            daUsers = new UsersAPIAccess();
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            CurrentUserTeam cut = (CurrentUserTeam)e.Parameter;
            currentUser = cut.CurrentUser;
            LoadTeams(cut.Tag);
        }

        private async Task LoadTeams(int tag)
        {
            LeagueTable t = await daTeams.GetTeamsByLeagueId(tag);

            _teams.Clear();

            foreach (var item in t.standing)
            {
                _teams.Add(item);
            }
        }

        public async void GoToTeam(string href)
        {
            currentUser = await daUsers.GetUserById(currentUser.idUser);
            CurrentUserTeam cut = new CurrentUserTeam();
            cut.CurrentTeam = await daTeams.GetTeamByLink(href);
            cut.CurrentUser = currentUser;
           
            _navigationService.NavigateTo("TeamPage", cut);
        }

        private ICommand goToHomeCommand;

        public ICommand GoToHomeCommand
        {
            get
            {
                if (this.goToHomeCommand == null)
                    this.goToHomeCommand = new Common.RelayCommand(() => GoToHome());
                return this.goToHomeCommand;
            }
        }

        private ICommand goToFavCommand;

        public ICommand GoToFavCommand
        {
            get
            {
                if (this.goToFavCommand == null)
                    this.goToFavCommand = new Common.RelayCommand(() => GoToFav());
                return this.goToFavCommand;
            }
        }

        private ICommand goToChampionshipsCommand;

        public ICommand GoToChampionshipsCommand
        {
            get
            {
                if (this.goToChampionshipsCommand == null)
                    this.goToChampionshipsCommand = new Common.RelayCommand(() => GoToChampionships());
                return this.goToChampionshipsCommand;
            }
        }

        private void GoToChampionships()
        {
            _navigationService.NavigateTo("ChampionshipsPage", currentUser);
        }

        public void GoToHome()
        {
            _navigationService.NavigateTo("HomePage", currentUser);
        }

        private void GoToFav()
        {
            _navigationService.NavigateTo("FavoritePage", currentUser);
        }
    }
}
