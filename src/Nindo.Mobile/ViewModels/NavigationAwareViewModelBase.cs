using System.Threading.Tasks;
using Nindo.Mobile.Services;
using Nindo.Mobile.Views;
using Nindo.Net.Models;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Nindo.Mobile.ViewModels
{
    public class NavigationAwareViewModelBase : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        #region commands

        public IAsyncCommand OpenSearchCommand { get; }
        public IAsyncCommand OpenArtistDetailPageCommand { get; }

        #endregion

        public NavigationAwareViewModelBase(INavigationService navigationService)
        {
            _navigationService = navigationService;

            OpenSearchCommand = new AsyncCommand(NavigateToSearchPageAsync, CanExecute);
            OpenArtistDetailPageCommand = new AsyncCommand(NavigateToArtistDetailPageAsync, CanExecute);
        }

        private async Task NavigateToSearchPageAsync()
        {
            await _navigationService.OpenSearchPageAsync();
        }

        private async Task NavigateToArtistDetailPageAsync()
        {
            await _navigationService.OpenArtistDetailPageAsync(SelectedRank.ArtistId);
        }

        private bool CanExecute(object arg)
        {
            return !IsBusy;
        }

        #region properties

        private Rank _selectedRank;

        public Rank SelectedRank
        {
            get => _selectedRank;
            set
            {
                _selectedRank = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}