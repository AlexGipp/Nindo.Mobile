using Nindo.Mobile.ViewModels;
using Nindo.Net.Models;
using Xamarin.Forms;

namespace Nindo.Mobile.Models
{
    public class ExtendedViral : ViewModelBase
    {
        public string ViralTitle { get; set; }
        private Viral _viralEntry { get; set; }

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