using System.Threading.Tasks;
using MvvmHelpers;
using Nindo.Mobile.Views;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Nindo.Mobile.ViewModels
{
    public class ViewModelBase : BaseViewModel
    {
        #region command

        public IAsyncCommand OpenSearchCommand { get; }

        #endregion

        public ViewModelBase()
        {
            OpenSearchCommand = new AsyncCommand(NavigateToSearchPageAsync, CanExecute);
        }

        private async Task NavigateToSearchPageAsync()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SearchPage());
        }

        private bool CanExecute(object arg)
        {
            return !IsBusy;
        }
    }
}