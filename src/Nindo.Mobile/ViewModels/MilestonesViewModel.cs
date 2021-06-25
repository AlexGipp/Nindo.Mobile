using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nindo.Mobile.Models;
using Nindo.Mobile.Services;
using Nindo.Net.Models;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms.Internals;

namespace Nindo.Mobile.ViewModels
{
    public class MilestonesViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;

        #region command
        public IAsyncCommand RefreshCommand { get; }

        #endregion

        public MilestonesViewModel(IApiService apiService)
        {
            Milestones = new[]
            {
                new ExtendedMilestone
                {
                    MilestoneTitle = "NÄCHSTE MEILENSTEINE"
                },
                new ExtendedMilestone
                {
                    MilestoneTitle = "LETZTE MEILENSTEINE"
                }
            };

            _apiService = apiService;
            RefreshCommand = new AsyncCommand(RefreshAsync, CanExecute);

        }

        public async Task LoadMilestonesAsync()
        {
            try
            {
                IsBusy = true;

                await Task.Run(async () =>
                {
                    Milestones[0].Milestones = new List<Milestone>();
                    Milestones[1].Milestones = new List<Milestone>();

                    var newMsTask = _apiService.GetMilestonesAsync();
                    var pastMsTask = _apiService.GetPastMilestonesAsync();

                    var taskList = new List<Task<Milestone[]>>
                    {
                        newMsTask,
                        pastMsTask,
                    };

                    Task<Milestone[]> currentTask;

                    while (taskList.Count > 0 && (currentTask = await Task.WhenAny(taskList)) != null)
                    {
                        taskList.Remove(currentTask);

                        if (currentTask == newMsTask)
                        {
                            Milestones[0].Milestones = newMsTask.Result.ToList();
                        }
                        else if (currentTask == pastMsTask)
                        {
                            Milestones[1].Milestones = pastMsTask.Result.ToList();
                        }
                    }
                });
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RefreshAsync()
        {
            try
            {
                IsRefreshing = true;

                await LoadMilestonesAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private bool CanExecute(object arg)
        {
            return !IsBusy && !IsRefreshing;
        }

        private IList<ExtendedMilestone> _milestones;

        public IList<ExtendedMilestone> Milestones
        {
            get => _milestones;
            set
            {
                _milestones = value;
                OnPropertyChanged();
            }
        }

        private bool _isRefreshing;

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }
    }
}