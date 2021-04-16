using System;
using System.Threading.Tasks;
using FluentAssertions;
using Nindo.Mobile.ViewModels;
using NUnit.Framework;

namespace Nindo.Mobile.Tests.ViewModels
{
    [TestFixture]
    public class HomeViewModelTests
    {
        [Test]
        public async Task Refresh_NoDataProvided_RefreshData()
        {
            // Arrange
            var sut = new HomeViewModel();

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
            var sut = new HomeViewModel();
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
            var sut = new HomeViewModel();
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
            var sut = new HomeViewModel();

            // Act
            Action act = () => sut.ChangePlatformCommand.Execute(platform);

            // Arrange
            act.Should().Throw<InvalidOperationException>().WithMessage("Invalid platform!");
        }

        [Test]
        public void Refresh_IsBusyFalse_CanExecute()
        {
            // Arrange
            var sut = new HomeViewModel();
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
            var sut = new HomeViewModel();
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
            var sut = new HomeViewModel();
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
            var sut = new HomeViewModel();
            sut.IsBusy = true;

            // Act
            var result = sut.ChangePlatformCommand.CanExecute(null);

            // Assert
            result.Should().BeFalse();
        }
    }
}