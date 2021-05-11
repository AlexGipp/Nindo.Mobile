using System.Threading.Tasks;
using Nindo.Mobile.Services;
using Nindo.Net.Models;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Nindo.Mobile.ViewModels.BaseViewModels
{
    public class NavigationAwareViewModelBase : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        #region commands

        public IAsyncCommand OpenSearchCommand { get; }
        public IAsyncCommand<Rank> OpenArtistOverviewPageCommand { get; }

        #endregion

        public NavigationAwareViewModelBase(INavigationService navigationService)
        {
            _navigationService = navigationService;

            OpenSearchCommand = new AsyncCommand(NavigateToSearchPageAsync, CanExecute);
            OpenArtistOverviewPageCommand = new AsyncCommand<Rank>(NavigateToArtistDetailPageAsync, CanExecute);
        }

        private async Task NavigateToSearchPageAsync()
        {
            await _navigationService.OpenSearchPageAsync();
        }

        private async Task NavigateToArtistDetailPageAsync(Rank selectedRank)
        {
            await _navigationService.OpenArtistOverviewPageAsync(selectedRank.ArtistId);
        }

        private bool CanExecute(object arg)
        {
            return !IsBusy;
        }
    }
}