using System.Threading.Tasks;
using Nindo.Net;
using Nindo.Net.Models;
using Nindo.Net.Models.Enums;

namespace Nindo.Mobile.Services.Implementations
{
    public class ApiService : IApiService
    {
        private readonly NindoClient _nindoClient;

        public ApiService()
        {
            _nindoClient = new NindoClient();
        }

        public Task<Rank[]> GetViewsScoreboardAsync(RankViewsPlatform platform, Size size)
        {
            return _nindoClient.GetViewsScoreboardAsync(platform, size);
        }

        public Task<Rank[]> GetLikesScoreboardAsync(RankLikesPlatform platform, Size size)
        {
            return _nindoClient.GetLikesScoreboardAsync(platform, size);
        }
        public Task<Rank[]> GetSubGainScoreboardAsync(RankAllPlatform platform, Size size)
        {
            return _nindoClient.GetSubGainScoreboardAsync(platform, size);
        }

        public Task<Subscriber[]> GetSubscribersAsync(RankAllPlatform platform, Size size)
        {
            return _nindoClient.GetSubscribersAsync(platform, size);
        }

        public Task<Rank[]> GetScoreboardAsync(RankAllPlatform platform, Size size)
        {
            return _nindoClient.GetScoreboardAsync(platform, size);
        }

        public Task<Rank[]> GetRetweetsScoreboardAsync(Size size)
        {
            return _nindoClient.GetRetweetsScoreboardAsync(size);
        }

        public Task<Rank[]> GetPeakViewersScoreboardAsync(Size size)
        {
            return _nindoClient.GetPeakViewersScoreboardAsync(size);
        }

        public Task<Rank[]> GetWatchtimeScoreboardAsync(Size size)
        {
            return _nindoClient.GetWatchtimeScoreboardAsync(size);
        }

        public Task<Rank[]> GetViewersScoreboardAsync(Size size)
        {
            return _nindoClient.GetViewersScoreboardAsync(size);
        }

        public Task<Viral[]> GetViralsAsync()
        {
            return _nindoClient.GetViralsAsync();
        }

        public Task<Milestone[]> GetMilestonesAsync()
        {
            return _nindoClient.GetMilestonesAsync();
        }

        public Task<Milestone[]> GetPastMilestonesAsync()
        {
            return _nindoClient.GetPastMilestonesAsync();
        }

        public Task<Coupons> GetCouponsAsync(int page)
        {
            return _nindoClient.GetCouponsAsync(page);
        }

        public Task<CouponBrands[]> GetCouponBrandsAsync()
        {
            return _nindoClient.GetCouponBrandsAsync();
        }

        public Task<string[]> GetCouponBranchesAsync()
        {
            return _nindoClient.GetCouponCategoriesAsync();
        }

        public Task<Coupons> GetCouponsByCategoryAsync(string category, int offset)
        {
            return _nindoClient.GetCouponsByCategoryAsync(category, offset);
        }

        public Task<Coupons> GetCouponsByBranchAsync(string id, int offset)
        {
            return _nindoClient.GetCouponsByBrandAsync(id, offset);
        }
        
        public Task<Search[]> SearchUserAsync(string term)
        {
            return _nindoClient.SearchUserAsync(term);
        }
    }
}