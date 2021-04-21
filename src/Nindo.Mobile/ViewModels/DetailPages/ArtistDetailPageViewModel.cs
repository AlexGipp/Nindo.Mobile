using System;
using System.Threading.Tasks;
using Nindo.Mobile.Services;
using Nindo.Net.Models;

namespace Nindo.Mobile.ViewModels.DetailPages
{
    public class ArtistDetailPageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;
        private readonly string _artistId;

        public ArtistDetailPageViewModel(IApiService apiService, string artistId)
        {
            _apiService = apiService;
            _artistId = artistId;
        }

        public async Task LoadArtistAsync()
        {
            try
            {
                IsBusy = true;

                await Task.Run(async () =>
                {
                    Artist = _apiService.GetArtistInformationAsync(_artistId).Result;
                });
            }
            finally
            {
                IsBusy = false;
            }
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