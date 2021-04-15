using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Nindo.Common.Common;
using Nindo.Mobile.Models;
using Nindo.Mobile.Services;
using Nindo.Net.Models;
using Nindo.Net.Models.Enums;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Size = Nindo.Net.Models.Enums.Size;

namespace Nindo.Mobile.ViewModels
{
    public class ChartsViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;

        #region commands

        public ICommand ChangePlatformCommand { get; }
        public IAsyncCommand<ChartsFilter> ChangeFilterCommand { get; }

        #endregion

        public ChartsViewModel(IApiService apiService)
        {
            _apiService = apiService;
            Title = "Nindo";

            ChangePlatformCommand = new Command<string>(ChangePlatform, CanExecute);
            ChangeFilterCommand = new AsyncCommand<ChartsFilter>(ChangeFilterAsync, CanExecute);

            ResultItems = new RangeObservableCollection<Rank>();
            FilterItems = new RangeObservableCollection<ChartsFilter>();
            YoutubeFilters = new List<ChartsFilter>
            {
                new ChartsFilter
                {
                    FilterTitle = "Views",
                    FilterMethod = async () => await _apiService.GetViewsScoreboardAsync(RankViewsPlatform.Youtube, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Likes",
                    FilterMethod = async () => await _apiService.GetLikesScoreboardAsync(RankLikesPlatform.Youtube, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Neue Abos",
                    FilterMethod = async () => await _apiService.GetSubGainScoreboardAsync(RankAllPlatform.Youtube, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Abos",
                    FilterMethod = async () => await _apiService.GetSubscribersAsync(RankAllPlatform.Youtube, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "YouTube-Rang",
                    FilterMethod = async () => await _apiService.GetScoreboardAsync(RankAllPlatform.Youtube, Size.Big),
                }
            };
            InstagramFilters = new List<ChartsFilter>
            {
                new ChartsFilter
                {
                    FilterTitle = "Likes",
                    FilterMethod = async () => await _apiService.GetLikesScoreboardAsync(RankLikesPlatform.Instagram, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Neue Follower",
                    FilterMethod = async () => await _apiService.GetSubGainScoreboardAsync(RankAllPlatform.Instagram, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Follower",
                    FilterMethod = async () => await _apiService.GetSubscribersAsync(RankAllPlatform.Instagram, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Instagram-Rang",
                    FilterMethod = async () => await _apiService.GetScoreboardAsync(RankAllPlatform.Instagram, Size.Big),
                },
            };
            InstagramFilters = new List<ChartsFilter>
            {
                new ChartsFilter
                {
                    FilterTitle = "Likes",
                    FilterMethod = async () => await _apiService.GetLikesScoreboardAsync(RankLikesPlatform.Instagram, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Neue Follower",
                    FilterMethod = async () => await _apiService.GetSubGainScoreboardAsync(RankAllPlatform.Instagram, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Follower",
                    FilterMethod = async () => await _apiService.GetSubscribersAsync(RankAllPlatform.Instagram, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Instagram-Rang",
                    FilterMethod = async () => await _apiService.GetScoreboardAsync(RankAllPlatform.Instagram, Size.Big),
                },
            };
            TiktokFilters = new List<ChartsFilter>
            {
                new ChartsFilter
                {
                    FilterTitle = "Likes",
                    FilterMethod = async () => await _apiService.GetLikesScoreboardAsync(RankLikesPlatform.TikTok, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Views",
                    FilterMethod = async () => await _apiService.GetViewsScoreboardAsync(RankViewsPlatform.TikTok, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Follower",
                    FilterMethod = async () => await _apiService.GetSubscribersAsync(RankAllPlatform.TikTok, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Tiktok-Rang",
                    FilterMethod = async () => await _apiService.GetScoreboardAsync(RankAllPlatform.TikTok, Size.Big),
                },
            };
            TwitterFilters = new List<ChartsFilter>
            {
                new ChartsFilter
                {
                    FilterTitle = "Likes",
                    FilterMethod = async () => await _apiService.GetLikesScoreboardAsync(RankLikesPlatform.Twitter, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Retweets",
                    FilterMethod = async () => await _apiService.GetRetweetsScoreboardAsync(Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Neue Follower",
                    FilterMethod = async () => await _apiService.GetSubGainScoreboardAsync(RankAllPlatform.Twitter, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Follower",
                    FilterMethod = async () => await _apiService.GetSubscribersAsync(RankAllPlatform.Twitter, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Twitter-Rang",
                    FilterMethod = async () => await _apiService.GetScoreboardAsync(RankAllPlatform.Twitter, Size.Big),
                },
            };
            TwitchFilters = new List<ChartsFilter>
            {
                new ChartsFilter
                {
                    FilterTitle = "Viewer",
                    FilterMethod = async () => await _apiService.GetViewersScoreboardAsync(Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Peak Viewer",
                    FilterMethod = async () => await _apiService.GetPeakViewersScoreboardAsync(Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Neue Follower",
                    FilterMethod = async () => await _apiService.GetSubGainScoreboardAsync(RankAllPlatform.Twitch, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Watchtime",
                    FilterMethod = async () => await _apiService.GetWatchtimeScoreboardAsync(Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Follower",
                    FilterMethod = async () => await _apiService.GetSubscribersAsync(RankAllPlatform.Twitch, Size.Big),
                },
                new ChartsFilter
                {
                    FilterTitle = "Twitch-Rang",
                    FilterMethod = async () => await _apiService.GetScoreboardAsync(RankAllPlatform.Twitch, Size.Big),
                },
            };
        }

        private void ChangePlatform(string platform)
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
                        CurrentPlatform = "youtube";
                        break;
                    case "instagram":
                        FilterItems.Clear();
                        FilterItems.AddRange(InstagramFilters);
                        CurrentPlatform = "instagram";
                        break;
                    case "tiktok":
                        FilterItems.Clear();
                        FilterItems.AddRange(TiktokFilters);
                        CurrentPlatform = "tiktok";
                        break;
                    case "twitter":
                        FilterItems.Clear();
                        FilterItems.AddRange(TwitterFilters);
                        CurrentPlatform = "twitter";
                        break;
                    case "twitch":
                        FilterItems.Clear();
                        FilterItems.AddRange(TwitchFilters);
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
                ResultItems.AddRange(await selectedFilter.FilterMethod());
            }
            finally
            {
                IsBusy = false;
            }
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
    }
}