using Nindo.Mobile.Services.Implementations;
using Nindo.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nindo.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CouponPage : ContentPage
    {
        public CouponPage()
        {
            BindingContext = new CouponViewModel(new ApiService());
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is CouponViewModel vm)
            {
                if (vm.Coupons.Any(m => !m.ComboboxItems.Any()))
                {
                    vm.LoadComboboxItemsAsync()
                        .ContinueWith(t =>
                        {
                            if (t.IsFaulted) Debug.WriteLine(t.Exception?.Message);
                        });
                }
                if (!vm.Coupons[1].Coupons.Any() && !vm.Coupons[2].Coupons.Any())
                {
                    vm.LoadDefaultItems()
                        .ContinueWith(t =>
                        {
                            if (t.IsFaulted) Debug.WriteLine(t.Exception?.Message);
                        });
                }
            }
        }
    }
}