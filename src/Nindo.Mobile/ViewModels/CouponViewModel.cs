﻿using System;
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

        #region command
        public IAsyncCommand RefreshCommand { get; }
        public IAsyncCommand LoadMoreCouponsCommand { get; set; }
        public IAsyncCommand<CouponBrands> ComboboxSelectionChangedCommand { get; set; }
        public IAsyncCommand<Coupon> CollectionViewSelectionChangedCommand { get; set; }
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
            CollectionViewSelectionChangedCommand = new AsyncCommand<Coupon>(CopyCouponCode, CanExecute);
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
                var brands = await _apiService.GetCouponBrandsAsync();
                var categories = await _apiService.GetCouponBranchesAsync();

                RangeObservableCollection<CouponBrands> brandItems = new RangeObservableCollection<CouponBrands>();
                brandItems.AddRange(brands);

                RangeObservableCollection<CouponBrands> categoryItems = new RangeObservableCollection<CouponBrands>();
                foreach (var item in categories)
                {
                    categoryItems.Add(new CouponBrands { Name = item });
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    Coupons[1].ComboboxItems = brandItems;
                    Coupons[2].ComboboxItems = categoryItems;
                });

            }
        }

        public async Task LoadDefaultItems()
        {
            var noFilter = await _apiService.GetCouponsAsync(0);
            Device.BeginInvokeOnMainThread(() =>
            {
                Coupons[1].Coupons.AddRange(noFilter.Coupon);
                Coupons[2].Coupons.AddRange(noFilter.Coupon);
            });
        }

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
                            Coupons[1].selectedItem = obj;
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
                            Coupons[2].selectedItem = obj;
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
                    await Task.Run(() =>
                    {

                        switch (SelectedTabIndex)
                        {
                            case 0:
                                var noFilter = _apiService.GetCouponsAsync(Coupons[0].pageNumber);
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    Coupons[0].Coupons.AddRange(noFilter.Result.Coupon);
                                });
                                if (noFilter.Result.HasMore == "true")
                                {
                                    Coupons[0].pageNumber += 20;
                                }
                                else
                                {
                                    hasMore = false;
                                }
                                break;

                            case 1:
                                if (Coupons[1].selectedItem != null)
                                {
                                    var brandFilter = _apiService.GetCouponsByBranchAsync(Coupons[1].selectedItem.Id, Coupons[1].pageNumber);
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        Coupons[1].Coupons.AddRange(brandFilter.Result.Coupon);
                                    });
                                    if (brandFilter.Result.HasMore == "true")
                                    {
                                        Coupons[1].pageNumber += 20;
                                    }
                                    else
                                    {
                                        hasMore = false;
                                    }
                                }
                                break;

                            case 2:
                                if (Coupons[2].selectedItem != null)
                                {
                                    try
                                    {
                                        var categoryFilter = _apiService.GetCouponsByCategoryAsync(Coupons[2].selectedItem.Name, Coupons[2].pageNumber);
                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            Coupons[2].Coupons.AddRange(categoryFilter.Result.Coupon);
                                        });
                                        if (categoryFilter.Result.HasMore == "true")
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

        public async Task CopyCouponCode(Coupon CollectionViewSelectedItem)
        {
            try
            {
                IsBusy = true;

                await Clipboard.SetTextAsync(CollectionViewSelectedItem.Code);
                var openSite = await Application.Current.MainPage.DisplayAlert("", "Code has been Copied", "Open Website", "Cancel");
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
                        Coupons[0].Coupons.Clear();
                        Coupons[0].pageNumber = 0;
                        break;
                    case 1:
                        Coupons[1].Coupons.Clear();
                        Coupons[1].pageNumber = 0;
                        break;
                    case 2:
                        Coupons[2].Coupons.Clear();
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
