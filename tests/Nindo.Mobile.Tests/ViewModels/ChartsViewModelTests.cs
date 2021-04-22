using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Nindo.Mobile.Services;
using Nindo.Mobile.ViewModels;
using Nindo.Net.Models;
using Nindo.Net.Models.Enums;
using NUnit.Framework;

namespace Nindo.Mobile.Tests.ViewModels
{
    [TestFixture]
    public class ChartsViewModelTests
    {
        private IApiService _apiService = Mock.Of<IApiService>();
        private INavigationService _navigationService = Mock.Of<INavigationService>();


        [OneTimeSetUp]
        public void Init()
        {
            _apiService = Mock.Of<IApiService>();
            _navigationService = Mock.Of<INavigationService>();

            Mock.Get(_apiService).Setup(api => api.GetViewsScoreboardAsync(RankViewsPlatform.Youtube, Size.Big))
                .ReturnsAsync(CreateRanks(100));
            Mock.Get(_apiService).Setup(api => api.GetSubGainScoreboardAsync(RankAllPlatform.Youtube, Size.Big))
                .ReturnsAsync(CreateRanks(100));
            Mock.Get(_apiService).Setup(api => api.GetLikesScoreboardAsync(RankLikesPlatform.Instagram, Size.Big))
                .ReturnsAsync(CreateRanks(100));
            Mock.Get(_apiService).Setup(api => api.GetScoreboardAsync(RankAllPlatform.Instagram, Size.Big))
                .ReturnsAsync(CreateRanks(100));
            Mock.Get(_apiService).Setup(api => api.GetLikesScoreboardAsync(RankLikesPlatform.TikTok, Size.Big))
                .ReturnsAsync(CreateRanks(100));
            Mock.Get(_apiService).Setup(api => api.GetLikesScoreboardAsync(RankLikesPlatform.Twitter, Size.Big))
                .ReturnsAsync(CreateRanks(100));
            Mock.Get(_apiService).Setup(api => api.GetViewersScoreboardAsync(Size.Big))
                .ReturnsAsync(CreateRanks(100));
        }

        private Rank[] CreateRanks(int count)
        {
            var ranks = new List<Rank>();
            for (int i = 0; i < count; i++)
            {
                ranks.Add(new Rank());
            }

            return ranks.ToArray();
        }

        [Test]
        public async Task LoadDefaultData_NoDataProvided_LoadDefaultData()
        {
            // Arrange
            var sut = new ChartsViewModel(_apiService, _navigationService);

            // Act
            await sut.LoadDefaultData();

            // Assert
            sut.CurrentPlatform.Should().Be("youtube");
            sut.CollectionViewHeaderText.Should().Be("Ø nach 5 Tagen");
            sut.ResultItems.Should().HaveCount(100);
        }

        [TestCase("youtube", "instagram", 5)]
        [TestCase("instagram", "tiktok", 4)]
        [TestCase("tiktok", "twitter", 4)]
        [TestCase("twitter", "twitch",5 )]
        [TestCase("twitch", "instagram", 6)]
        public void ChangePlatformAsync_NewPlatform_ChangePlatform(string platform, string previousPlatform, int filterCount)
        {
            // Arrange
            var sut = new ChartsViewModel(_apiService, _navigationService);

            sut.CurrentPlatform = previousPlatform;

            // Act
            sut.ChangePlatformCommand.ExecuteAsync(platform);

            // Assert
            sut.CurrentPlatform.Should().BeEquivalentTo(platform);
            sut.FilterItems.Count.Should().Be(filterCount, "platform changed");
            sut.ResultItems.Should().HaveCount(100, "Size.Big always returns 100 entries.");
        }

        [Test]
        public void ChangeFilterAsync_NewFilter_ChangeFilter()
        {
            // Arrange
            var sut = new ChartsViewModel(_apiService, _navigationService);

            // Act
            sut.ChangeFilterCommand.ExecuteAsync(sut.YoutubeFilters[2]);

            // Assert
            sut.CollectionViewHeaderText.Should().BeEquivalentTo(sut.YoutubeFilters[2].HeaderText);
            sut.ResultItems.Should().HaveCount(100);
            sut.SelectedPickerItem.Should().BeEquivalentTo(sut.YoutubeFilters[2]);
        }

        [Test]
        public void Refresh_NoDataProvided_RefreshData()
        {
            // Arrange
            var sut = new ChartsViewModel(_apiService, _navigationService);
            sut.SelectedPickerItem = sut.InstagramFilters[3];

            // Act
            sut.RefreshCommand.ExecuteAsync();

            // Assert
            sut.CollectionViewHeaderText.Should().BeEquivalentTo(sut.InstagramFilters[3].HeaderText);
            sut.ResultItems.Should().HaveCount(100);
            sut.SelectedPickerItem.Should().BeEquivalentTo(sut.InstagramFilters[3]);
        }

        [TestCase(false, true)]
        [TestCase(true, false)]
        public void Refresh_BusyState_HaveState(bool isBusy, bool canExecute)
        {
            // Arrange
            var sut = new ChartsViewModel(_apiService, _navigationService) {IsBusy = isBusy};


            // Act
            var result = sut.RefreshCommand.CanExecute(null);

            // Assert
            result.Should().Be(canExecute);
        }

        [TestCase(false, true)]
        [TestCase(true, false)]
        public void ChangePlatformAsync_BusyState_HaveState(bool isBusy, bool canExecute)
        {
            // Arrange
            var sut = new ChartsViewModel(_apiService, _navigationService);

            sut.IsBusy = isBusy;

            // Act
            var result = sut.ChangePlatformCommand.CanExecute(null);

            // Assert
            result.Should().Be(canExecute);
        }

        [TestCase(false, true)]
        [TestCase(true, false)]
        public void ChangeFilterAsync_BusyState_HaveState(bool isBusy, bool canExecute)
        {
            // Arrange
            var sut = new ChartsViewModel(_apiService, _navigationService);

            sut.IsBusy = isBusy;

            // Act
            var result = sut.ChangeFilterCommand.CanExecute(null);

            // Assert
            result.Should().Be(canExecute);
        }
    }
}