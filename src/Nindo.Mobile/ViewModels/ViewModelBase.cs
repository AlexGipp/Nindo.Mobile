using System;
using System.Threading.Tasks;
using MvvmHelpers;
using Nindo.Mobile.Services;
using Nindo.Mobile.Views;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Nindo.Mobile.ViewModels
{
    public class ViewModelBase : BaseViewModel
    {

        private bool CanExecute(object arg)
        {
            return !IsBusy;
        }
    }
}