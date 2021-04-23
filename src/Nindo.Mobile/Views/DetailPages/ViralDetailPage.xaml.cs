using System.Collections.Generic;
using Nindo.Mobile.ViewModels;
using Nindo.Mobile.ViewModels.DetailPages;
using Nindo.Net.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nindo.Mobile.Views.DetailPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViralDetailPage : ContentPage
    {
        public ViralDetailPage(IList<Viral> viral)
        {
            InitializeComponent();
            BindingContext = new ViralDetailPageViewModel(viral);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is ViralDetailPageViewModel vm)
                vm.SetupViewModel();
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            await Navigation.PopAsync();
        }
    }
}