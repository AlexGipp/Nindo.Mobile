using System;
using System.Threading.Tasks;
using Nindo.Mobile.Services;
using Nindo.Mobile.ViewModels.BaseViewModels;
using Nindo.Net.Models;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Nindo.Mobile.ViewModels.DetailPages
{
    public class ArtistOverviewPageViewModel : ViewModelBase
    {
        #region Commands

        public IAsyncCommand<ArtistChannel> OpenDetailPageCommand { get; }

        #endregion

        private readonly IApiService _apiService;
        private readonly INavigationService _navigationService;
        private readonly string _artistId;

        public ArtistOverviewPageViewModel(IApiService apiService, INavigationService navigationService, string artistId)
        {
            _apiService = apiService;
            _artistId = artistId;
            _navigationService = navigationService;

            OpenDetailPageCommand = new AsyncCommand<ArtistChannel>(OpenArtistDetailPageAsync, CanExecute);
        }

        private async Task OpenArtistDetailPageAsync(ArtistChannel artist)
        {
            await _navigationService.OpenArtistDetailPageAsync(artist);
        }

        public async Task LoadArtistAsync()
        {
            try
            {
                IsBusy = true;

                await Task.Run(async () =>
                {
                    Artist = _apiService.GetArtistInformationAsync(_artistId).Result;
                    Title = Artist.Name;
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanExecute(object args)
        {
            return !IsBusy;
        }

        #region Properties

        private Artist _artist;
        public Artist Artist
        {
            get => _artist;
            set
            {
                _artist = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}