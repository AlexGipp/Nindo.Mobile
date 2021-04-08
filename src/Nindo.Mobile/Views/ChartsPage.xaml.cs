using System.Diagnostics;
using System.Linq;
using Nindo.Mobile.Services.Implementations;
using Nindo.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nindo.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChartsPage : ContentPage
    {
        public ChartsPage()
        {
            BindingContext = new ChartsViewModel(new ApiService());
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            
        }
    }
}