using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nindo.Common.Common;
using Nindo.Mobile.Models;
using Nindo.Mobile.Services;
using Nindo.Net.Models;
using Nindo.Net.Models.Enums;
using Xamarin.CommunityToolkit.ObjectModel;
using Size = Nindo.Net.Models.Enums.Size;

namespace Nindo.Mobile.ViewModels
{
    public class ChartsViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;

        #region commands

        public IAsyncCommand<string> ChangePlatformCommand { get; }
        public IAsyncCommand<ChartsFilter> ChangeFilterCommand { get; }
        public IAsyncCommand RefreshCommand { get; }


        #endregion

        public ChartsViewModel(IApiService apiService)
        {
            _apiService = apiService;
            Title = "Nindo";

            ChangePlatformCommand = new AsyncCommand<string>(ChangePlatform, CanExecute);
            ChangeFilterCommand = new AsyncCommand<ChartsFilter>(ChangeFilterAsync, CanExecute);
            RefreshCommand = new AsyncCommand(RefreshCurrentChartsAsync, CanExecute);

            ResultItems = new RangeObservableCollection<Rank>();
            FilterItems = new RangeObservableCollection<ChartsFilter>();
            YoutubeFilters = new List<ChartsFilter>
            {
                new ChartsFilter
                {
                    FilterTitle = "Views",
                    HeaderText = "Ø nach 5 Tagen",
                    FilterMethod = async () => await _apiService.GetViewsScoreboardAsync(RankViewsPlatform.Youtube, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Likes",
                    HeaderText = "Ø nach 5 Tagen",
                    FilterMethod = async () => await _apiService.GetLikesScoreboardAsync(RankLikesPlatform.Youtube, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Neue Abos",
                    HeaderText = "Zuwachs in den letzten 30 Tagen",
                    FilterMethod = async () => await _apiService.GetSubGainScoreboardAsync(RankAllPlatform.Youtube, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Abos",
                    HeaderText = string.Empty,
                    FilterMethod = async () => await _apiService.GetSubscribersAsync(RankAllPlatform.Youtube, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "YouTube-Rang",
                    HeaderText = "Berechnet sich aus allen Chart-Platzierungen",
                    FilterMethod = async () => await _apiService.GetScoreboardAsync(RankAllPlatform.Youtube, Size.Big),
                }
            };
            InstagramFilters = new List<ChartsFilter>
            {
                new ChartsFilter
                {
                    FilterTitle = "Likes",
                    HeaderText = "Ø nach 5 Tagen",
                    FilterMethod = async () => await _apiService.GetLikesScoreboardAsync(RankLikesPlatform.Instagram, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Neue Follower",
                    HeaderText = "Zuwachs in den letzten 30 Tagen",
                    FilterMethod = async () => await _apiService.GetSubGainScoreboardAsync(RankAllPlatform.Instagram, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Follower",
                    HeaderText = string.Empty,
                    FilterMethod = async () => await _apiService.GetSubscribersAsync(RankAllPlatform.Instagram, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Instagram-Rang",
                    HeaderText = "Berechnet sich aus allen Chart-Platzierungen",
                    FilterMethod = async () => await _apiService.GetScoreboardAsync(RankAllPlatform.Instagram, Size.Big),
                },
            };
            TiktokFilters = new List<ChartsFilter>
            {
                new ChartsFilter
                {
                    FilterTitle = "Likes",
                    HeaderText = "Ø nach 5 Tagen",
                    FilterMethod = async () => await _apiService.GetLikesScoreboardAsync(RankLikesPlatform.TikTok, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Views",
                    HeaderText = "Ø nach 5 Tagen",
                    FilterMethod = async () => await _apiService.GetViewsScoreboardAsync(RankViewsPlatform.TikTok, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Follower",
                    HeaderText = string.Empty,
                    FilterMethod = async () => await _apiService.GetSubscribersAsync(RankAllPlatform.TikTok, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Tiktok-Rang",
                    HeaderText = "Berechnet sich aus allen Chart-Platzierungen",
                    FilterMethod = async () => await _apiService.GetScoreboardAsync(RankAllPlatform.TikTok, Size.Big),
                },
            };
            TwitterFilters = new List<ChartsFilter>
            {
                new ChartsFilter
                {
                    FilterTitle = "Likes",
                    HeaderText = "Ø nach 5 Tagen",
                    FilterMethod = async () => await _apiService.GetLikesScoreboardAsync(RankLikesPlatform.Twitter, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Retweets",
                    HeaderText = "Ø nach 5 Tagen",
                    FilterMethod = async () => await _apiService.GetRetweetsScoreboardAsync(Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Neue Follower",
                    HeaderText = "Zuwachs in den letzten 30 Tagen",
                    FilterMethod = async () => await _apiService.GetSubGainScoreboardAsync(RankAllPlatform.Twitter, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Follower",
                    HeaderText = string.Empty,
                    FilterMethod = async () => await _apiService.GetSubscribersAsync(RankAllPlatform.Twitter, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Twitter-Rang",
                    HeaderText = "Berechnet sich aus allen Chart-Platzierungen",
                    FilterMethod = async () => await _apiService.GetScoreboardAsync(RankAllPlatform.Twitter, Size.Big),
                },
            };
            TwitchFilters = new List<ChartsFilter>
            {
                new ChartsFilter
                {
                    FilterTitle = "Viewer",
                    HeaderText = "Ø in den letzten 30 Tagen",
                    FilterMethod = async () => await _apiService.GetViewersScoreboardAsync(Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Peak Viewer",
                    HeaderText = "Höchstwert der letzten 30 Tage",
                    FilterMethod = async () => await _apiService.GetPeakViewersScoreboardAsync(Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Neue Follower",
                    HeaderText = "Zuwachs in den letzten 30 Tage",
                    FilterMethod = async () => await _apiService.GetSubGainScoreboardAsync(RankAllPlatform.Twitch, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Watchtime",
                    HeaderText = "Summe der letzten 30 Tage",
                    FilterMethod = async () => await _apiService.GetWatchtimeScoreboardAsync(Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Follower",
                    HeaderText = string.Empty,
                    FilterMethod = async () => await _apiService.GetSubscribersAsync(RankAllPlatform.Twitch, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Twitch-Rang",
                    HeaderText = "Berechnet sich aus allen Chart-Platzierungen",
                    FilterMethod = async () => await _apiService.GetScoreboardAsync(RankAllPlatform.Twitch, Size.Big),
                },
            };
        }

        public async Task LoadDefaultData()
        {
            if (YoutubeFilters != null)
            {
                await ChangePlatform("youtube");
                await ChangeFilterAsync(YoutubeFilters.First());
            }
        }

        private async Task ChangePlatform(string platform)
        {
            try
            {
                IsBusy = true;
                if (CurrentPlatform == platform)
                    return;

                switch (platform)
                {
                    case "youtube":
                        FilterItems.Clear();
                        FilterItems.AddRange(YoutubeFilters);

                        await ChangeFilterAsync(YoutubeFilters.First());

                        CurrentPlatform = "youtube";
                        break;
                    case "instagram":
                        FilterItems.Clear();
                        FilterItems.AddRange(InstagramFilters);

                        await ChangeFilterAsync(InstagramFilters.First());

                        CurrentPlatform = "instagram";
                        break;
                    case "tiktok":
                        FilterItems.Clear();
                        FilterItems.AddRange(TiktokFilters);

                        await ChangeFilterAsync(TiktokFilters.First());

                        CurrentPlatform = "tiktok";
                        break;
                    case "twitter":
                        FilterItems.Clear();
                        FilterItems.AddRange(TwitterFilters);

                        await ChangeFilterAsync(TwitterFilters.First());

                        CurrentPlatform = "twitter";
                        break;
                    case "twitch":
                        FilterItems.Clear();
                        FilterItems.AddRange(TwitchFilters);

                        await ChangeFilterAsync(TwitchFilters.First());

                        CurrentPlatform = "twitch";
                        break;
                    default:
                        throw new InvalidOperationException("Invalid platform!");
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ChangeFilterAsync(ChartsFilter selectedFilter)
        {
            try
            {
                IsBusy = true;

                ResultItems.Clear();
                SelectedPickerItem = selectedFilter;
                ResultItems.AddRange(await selectedFilter.FilterMethod());
                CollectionViewHeaderText = selectedFilter.HeaderText;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RefreshCurrentChartsAsync()
        {
            await ChangeFilterAsync(SelectedPickerItem);
        }

        private bool CanExecute(object arg)
        {
            return !IsBusy;
        }

        private string _currentPlatform;
        public string CurrentPlatform
        {
            get => _currentPlatform;
            set
            {
                if (_currentPlatform == value) return;
                _currentPlatform = value;
                OnPropertyChanged();
            }
        }


        private IList<ChartsFilter> _youtubeFilters;
        public IList<ChartsFilter> YoutubeFilters
        {
            get => _youtubeFilters;
            set
            {
                _youtubeFilters = value;
                OnPropertyChanged();
            }
        }

        private IList<ChartsFilter> _instagramFilters;
        public IList<ChartsFilter> InstagramFilters
        {
            get => _instagramFilters;
            set
            {
                _instagramFilters = value;
                OnPropertyChanged();
            }
        }

        private IList<ChartsFilter> _tiktokFilters;
        public IList<ChartsFilter> TiktokFilters
        {
            get => _tiktokFilters;
            set
            {
                _tiktokFilters = value;
                OnPropertyChanged();
            }
        }

        private IList<ChartsFilter> _twitterFilters;
        public IList<ChartsFilter> TwitterFilters
        {
            get => _twitterFilters;
            set
            {
                _twitterFilters = value;
                OnPropertyChanged();
            }
        }

        private IList<ChartsFilter> _twitchFilters;
        public IList<ChartsFilter> TwitchFilters
        {
            get => _twitchFilters;
            set
            {
                _twitchFilters = value;
                OnPropertyChanged();
            }
        }

        private RangeObservableCollection<ChartsFilter> _filterItems;
        public RangeObservableCollection<ChartsFilter> FilterItems
        {
            get => _filterItems;
            set
            {
                _filterItems = value;
                OnPropertyChanged();
            }
        }

        private RangeObservableCollection<Rank> _resultItems;

        public RangeObservableCollection<Rank> ResultItems
        {
            get => _resultItems;
            set
            {
                _resultItems = value;
                OnPropertyChanged();
            }
        }

        private ChartsFilter _selectedPickerItem;

        public ChartsFilter SelectedPickerItem
        {
            get => _selectedPickerItem;
            set
            {
                _selectedPickerItem = value;
                OnPropertyChanged();
            }
        }

        private string _collectionViewHeaderText;

        public string CollectionViewHeaderText
        {
            get => _collectionViewHeaderText;
            set
            {
                _collectionViewHeaderText = value;
                OnPropertyChanged();
            }
        }
    }
}