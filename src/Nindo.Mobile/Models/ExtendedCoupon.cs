using Nindo.Common.Common;
using Nindo.Mobile.ViewModels;
using Nindo.Net.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nindo.Mobile.Models
{
    public class ExtendedCoupon : ViewModelBase
    {
        public string CouponTitle { get; set; }

        public int pageNumber; 

        private RangeObservableCollection<Coupon> _coupons = new RangeObservableCollection<Coupon>();

        public RangeObservableCollection<Coupon> Coupons
        {
            get => _coupons;
            set
            {
                _coupons = value;
                OnPropertyChanged();
            }
        }

        private RangeObservableCollection<CouponBrands> _comboboxItems = new RangeObservableCollection<CouponBrands>();

        public RangeObservableCollection<CouponBrands> ComboboxItems
        {
            get => _comboboxItems;
            set
            {
                _comboboxItems = value;
                OnPropertyChanged();
            }
        }


        private bool _comboboxIsVisible;

        public bool ComboboxIsVisible
        {
            get => _comboboxIsVisible;
            set
            {
                _comboboxIsVisible = value;
                OnPropertyChanged();
            }
        }
    }
}
