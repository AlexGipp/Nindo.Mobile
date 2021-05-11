using System.Diagnostics;
using Nindo.Mobile.Services.Implementations;
using Nindo.Mobile.ViewModels.DetailPages.PlatformDetailPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nindo.Mobile.Views.DetailPages.PlatformDetailPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class YoutubeArtistDetailPage : ContentPage
    {
        private readonly string _id;

        public YoutubeArtistDetailPage(string id)
        {
            BindingContext = new YoutubeArtistDetailPageViewModel(new ApiService());
            InitializeComponent();

            _id = id;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is YoutubeArtistDetailPageViewModel vm)
                vm.GetYoutubeDataAsync(_id)
                    .ContinueWith(t =>
                    {
                        if (t.IsFaulted) Debug.WriteLine(t.Exception?.Message);
                    });
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            await Navigation.PopAsync();
        }
    }
}