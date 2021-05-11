using MvvmHelpers;

namespace Nindo.Mobile.ViewModels.BaseViewModels
{
    public class ViewModelBase : BaseViewModel
    {

        private bool CanExecute(object arg)
        {
            return !IsBusy;
        }
    }
}