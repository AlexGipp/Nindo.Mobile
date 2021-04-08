using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Nindo.Mobile.Models;
using Nindo.Mobile.Services;
using Nindo.Net.Models.Enums;

namespace Nindo.Mobile.ViewModels
{
    public class ChartsViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;

        public ChartsViewModel(IApiService apiService)
        {
            Title = "Nindo";

            YoutubeFilters = new List<ChartsFilters>
            {
                new ChartsFilters
                {
                    FilterTitle = "Views",
                    FilterMethod = _apiService.GetViewsScoreboardAsync(RankViewsPlatform.Youtube, Size.Big),
                },
                new ChartsFilters
                {
                    FilterTitle = "Likes",
                    FilterMethod = _apiService.GetLikesScoreboardAsync(RankLikesPlatform.Youtube, Size.Big),
                },
                new ChartsFilters
                {
                    FilterTitle = "Neue Abos",
                    FilterMethod = _apiService.GetSubGainScoreboardAsync(RankAllPlatform.Youtube, Size.Big),
                },
                new ChartsFilters
                {
                    FilterTitle = "Abos",
                    FilterMethod = _apiService.GetSubscribersAsync(RankAllPlatform.Youtube, Size.Big),
                },
                new ChartsFilters
                {
                    FilterTitle = "YouTube-Rang",
                    FilterMethod = _apiService.GetScoreboardAsync(RankAllPlatform.Youtube, Size.Big),
                }
            };
        }


        private IList<ChartsFilters> _youtubeFilters;
        public IList<ChartsFilters> YoutubeFilters
        {
            get => _youtubeFilters;
            set
            {
                _youtubeFilters = value;
                OnPropertyChanged();
            }
        }

        private IList<ChartsFilters> _instagramFilters;
        public IList<ChartsFilters> InstagramFilters
        {
            get => _instagramFilters;
            set
            {
                _instagramFilters = value;
                OnPropertyChanged();
            }
        }


    }
}