using System;
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
    public class HomeViewModelTests
    {
        private IApiService _apiService = Mock.Of<IApiService>();
        private INavigationService _navigationService = Mock.Of<INavigationService>();

        [OneTimeSetUp]
        public void Init()
        {
            _apiService = Mock.Of<IApiService>();
            _navigationService = Mock.Of<INavigationService>();

            Mock.Get(_apiService).Setup(api => api.GetViewsScoreboardAsync(RankViewsPlatform.Youtube, Size.Small))
                .ReturnsAsync(CreateRanks(10));
            Mock.Get(_apiService).Setup(api => api.GetSubGainScoreboardAsync(RankAllPlatform.Youtube, Size.Small))
                .ReturnsAsync(CreateRanks(10));
            Mock.Get(_apiService).Setup(api => api.GetLikesScoreboardAsync(RankLikesPlatform.Instagram, Size.Small))
                .ReturnsAsync(CreateRanks(10));
            Mock.Get(_apiService).Setup(api => api.GetScoreboardAsync(RankAllPlatform.Instagram, Size.Small))
                .ReturnsAsync(CreateRanks(10));
            Mock.Get(_apiService).Setup(api => api.GetLikesScoreboardAsync(RankLikesPlatform.TikTok, Size.Small))
                .ReturnsAsync(CreateRanks(10));
            Mock.Get(_apiService).Setup(api => api.GetLikesScoreboardAsync(RankLikesPlatform.Twitter, Size.Small))
                .ReturnsAsync(CreateRanks(10));
            Mock.Get(_apiService).Setup(api => api.GetViewersScoreboardAsync(Size.Small))
                .ReturnsAsync(CreateRanks(10));
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
        public async Task Refresh_NoDataProvided_RefreshData()
        {
            // Arrange
            var sut = new HomeViewModel(_apiService, _navigationService);

            // Act
            await sut.RefreshCommand.ExecuteAsync();

            // Assert
            sut.CurrentPlatform.Should().Be("youtube");
            sut.Items.Should().HaveCount(10);
        }

        [TestCase("youtube", "youtube")]
        [TestCase("instagram", "instagram")]
        [TestCase("tiktok", "tiktok")]
        [TestCase("twitter", "twitter")]
        [TestCase("twitch", "twitch")]
        [TestCase("test", "test")]
        public void ChangePlatform_SamePlatform_Return(string platform, string previousPlatform)
        {
            // Arrange
            var sut = new HomeViewModel(_apiService, _navigationService);
            sut.CurrentPlatform = previousPlatform;

            // Act
            sut.ChangePlatformCommand.Execute(platform);

            // Arrange
            sut.CurrentPlatform.Should().BeEquivalentTo(platform);
        }

        [TestCase("youtube", "instagram")]
        [TestCase("instagram", "tiktok")]
        [TestCase("tiktok", "twitter")]
        [TestCase("twitter", "twitch")]
        [TestCase("twitch", "instagram")]
        public void ChangePlatform_NewPlatform_ChangePlatform(string platform, string previousPlatform)
        {
            // Arrange
            var sut = new HomeViewModel(_apiService, _navigationService);
            sut.CurrentPlatform = previousPlatform;

            // Act
            sut.ChangePlatformCommand.Execute(platform);

            // Arrange
            sut.CurrentPlatform.Should().BeEquivalentTo(platform);
        }

        [TestCase("123")]
        [TestCase("yutube")]
        public void ChangePlatform_InvalidPlatform_ThrowException(string platform)
        {
            // Arrange
            var sut = new HomeViewModel(_apiService, _navigationService);

            // Act
            Action act = () => sut.ChangePlatformCommand.Execute(platform);

            // Arrange
            act.Should().Throw<InvalidOperationException>().WithMessage("Invalid platform!");
        }

        [Test]
        public void Refresh_IsBusyFalse_CanExecute()
        {
            // Arrange
            var sut = new HomeViewModel(_apiService, _navigationService);

            sut.IsBusy = false;

            // Act
            var result = sut.RefreshCommand.CanExecute(null);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void Refresh_IsBusyTrue_CantExecute()
        {
            // Arrange
            var sut = new HomeViewModel(_apiService, _navigationService);

            sut.IsBusy = true;

            // Act
            var result = sut.RefreshCommand.CanExecute(null);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void ChangePlatform_IsBusyFalse_CanExecute()
        {
            // Arrange
            var sut = new HomeViewModel(_apiService, _navigationService);

            sut.IsBusy = false;

            // Act
            var result = sut.ChangePlatformCommand.CanExecute(null);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void ChangePlatform_IsBusyTrue_CantExecute()
        {
            // Arrange
            var sut = new HomeViewModel(_apiService, _navigationService);

            sut.IsBusy = true;

            // Act
            var result = sut.ChangePlatformCommand.CanExecute(null);

            // Assert
            result.Should().BeFalse();
        }
    }
}