using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nindo.Common.Common;
using Nindo.Mobile.Services;
using Nindo.Mobile.ViewModels.BaseViewModels;
using Nindo.Net.Models;
using Nindo.Mobile.Models;
using System.Linq;

namespace Nindo.Mobile.ViewModels.DetailPages.PlatformDetailPages
{
    public class YoutubeArtistDetailPageViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;

        public YoutubeArtistDetailPageViewModel(IApiService apiService)
        {
            _apiService = apiService;

            ChannelHistory = new RangeObservableCollection<YoutubeHistoricChannel>();
            LastSevenDays = new RangeObservableCollection<YtChannelHistory>();
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

                Title = ChannelInfo.Name;

                CalculateDifference();

            }
            finally
            {
                IsBusy = false;
            }
        }

        public void CalculateDifference()
        {
            try
            {
                var items = ChannelHistory.Reverse().Take(8).ToList();
                var previousItem = items.First();

                for (int i = 0; i <= 8; i++)
                {
                    var listItem = new YtChannelHistory
                    {
                        Difference = previousItem.Followers - items[1 + i].Followers,
                        Follower = items[i].Followers,
                        Timestamp = items[i].Timestamp.ToString("dd.MM"),
                        Views = previousItem.Views - items[1 + i].Views
                    };
                    LastSevenDays.Add(listItem);
                    previousItem = items[1 + i];
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
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

        private RangeObservableCollection<YoutubeHistoricChannel> _channelHistory;

        public RangeObservableCollection<YoutubeHistoricChannel> ChannelHistory
        {
            get => _channelHistory;
            set
            {
                _channelHistory = value;
                OnPropertyChanged();
            }
        }

        private RangeObservableCollection<YtChannelHistory> _lastSevenDays;

        public RangeObservableCollection<YtChannelHistory> LastSevenDays
        {
            get => _lastSevenDays;
            set
            {
                _lastSevenDays = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}