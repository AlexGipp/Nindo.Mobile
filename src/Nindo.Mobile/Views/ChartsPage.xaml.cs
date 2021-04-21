using System.Diagnostics;
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
            BindingContext = new ChartsViewModel(new ApiService(), new NavigationService());
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is ChartsViewModel vm && vm.ResultItems.Count == 0)
                vm.LoadDefaultData()
                    .ContinueWith(t =>
                    {
                        if (t.IsFaulted) Debug.WriteLine(t.Exception?.Message);
                    });
        }
    }
}