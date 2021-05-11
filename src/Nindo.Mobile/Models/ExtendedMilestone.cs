using System.Collections.Generic;
using Nindo.Mobile.ViewModels;
using Nindo.Mobile.ViewModels.BaseViewModels;
using Nindo.Net.Models;

namespace Nindo.Mobile.Models
{
    public class ExtendedMilestone : ViewModelBase
    {
        public string MilestoneTitle { get; set; }

        private List<Milestone> _milestones = new List<Milestone>();

        public List<Milestone> Milestones
        {
            get => _milestones;
            set
            {
                _milestones = value;
                OnPropertyChanged();
            }
        }

    }
}