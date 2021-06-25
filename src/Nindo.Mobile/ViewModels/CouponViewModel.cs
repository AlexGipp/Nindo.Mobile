using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Nindo.Common.Common;
using Nindo.Mobile.Models;
using Nindo.Mobile.Services;
using Nindo.Net.Models;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Nindo.Mobile.ViewModels
{
    public class CouponViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;

        #region Commands
        public IAsyncCommand RefreshCommand { get; }
        public IAsyncCommand LoadMoreCouponsCommand { get; }
        public IAsyncCommand<CouponBrands> ComboboxSelectionChangedCommand { get; }
        public IAsyncCommand<Coupon> CollectionViewSelectionChangedCommand { get; }
        #endregion

        public CouponViewModel(IApiService apiService)
        {
            Coupons = new[]
            {
                new ExtendedCoupon
                {
                    CouponTitle = "Keine"
                },
                new ExtendedCoupon
                {
                    CouponTitle = "Marken"
                },
                new ExtendedCoupon
                {
                    CouponTitle = "Kategorien"
                }
            };

            InitLists();
            _apiService = apiService;
            RefreshCommand = new AsyncCommand(RefreshAsync, CanExecute);
            LoadMoreCouponsCommand = new AsyncCommand(LoadCouponsAsync, CanExecute);
            ComboboxSelectionChangedCommand = new AsyncCommand<CouponBrands>(ComboboxSelectionChangedAsync, CanExecute);
            CollectionViewSelectionChangedCommand = new AsyncCommand<Coupon>(CopyCouponCode, CanExecute);
        }


        private void InitLists()
        {
            Coupons[0].Coupons = new RangeObservableCollection<Coupon>();
            Coupons[1].Coupons = new RangeObservableCollection<Coupon>();
            Coupons[2].Coupons = new RangeObservableCollection<Coupon>();

            Coupons[0].ComboboxIsVisible = false;
            Coupons[1].ComboboxIsVisible = true;
            Coupons[2].ComboboxIsVisible = true;

            Coupons[1].ComboboxItems = new RangeObservableCollection<CouponBrands>();
            Coupons[2].ComboboxItems = new RangeObservableCollection<CouponBrands>();

            Coupons[0].PageNumber = 0;
            Coupons[1].PageNumber = 0;
            Coupons[2].PageNumber = 0;
        }

        public async Task LoadComboboxItemsAsync()
        {
            if (!Coupons[1].ComboboxItems.Any() && !Coupons[2].ComboboxItems.Any())
            {
                var brands = await _apiService.GetCouponBrandsAsync();
                var categories = await _apiService.GetCouponBranchesAsync();

                var brandItems = new RangeObservableCollection<CouponBrands>();
                brandItems.AddRange(brands);

                var categoryItems = new RangeObservableCollection<CouponBrands>();
                foreach (var item in categories)
                {
                    categoryItems.Add(new CouponBrands { Name = item });
                }

                Coupons[1].ComboboxItems = brandItems;
                Coupons[2].ComboboxItems = categoryItems;

            }
        }

        public async Task LoadDefaultItems()
        {
            var noFilter = await _apiService.GetCouponsAsync(0);
            Coupons[1].Coupons.AddRange(noFilter.Coupon);
            Coupons[2].Coupons.AddRange(noFilter.Coupon);
        }

        private async Task ComboboxSelectionChangedAsync(CouponBrands couponBrands)
        {
            try
            {
                IsBusy = true;

                switch (SelectedTabIndex)
                {
                    case 1:
                        if (Coupons[1].ComboboxItems.Contains(couponBrands))
                        {
                            Coupons[1].Coupons.Clear();
                            Coupons[1].PageNumber = 0;
                            _hasMore = true;
                            Coupons[1].SelectedItem = couponBrands;
                            await LoadCouponsAsync();
                        }
                        break;

                    case 2:
                        if (Coupons[2].ComboboxItems.Contains(couponBrands))
                        {
                            Coupons[2].Coupons.Clear();
                            Coupons[2].PageNumber = 0;
                            _hasMore = true;
                            Coupons[2].SelectedItem = couponBrands;
                            await LoadCouponsAsync();
                        }
                        break;
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool _hasMore = true;
        private async Task LoadCouponsAsync()
        {
            try
            {
                IsBusy = true;

                if (_hasMore)
                {
                    await Task.Run(() =>
                    {

                        switch (SelectedTabIndex)
                        {
                            case 0:
                                var noFilter = _apiService.GetCouponsAsync(Coupons[0].PageNumber);
                                Coupons[0].Coupons.AddRange(noFilter.Result.Coupon);
                                if (noFilter.Result.HasMore)
                                {
                                    Coupons[0].PageNumber += 20;
                                }
                                else
                                {
                                    _hasMore = false;
                                }

                                break;

                            case 1:
                                if (Coupons[1].SelectedItem != null)
                                {
                                    var brandFilter = _apiService.GetCouponsByBranchAsync(Coupons[1].SelectedItem.Id,
                                        Coupons[1].PageNumber);
                                    Coupons[1].Coupons.AddRange(brandFilter.Result.Coupon);
                                    if (brandFilter.Result.HasMore)
                                    {
                                        Coupons[1].PageNumber += 20;
                                    }
                                    else
                                    {
                                        _hasMore = false;
                                    }
                                }

                                break;

                            case 2:
                                if (Coupons[2].SelectedItem != null)
                                {
                                    try
                                    {
                                        var categoryFilter =
                                            _apiService.GetCouponsByCategoryAsync(Coupons[2].SelectedItem.Name,
                                                Coupons[2].PageNumber);
                                        Coupons[2].Coupons.AddRange(categoryFilter.Result.Coupon);
                                        if (categoryFilter.Result.HasMore)
                                        {
                                            Coupons[2].PageNumber += 20;
                                        }
                                        else
                                        {
                                            _hasMore = false;
                                        }

                                    }
                                    catch (Exception e)
                                    {
                                        Debug.WriteLine(e);
                                    }
                                }

                                break;
                        }
                    });
                }
            }
            catch(Exception ex)
            {
                
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task CopyCouponCode(Coupon collectionViewSelectedItem)
        {
            try
            {
                IsBusy = true;

                await Clipboard.SetTextAsync(collectionViewSelectedItem.Code);
                var openSite = await Application.Current.MainPage.DisplayAlert("", "Code has been Copied", "Open Website", "Cancel");
                if (openSite)
                {
                    await Browser.OpenAsync(collectionViewSelectedItem.Brand.Url, BrowserLaunchMode.SystemPreferred);
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
                IsRefreshing = true;

                switch (SelectedTabIndex)
                {
                    case 0:
                        Coupons[0].Coupons.Clear();
                        Coupons[0].PageNumber = 0;
                        break;
                    case 1:
                        Coupons[1].Coupons.Clear();
                        Coupons[1].PageNumber = 0;
                        break;
                    case 2:
                        Coupons[2].Coupons.Clear();
                        Coupons[2].PageNumber = 0;
                        break;
                }

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
