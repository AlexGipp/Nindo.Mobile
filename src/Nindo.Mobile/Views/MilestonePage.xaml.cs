using System.Diagnostics;
using System.Linq;
using Nindo.Mobile.Services.Implementations;
using Nindo.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nindo.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MilestonePage : ContentPage
    {
        public MilestonePage()
        {
            BindingContext = new MilestonesViewModel(new ApiService(), new NavigationService());
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is MilestonesViewModel vm && vm.Milestones.Any(m => !m.Milestones.Any()))
                vm.LoadMilestonesAsync()
                    .ContinueWith(t =>
                    {
                        if (t.IsFaulted) Debug.WriteLine(t.Exception?.Message);
                    });
        }
    }
}