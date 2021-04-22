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
        public IAsyncCommand<Rank> OpenArtistDetailPageCommand { get; }

        #endregion

        public NavigationAwareViewModelBase(INavigationService navigationService)
        {
            _navigationService = navigationService;

            OpenSearchCommand = new AsyncCommand(NavigateToSearchPageAsync, CanExecute);
            OpenArtistDetailPageCommand = new AsyncCommand<Rank>(NavigateToArtistDetailPageAsync, CanExecute);
        }

        private async Task NavigateToSearchPageAsync()
        {
            await _navigationService.OpenSearchPageAsync();
        }

        private async Task NavigateToArtistDetailPageAsync(Rank selectedRank)
        {
            await _navigationService.OpenArtistDetailPageAsync(selectedRank.ArtistId);
        }

        private bool CanExecute(object arg)
        {
            return !IsBusy;
        }
    }
}