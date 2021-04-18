using Nindo.Mobile.Services.Implementations;
using Nindo.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nindo.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            BindingContext = new SearchViewModel(new ApiService());
            InitializeComponent();
        }
    }
}