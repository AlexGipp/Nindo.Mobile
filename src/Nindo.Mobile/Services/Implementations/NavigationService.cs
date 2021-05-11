using System.Collections.Generic;
using System.Threading.Tasks;
using Nindo.Mobile.Views;
using Nindo.Mobile.Views.DetailPages;
using Nindo.Mobile.Views.DetailPages.PlatformDetailPages;
using Nindo.Net.Models;
using Xamarin.Forms;

namespace Nindo.Mobile.Services.Implementations
{
    public class NavigationService : INavigationService
    {
        public async Task OpenViralDetailPageAsync(IList<Viral> entries)
        {
            if (entries != null)
                await Application.Current.MainPage.Navigation.PushAsync(new ViralDetailPage(entries), true);
        }

        public async Task OpenArtistOverviewPageAsync(string artistId)
        {
            if (!string.IsNullOrEmpty(artistId))
                await Application.Current.MainPage.Navigation.PushAsync(new ArtistOverviewPage(artistId), true);
        }

        public async Task OpenArtistDetailPageAsync(ArtistChannel artist)
        {
            var id = artist.Id;
            var page = artist.Platform switch
            {
                "twitch" =>  new YoutubeArtistDetailPage(id),
                "instagram" => new YoutubeArtistDetailPage(id),
                "twitter" =>  new YoutubeArtistDetailPage(id),
                "tiktok" =>  new YoutubeArtistDetailPage(id),
                "youtube" =>  new YoutubeArtistDetailPage(id),
                _ => new YoutubeArtistDetailPage(id)
            };

            await Application.Current.MainPage.Navigation.PushAsync(page, true);
        }

        public async Task OpenSearchPageAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SearchPage());
        }
    }
}