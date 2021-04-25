using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Nindo.Common.Common;
using Nindo.Mobile.Models;
using Nindo.Mobile.Services;
using Nindo.Net.Models;
using Syncfusion.XForms.ComboBox;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Nindo.Mobile.ViewModels
{
    public class CouponViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;

        #region command
        public IAsyncCommand RefreshCommand { get; }
        public IAsyncCommand LoadMoreCouponsCommand { get; set; }
        public IAsyncCommand<CouponBrands> ComboboxSelectionChangedCommand { get; set; }
        public IAsyncCommand CollectionViewSelectionChangedCommand { get; set; }
        #endregion

        public CouponViewModel(IApiService apiService)
        {
            Coupons = new[]
            {
                new ExtendedCoupon
                {
                    CouponTitle = "keine Filter"
                },
                new ExtendedCoupon
                {
                    CouponTitle = "Brand Filter"
                },
                new ExtendedCoupon
                {
                    CouponTitle = "Category Filter"
                }
            };

            InitLists();
            _apiService = apiService;
            RefreshCommand = new AsyncCommand(RefreshAsync, CanExecute);
            LoadMoreCouponsCommand = new AsyncCommand(LoadCouponsAsync, CanExecute);
            ComboboxSelectionChangedCommand = new AsyncCommand<CouponBrands>(ComboboxSelectionChangedAsync, CanExecute);
            CollectionViewSelectionChangedCommand = new AsyncCommand(CopyCouponCode, CanExecute);
        }

        
        public void InitLists()
        {
            Coupons[0].Coupons = new RangeObservableCollection<Coupon>() { };
            Coupons[1].Coupons = new RangeObservableCollection<Coupon>() { };
            Coupons[2].Coupons = new RangeObservableCollection<Coupon>() { };

            Coupons[0].ComboboxIsVisible = false;
            Coupons[1].ComboboxIsVisible = true;
            Coupons[2].ComboboxIsVisible = true;

            Coupons[1].ComboboxItems = new RangeObservableCollection<CouponBrands>() { };
            Coupons[2].ComboboxItems = new RangeObservableCollection<CouponBrands>() { };

            Coupons[0].pageNumber = 0;
            Coupons[1].pageNumber = 0;
            Coupons[2].pageNumber = 0;
        }

        public async Task LoadComboboxItemsAsync()
        {
            if (!Coupons[1].ComboboxItems.Any() && !Coupons[2].ComboboxItems.Any())
            {
                var brandItems = await _apiService.GetCouponBrandsAsync();
                var categories = await _apiService.GetCouponBranchesAsync();

                RangeObservableCollection<CouponBrands> categoryItems = new RangeObservableCollection<CouponBrands>();
                foreach (var item in categories)
                {
                    categoryItems.Add(new CouponBrands { Name = item });
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    Coupons[1].ComboboxItems.AddRange(brandItems);
                    Coupons[2].ComboboxItems.AddRange(categoryItems);

                });
                Device.BeginInvokeOnMainThread(() => {

                    var noFilter = _apiService.GetCouponsAsync(0);

                    Coupons[1].Coupons.AddRange(noFilter.Result.Coupon);
                    Coupons[2].Coupons.AddRange(noFilter.Result.Coupon);    
                });
            }
        }

        CouponBrands selectedItem;
        public async Task ComboboxSelectionChangedAsync(CouponBrands obj)
        {
            try
            {
                IsBusy = true;

                switch (SelectedTabIndex)
                {
                    case 1:
                        if (Coupons[1].ComboboxItems.Contains(obj))
                        {
                            Coupons[1].Coupons.Clear();
                            Coupons[1].pageNumber = 0;
                            hasMore = true;
                            selectedItem = obj;
                            await LoadCouponsAsync();
                        }
                        else
                        {
                            return;
                        }
                        break;

                    case 2:
                        if (Coupons[2].ComboboxItems.Contains(obj))
                        {
                            Coupons[2].Coupons.Clear();
                            Coupons[2].pageNumber = 0;
                            hasMore = true;
                            selectedItem = obj;
                            await LoadCouponsAsync();
                        }
                        else
                        {
                            return;
                        }
                        break;
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool hasMore = true;
        public async Task LoadCouponsAsync()
        {
            try
            {
                IsBusy = true;

                if (hasMore == true)
                {
                    await Task.Run(async () =>
                    {

                        switch (SelectedTabIndex)
                        {
                            case 0:
                                var noFilter = await _apiService.GetCouponsAsync(Coupons[0].pageNumber);
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    Coupons[0].Coupons.AddRange(noFilter.Coupon);
                                });
                                if (noFilter.HasMore == "true")
                                {
                                    Coupons[0].pageNumber += 20;
                                }
                                else
                                {
                                    hasMore = false;
                                }
                                break;

                            case 1:
                                    var brandFilter = await _apiService.GetCouponsByBranchAsync(selectedItem.Id, Coupons[1].pageNumber);
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        Coupons[1].Coupons.AddRange(brandFilter.Coupon);
                                    });
                                    if (brandFilter.HasMore == "true")
                                    {
                                        Coupons[1].pageNumber += 20;
                                    }
                                    else
                                    {
                                        hasMore = false;
                                    }
                                break;

                            case 2:
                                    try
                                    {
                                        var categoryFilter = await _apiService.GetCouponsByCategoryAsync(selectedItem.Name, Coupons[2].pageNumber);
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            Coupons[2].Coupons.AddRange(categoryFilter.Coupon);
                                        });
                                        if (categoryFilter.HasMore == "true")
                                        {
                                            Coupons[2].pageNumber += 20;
                                        }
                                        else
                                        {
                                            hasMore = false;
                                        }

                                    }
                                    catch (Exception e)
                                    {
                                        Debug.WriteLine(e);
                                    }
                                break;
                        }
                    });
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task CopyCouponCode()
        {
            try
            {
                IsBusy = true;

                await Clipboard.SetTextAsync(CollectionViewSelectedItem.Code);
                var openSite = await Application.Current.MainPage.DisplayAlert("", "Code has been Copied \n To Open Website Click UwU", "UwU", "Cancel");
                if (openSite == true)
                {
                    await Browser.OpenAsync(CollectionViewSelectedItem.Brand.Url, BrowserLaunchMode.SystemPreferred);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RefreshAsync()
        {
            try
            {
                switch (SelectedTabIndex)
                {
                    case 0:
                        Coupons[0].pageNumber = 0;
                        break;
                    case 1:
                        Coupons[1].pageNumber = 0;
                        break;
                    case 2:
                        Coupons[2].pageNumber = 0;
                        break;
                }
                IsRefreshing = true;

                await LoadCouponsAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private bool CanExecute(object arg)
        {
            return !IsBusy && !IsRefreshing;
        }

        private IList<ExtendedCoupon> _coupons;

        public IList<ExtendedCoupon> Coupons
        {
            get => _coupons;
            set
            {
                _coupons = value;
                OnPropertyChanged();
            }
        }

        private Coupon _collectionViewSelectedItem;
        public Coupon CollectionViewSelectedItem
        {
            get => _collectionViewSelectedItem;
            set
            {
                _collectionViewSelectedItem = value;

                OnPropertyChanged();
            }
        }

        private int _selectedTabIndex;

        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged();
            }
        }

        private bool _isRefreshing;

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }
    }
}
