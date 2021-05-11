using Nindo.Mobile.ViewModels;
using Nindo.Mobile.ViewModels.BaseViewModels;
using Nindo.Net.Models;

namespace Nindo.Mobile.Models
{
    public class ExtendedViral : ViewModelBase
    {
        public string ViralTitle { get; set; }

        public string ViralEmbed { get; set; }

        public string FormattedValue { get; set; }

        private Viral _viralEntry;
        public Viral ViralEntry
        {
            get => _viralEntry;
            set
            {
                _viralEntry = value;
                OnPropertyChanged();
            }
        }
    }
}