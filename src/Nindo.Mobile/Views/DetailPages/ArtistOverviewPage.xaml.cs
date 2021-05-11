using System.Diagnostics;
using Nindo.Mobile.Services.Implementations;
using Nindo.Mobile.ViewModels.DetailPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nindo.Mobile.Views.DetailPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArtistOverviewPage : ContentPage
    {
        public ArtistOverviewPage(string artistId)
        {
            BindingContext = new ArtistOverviewPageViewModel(new ApiService(), new NavigationService(), artistId);
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is ArtistOverviewPageViewModel vm)
                vm.LoadArtistAsync()
                    .ContinueWith(t =>
                    {
                        if (t.IsFaulted) Debug.WriteLine(t.Exception?.Message);
                    });
        }
    }
}