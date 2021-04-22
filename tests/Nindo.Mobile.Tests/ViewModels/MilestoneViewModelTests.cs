using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Nindo.Mobile.Services;
using Nindo.Mobile.ViewModels;
using Nindo.Net.Models;
using NUnit.Framework;

namespace Nindo.Mobile.Tests.ViewModels
{
    [TestFixture]
    public class MilestoneViewModelTests
    {
        private Milestone[] CreateMilestones(int count)
        {
            var milestones = new List<Milestone>();
            for (int i = 0; i < count; i++)
            {
                milestones.Add(new Milestone());
            }

            return milestones.ToArray();
        }

        [Test]
        public async Task LoadMilestonesAsync_NothingProvided_LoadData()
        {
            // Arrange 
            var apiService = Mock.Of<IApiService>();
            var navigationService = Mock.Of<INavigationService>();

            var sut = new MilestonesViewModel(apiService, navigationService);

            Mock.Get(apiService).Setup(api => api.GetMilestonesAsync())
                .ReturnsAsync(CreateMilestones(3));
            Mock.Get(apiService).Setup(api => api.GetPastMilestonesAsync())
                .ReturnsAsync(CreateMilestones(6));

            // Act
            await sut.LoadMilestonesAsync();

            // Assert
            sut.Milestones[0].Milestones.Should().HaveCount(3);
            sut.Milestones[1].Milestones.Should().HaveCount(6);
        }

        [Test]
        public async Task Refresh_NoDataProvided_RefreshData()
        {
            // Arrange 
            var apiService = Mock.Of<IApiService>();
            var navigationService = Mock.Of<INavigationService>();

            var sut = new MilestonesViewModel(apiService, navigationService);

            // Act
            await sut.RefreshCommand.ExecuteAsync();

            // Assert
            Mock.Get(apiService).Verify(m => m.GetMilestonesAsync(), Times.Once);
            Mock.Get(apiService).Verify(m => m.GetPastMilestonesAsync(), Times.Once);
        }

        [Test]
        public void Refresh_NotBusyNotRefreshing_CanExecute()
        {
            // Arrange 
            var apiService = Mock.Of<IApiService>();
            var navigationService = Mock.Of<INavigationService>();

            var sut = new MilestonesViewModel(apiService, navigationService);

            sut.IsBusy = false;
            sut.IsRefreshing = false;

            // Act
            var result = sut.RefreshCommand.CanExecute(null);

            // Assert
            result.Should().BeTrue();
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public void Refresh_IsRefreshingOrBusy_CantExecute(bool isBusy, bool isRefreshing)
        {
            // Arrange 
            var apiService = Mock.Of<IApiService>();
            var navigationService = Mock.Of<INavigationService>();

            var sut = new MilestonesViewModel(apiService, navigationService);

            sut.IsBusy = isBusy;
            sut.IsRefreshing = isRefreshing;

            // Act
            var result = sut.RefreshCommand.CanExecute(null);

            // Assert
            result.Should().BeFalse();
        }
    }
}