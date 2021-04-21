using System;
using System.Linq;
using System.Threading.Tasks;
using Nindo.Common.Common;
using Nindo.Mobile.Models;
using Nindo.Mobile.Services;
using Nindo.Net.Models;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Nindo.Mobile.ViewModels
{
    public class ViralViewModel : NavigationAwareViewModelBase
    {
        private readonly IApiService _apiService;
        private readonly INavigationService _navigationService;

        #region commands

        public IAsyncCommand<Platforms> OpenDetailPageCommand { get; }

        #endregion

        public ViralViewModel(IApiService apiService, INavigationService navigationService) : base(navigationService)
        {
            _apiService = apiService;
            _navigationService = navigationService;

            OpenDetailPageCommand = new AsyncCommand<Platforms>(OpenDetailPageAsync, CanExecute);
        }

        public async Task GetViralAsync()
        {
            try
            {
                IsBusy = true;

                var items = await Task.Run(_apiService.GetViralsAsync);
                ViralData.AddRange(items);
            }
            finally
            {
                IsBusy = false;
                OpenDetailPageCommand.RaiseCanExecuteChanged();
            }
        }

        private async Task OpenDetailPageAsync(Platforms type)
        {
            try
            {
                IsBusy = true;

                var entries = type switch
                {
                    Platforms.Youtube => ViralData.Where(x => "youtube".Equals(x.Platform)),
                    Platforms.Instagram => ViralData.Where(x => "instagram".Equals(x.Platform)),
                    Platforms.Tiktok => ViralData.Where(x => "tiktok".Equals(x.Platform)),
                    Platforms.Twitter => ViralData.Where(x => "twitter".Equals(x.Platform)),
                    Platforms.Twitch => ViralData.Where(x => "twitch".Equals(x.Platform)),
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                };

                await _navigationService.OpenViralDetailPage(entries.ToList());
            }
            finally
            {
                IsBusy = false;
            }

        }

        private bool CanExecute(object arg)
        {
            return !IsBusy && ViralData.Any();
        }

        public RangeObservableCollection<Viral> ViralData { get; } = new RangeObservableCollection<Viral>();
    }
}