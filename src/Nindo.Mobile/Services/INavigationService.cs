using System.Collections.Generic;
using System.Threading.Tasks;
using Nindo.Net.Models;

namespace Nindo.Mobile.Services
{
    public interface INavigationService
    {
        Task OpenViralDetailPageAsync(IList<Viral> entries);

        Task OpenArtistOverviewPageAsync(string artistId);

        Task OpenArtistDetailPageAsync(ArtistChannel artist);

        Task OpenSearchPageAsync();
    }
}