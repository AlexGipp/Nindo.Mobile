using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Nindo.Mobile.Models;
using Nindo.Mobile.Services;
using Nindo.Mobile.ViewModels;
using Nindo.Net.Models;
using NUnit.Framework;

namespace Nindo.Mobile.Tests.ViewModels
{
    [TestFixture]
    public class ViralViewModelTests
    {
        [TestCase(Platforms.Youtube)]
        [TestCase(Platforms.Instagram)]
        [TestCase(Platforms.Tiktok)]
        [TestCase(Platforms.Twitter)]
        [TestCase(Platforms.Twitch)]
        public async Task OpenDetailPage_EnumProvided_NavigateToDetailPage(Platforms type)
        {
            // Arrange
            var apiService = Mock.Of<IApiService>();
            var navigationService = Mock.Of<INavigationService>();

            Mock.Get(apiService).Setup(api => api.GetViralsAsync())
                .ReturnsAsync(new[]
                {
                    new Viral{Platform = "twitter", Type = "likes" },
                    new Viral{Platform = "twitter", Type = "retweets" },
                    new Viral{Platform = "tiktok", Type = "likes" },
                    new Viral{Platform = "tiktok", Type = "kommentare" },
                    new Viral{Platform = "tiktok", Type = "views" },
                    new Viral{Platform = "youtube", Type = "likes" },
                    new Viral{Platform = "youtube", Type = "kommentare" },
                    new Viral{Platform = "youtube", Type = "views" },
                    new Viral{Platform = "instagram", Type = "likes" },
                    new Viral{Platform = "instagram", Type = "kommentare" }
                });

            var sut = new ViralViewModel(apiService, navigationService);

            // Act
            await sut.GetViralAsync();
            await sut.OpenDetailPageCommand.ExecuteAsync(type);

            // Assert
            Mock.Get(navigationService).Verify(m => m.OpenViralDetailPageAsync(It.IsAny<IList<Viral>>()), Times.Once);
        }

        [Test]
        public async Task OpenDetailPage_IsBusyFalseHasData_CanExecute()
        {
            // Arrange
            var apiService = Mock.Of<IApiService>();
            var navigationService = Mock.Of<INavigationService>();

            Mock.Get(apiService).Setup(api => api.GetViralsAsync())
                .ReturnsAsync(new[]
                {
                    new Viral{Platform = "twitter", Type = "likes" },
                    new Viral{Platform = "twitter", Type = "retweets" },
                    new Viral{Platform = "tiktok", Type = "likes" }
                });

            var sut = new ViralViewModel(apiService, navigationService) {IsBusy = false};

            // Act
            await sut.GetViralAsync();
            var result = sut.OpenDetailPageCommand.CanExecute(null);

            //Arrange
            result.Should().BeTrue();
        }

        [Test]
        public async Task OpenDetailPage_IsBusyTrueHasData_CantExecute()
        {
            // Arrange
            var apiService = Mock.Of<IApiService>();
            var navigationService = Mock.Of<INavigationService>();

            Mock.Get(apiService).Setup(api => api.GetViralsAsync())
                .ReturnsAsync(new[]
                {
                    new Viral{Platform = "twitter", Type = "likes" },
                    new Viral{Platform = "twitter", Type = "retweets" },
                    new Viral{Platform = "tiktok", Type = "likes" }
                });

            var sut = new ViralViewModel(apiService, navigationService);
            await sut.GetViralAsync();
            sut.IsBusy = true;

            // Act
            var result = sut.OpenDetailPageCommand.CanExecute(null);

            //Arrange
            result.Should().BeFalse();
        }

        [Test]
        public void OpenDetailPage_IsBusyFalseHasNoData_CantExecute()
        {
            // Arrange
            var apiService = Mock.Of<IApiService>();
            var navigationService = Mock.Of<INavigationService>();


            var sut = new ViralViewModel(apiService, navigationService);
            sut.IsBusy = false;

            // Act
            var result = sut.OpenDetailPageCommand.CanExecute(null);

            //Arrange
            result.Should().BeFalse();
        }

        [Test]
        public void OpenDetailPage_IsBusyTrueHasNoData_CantExecute()
        {
            // Arrange
            var apiService = Mock.Of<IApiService>();
            var navigationService = Mock.Of<INavigationService>();


            var sut = new ViralViewModel(apiService, navigationService);
            sut.IsBusy = true;

            // Act
            var result = sut.OpenDetailPageCommand.CanExecute(null);

            //Arrange
            result.Should().BeFalse();
        }
    }
}