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
                ResultItems.AddRange(await selectedFilter.FilterMethod.Invoke());
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