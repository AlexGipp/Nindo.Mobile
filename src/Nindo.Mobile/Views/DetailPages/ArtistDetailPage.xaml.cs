using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nindo.Mobile.Services.Implementations;
using Nindo.Mobile.ViewModels.DetailPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nindo.Mobile.Views.DetailPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArtistDetailPage : ContentPage
    {
        public ArtistDetailPage(string artistId)
        {
            BindingContext = new ArtistDetailPageViewModel(new ApiService(), artistId);
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is ArtistDetailPageViewModel vm)
                vm.LoadArtistAsync()
                    .ContinueWith(t =>
                    {
                        if (t.IsFaulted) Debug.WriteLine(t.Exception?.Message);
                    });
        }
    }
}