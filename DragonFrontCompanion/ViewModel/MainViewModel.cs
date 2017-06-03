
using DragonFrontCompanion.Data;
using DragonFrontDb;
using DragonFrontDb.Enums;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace DragonFrontCompanion.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private IDialogService _dialogService;
        private ICardsService _cardsService;
        private bool _cardDataReset;
        public MainViewModel(INavigationService navigationService, ICardsService cardsService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _cardsService = cardsService;

            VersionDisplay = "v" + App.VersionName;
            Title = App.APP_NAME;

            cardsService.DataUpdateAvailable += CardsService_DataUpdateAvailable;
            cardsService.DataUpdated += CardsService_DataUpdated;
            if (Settings.EnableAutoUpdate) cardsService.CheckForUpdatesAsync();
        }

        private void CardsService_DataUpdated(object sender, Cards e)
        {
            _cardDataReset = Settings.ActiveCardDataVersion == null;

            Device.BeginInvokeOnMainThread(() =>
            {
                MessagingCenter.Send<string>("Loaded Card Data v" + (Settings.ActiveCardDataVersion ?? Info.Current.CardDataVersion.ToString()), App.MESSAGES.SHOW_TOAST);
            });
        }

        private void CardsService_DataUpdateAvailable(object sender, Info e)
        {
            if (_cardDataReset || 
                !Settings.EnableAutoUpdate || 
                e.CardDataStatus != DataStatus.RELEASE) return;

            Device.BeginInvokeOnMainThread(() =>
            {
                MessagingCenter.Send<string>("Updating Card Data", App.MESSAGES.SHOW_TOAST);
            });
            _cardsService.UpdateCardDataAsync();
        }

        #region Properties

        private string _versionDisplay = "";
        public string VersionDisplay
        {
            get { return _versionDisplay; }
            set { Set(ref _versionDisplay, value); }
        }


        private string _title = "";
        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }


        private bool _hasNavigated = false;
        public bool HasNavigated
        {
            get { return _hasNavigated; }
            set { Set(ref _hasNavigated, value); }
        }

        #endregion

        #region Commands
        private RelayCommand _navigateToDecks;

        /// <summary>
        /// Gets the NavigateToDecks.
        /// </summary>
        public RelayCommand NavigateToDecksCommand
        {
            get
            {
                return _navigateToDecks
                    ?? (_navigateToDecks = new RelayCommand(
                    () =>
                    {
                        if (HasNavigated) return;
                        HasNavigated = true;
                        _navigationService.NavigateTo(ViewModelLocator.DecksPageKey);
                    }));
            }
        }

        private RelayCommand _navigateToCards;

        /// <summary>
        /// Gets the NavigateToCardsCommand.
        /// </summary>
        public RelayCommand NavigateToCardsCommand
        {
            get
            {
                return _navigateToCards
                    ?? (_navigateToCards = new RelayCommand(
                    () =>
                    {
                        if (HasNavigated) return;
                        HasNavigated = true;
                        _navigationService.NavigateTo(ViewModelLocator.CardsPageKey);
                    }));
            }
        }

        private RelayCommand _navigateToAbout;

        /// <summary>
        /// Gets the NavigateToAboutCommand.
        /// </summary>
        public RelayCommand NavigateToAboutCommand
        {
            get
            {
                return _navigateToAbout
                    ?? (_navigateToAbout = new RelayCommand(
                    () =>
                    {
                        if (HasNavigated) return;
                        HasNavigated = true;
                        _navigationService.NavigateTo(ViewModelLocator.AboutPageKey);
                    }));
            }
        }

        private RelayCommand _navigateToSettings;

        /// <summary>
        /// Gets the NavigateToSettingsCommand.
        /// </summary>
        public RelayCommand NavigateToSettingsCommand
        {
            get
            {
                return _navigateToSettings
                    ?? (_navigateToSettings = new RelayCommand(
                    () =>
                    {
                        if (HasNavigated) return;
                        HasNavigated = true;
                        _navigationService.NavigateTo(ViewModelLocator.SettingsPageKey);
                  
                    }));
            }
        }

        private RelayCommand<string> _navigateToNewCards;

        /// <summary>
        /// Gets the NavigateToCardsCommand.
        /// </summary>
        public RelayCommand<string> NavigateToNewCardsCommand
        {
            get
            {
                return _navigateToNewCards
                    ?? (_navigateToNewCards = new RelayCommand<string>(
                    (searchText) =>
                    {
                        if (HasNavigated) return;
                        HasNavigated = true;
                        _navigationService.NavigateTo(ViewModelLocator.CardsPageKey, searchText);
                    }));
            }
        }
        #endregion  


    }
}