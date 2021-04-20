using Nindo.Mobile.ViewModels;
using Nindo.Net.Models;
using Xamarin.Forms;

namespace Nindo.Mobile.Models
{
    public class ExtendedViral : ViewModelBase
    {
        public string ViralTitle { get; set; }

        public string ViralEmbed { get; set; }

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