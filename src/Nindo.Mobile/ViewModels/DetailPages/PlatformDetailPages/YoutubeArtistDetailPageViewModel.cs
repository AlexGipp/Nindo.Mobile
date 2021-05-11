using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nindo.Common.Common;
using Nindo.Mobile.Services;
using Nindo.Mobile.ViewModels.BaseViewModels;
using Nindo.Net.Models;

namespace Nindo.Mobile.ViewModels.DetailPages.PlatformDetailPages
{
    public class YoutubeArtistDetailPageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;

        public YoutubeArtistDetailPageViewModel(IApiService apiService)
        {
            _apiService = apiService;

            Title = "Why do i exist?";

            ChannelHistory = new RangeObservableCollection<YoutubeChannel>();
        }

        public async Task GetYoutubeDataAsync(string userId)
        {
            try
            {
                IsBusy = true;

                await Task.Run(async () =>
                {
                    ChannelInfo = await _apiService.GetYouTubeChannelInformationAsync(userId);
                    ChannelHistory.AddRange(await _apiService.GetYouTubeChannelHistoryAsync(userId));
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        #region Properties

        private YoutubeChannel _channelInfo;

        public YoutubeChannel ChannelInfo
        {
            get => _channelInfo;
            set
            {
                _channelInfo = value;
                OnPropertyChanged();
            }
        }

        private RangeObservableCollection<YoutubeChannel> _channelHistory;

        public RangeObservableCollection<YoutubeChannel> ChannelHistory
        {
            get => _channelHistory;
            set
            {
                _channelHistory = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}