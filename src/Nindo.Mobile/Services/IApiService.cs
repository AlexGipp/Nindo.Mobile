using System.Threading.Tasks;
using Nindo.Net.Models;
using Nindo.Net.Models.Enums;

namespace Nindo.Mobile.Services
{
    public interface IApiService
    {
        public Task<Rank[]> GetViewsScoreboardAsync(RankViewsPlatform platform, Size size);

        public Task<Rank[]> GetLikesScoreboardAsync(RankLikesPlatform platform, Size size);

        public Task<Rank[]> GetViewersScoreboardAsync(Size size);

        public Task<Rank[]> GetSubGainScoreboardAsync(RankAllPlatform platform, Size size);

        public Task<Subscriber[]> GetSubscribersAsync(RankAllPlatform platform, Size size);

        public Task<Rank[]> GetScoreboardAsync(RankAllPlatform platform, Size size);

        public Task<Rank[]> GetRetweetsScoreboardAsync(Size size);

        public Task<Rank[]> GetPeakViewersScoreboardAsync(Size size);

        public Task<Rank[]> GetWatchtimeScoreboardAsync(Size size);

        public Task<Viral[]> GetViralsAsync();

        public Task<Milestone[]> GetMilestonesAsync();

        public Task<Milestone[]> GetPastMilestonesAsync();

        public Task<Search[]> SearchUserAsync(string term);
    }
}