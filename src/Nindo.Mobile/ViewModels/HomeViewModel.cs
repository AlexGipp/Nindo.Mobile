using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nindo.Common.Common;
using Nindo.Mobile.Services;
using Nindo.Mobile.Services.Implementations;
using Nindo.Mobile.ViewModels.BaseViewModels;
using Nindo.Net.Models;
using Nindo.Net.Models.Enums;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Size = Nindo.Net.Models.Enums.Size;

namespace Nindo.Mobile.ViewModels
{
    public class HomeViewModel : NavigationAwareViewModelBase
    {
        private readonly IApiService _apiService;

        #region commands

        public IAsyncCommand RefreshCommand { get; }
        public Command<string> ChangePlatformCommand { get; }

        #endregion

        public HomeViewModel(IApiService apiService, INavigationService navigationService) : base(navigationService)
        {
            _apiService = apiService;
            Title = "Nindo";

            Items = new RangeObservableCollection<Rank>();

            RefreshCommand = new AsyncCommand(RefreshRanksAsync, CanExecute);
            ChangePlatformCommand = new Command<string>(ChangePlatform, CanExecute);
        }

        public async Task LoadRanksAsync()
        {
            try
            {
                IsBusy = true;

                await Task.Run(async () =>
                {
                    var youtubeTask = _apiService.GetViewsScoreboardAsync(RankViewsPlatform.Youtube, Size.Small);
                    var instTask = _apiService.GetLikesScoreboardAsync(RankLikesPlatform.Instagram, Size.Small);
                    var ttTask = _apiService.GetLikesScoreboardAsync(RankLikesPlatform.TikTok, Size.Small);
                    var twitterTask = _apiService.GetLikesScoreboardAsync(RankLikesPlatform.Twitter, Size.Small);
                    var twitchTask = _apiService.GetViewersScoreboardAsync(Size.Small);

                    var taskList = new List<Task<Rank[]>>
                    {
                        youtubeTask,
                        instTask,
                        ttTask,
                        twitterTask,
                        twitchTask
                    };

                    Task<Rank[]> currentTask = null;

                    while (taskList.Count > 0 && (currentTask = await Task.WhenAny(taskList)) != null)
                    {
                        taskList.Remove(currentTask);

                        if (currentTask == youtubeTask)
                        {
                            Youtube.AddRange(currentTask.Result);
                            Items.AddRange(Youtube);
                            CurrentPlatform ??= "youtube";
                        }
                        else if (currentTask == instTask)
                        {
                            Instagram.AddRange(currentTask.Result);
                        }
                        else if (currentTask == ttTask)
                        {
                            Tiktok.AddRange(currentTask.Result);
                        }
                        else if (currentTask == twitterTask)
                        {
                            Twitter.AddRange(currentTask.Result);
                        }
                        else if (currentTask == twitchTask)
                        {
                            Twitch.AddRange(currentTask.Result);
                        }
                    }
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RefreshRanksAsync()
        {
            ClearCollections();
            CurrentPlatform = "youtube";
            await LoadRanksAsync();
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
                        Items.Clear();
                        Items.AddRange(Youtube);
                        CurrentPlatform = "youtube";
                        break;
                    case "instagram":
                        Items.Clear();
                        Items.AddRange(Instagram);
                        CurrentPlatform = "instagram";
                        break;
                    case "tiktok":
                        Items.Clear();
                        Items.AddRange(Tiktok);
                        CurrentPlatform = "tiktok";
                        break;
                    case "twitter":
                        Items.Clear();
                        Items.AddRange(Twitter);
                        CurrentPlatform = "twitter";
                        break;
                    case "twitch":
                        Items.Clear();
                        Items.AddRange(Twitch);
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

        private bool CanExecute(object arg)
        {
            return !IsBusy;
        }

        private void ClearCollections()
        {
            Items.Clear();
            Youtube.Clear();
            Instagram.Clear();
            Tiktok.Clear();
            Twitter.Clear();
            Twitch.Clear();
        }

        private RangeObservableCollection<Rank> _items;
        public RangeObservableCollection<Rank> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }

        private RangeObservableCollection<Rank> Youtube { get; } = new RangeObservableCollection<Rank>();

        private RangeObservableCollection<Rank> Instagram { get; } = new RangeObservableCollection<Rank>();

        private RangeObservableCollection<Rank> Tiktok { get; } = new RangeObservableCollection<Rank>();

        private RangeObservableCollection<Rank> Twitter { get; } = new RangeObservableCollection<Rank>();

        private RangeObservableCollection<Rank> Twitch { get; } = new RangeObservableCollection<Rank>();

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
    }
}