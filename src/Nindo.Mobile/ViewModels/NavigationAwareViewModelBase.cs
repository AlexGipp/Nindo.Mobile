using System.Threading.Tasks;
using Nindo.Mobile.Services;
using Nindo.Mobile.Views;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Nindo.Mobile.ViewModels
{
    public class NavigationAwareViewModelBase : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        #region commands

        public IAsyncCommand OpenSearchCommand { get; }
        public IAsyncCommand<string> OpenArtistDetailPageCommand { get; }

        #endregion

        public NavigationAwareViewModelBase(INavigationService navigationService)
        {
            _navigationService = navigationService;

            OpenSearchCommand = new AsyncCommand(NavigateToSearchPageAsync, CanExecute);
            OpenArtistDetailPageCommand = new AsyncCommand<string>(NavigateToArtistDetailPageAsync, CanExecute);
        }

        private async Task NavigateToSearchPageAsync()
        {
            await _navigationService.OpenSearchPageAsync();
        }

        private async Task NavigateToArtistDetailPageAsync(string artistId)
        {
            await _navigationService.OpenArtistDetailPageAsync(artistId);
        }

        private bool CanExecute(object arg)
        {
            return !IsBusy;
        }
    }
}