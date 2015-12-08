using EuropeanChampionshipsUniversal.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;

namespace EuropeanChampionshipsUniversal.ViewModel
{
    public class ChampionshipsViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private ICommand goToRankingCommand;

        private User currentUser;

        public User CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }

        public int IDBundesliga { get; set; }
        public int IDEreDivisie { get; set; }
        public int IDLigue1 { get; set; }
        public int IDPremierLeaue { get; set; }
        public int IDPrimeiraLiga { get; set; }
        public int IDPrimeraDivision { get; set; }
        public int IDSerieA { get; set; }

        public ICommand GoToRankingCommand
        {
            get
            {
                if (this.goToRankingCommand == null)
                    this.goToRankingCommand = new RelayCommand<int>(GoToRanking);
                return this.goToRankingCommand;
            }
        }

        [PreferredConstructor]
        public ChampionshipsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            IDBundesliga = 394;
            IDEreDivisie = 404;
            IDLigue1 = 396;
            IDPremierLeaue = 398;
            IDPrimeiraLiga = 402;
            IDPrimeraDivision = 399;
            IDSerieA = 401;
        }

        public void GoToRanking(int tag)
        {
            CurrentUserTeam cut = new CurrentUserTeam();
            cut.CurrentUser = currentUser;
            cut.Tag = tag;
            _navigationService.NavigateTo("RankingPage", cut);
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            currentUser = (User)e.Parameter;
        }
    }
}
