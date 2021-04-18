using System;
using System.Threading.Tasks;
using Nindo.Mobile.Services;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Nindo.Mobile.ViewModels
{
    public class SearchViewModel : ViewModelBase
    {

        #region command

        public IAsyncCommand SearchCommand { get; }

        #endregion

        private readonly IApiService _apiService;

        public SearchViewModel(IApiService apiService)
        {
            _apiService = apiService;
            Title = "Nindo";

            SearchCommand = new AsyncCommand(SearchAsync, CanExecute);

        }

        private async Task SearchAsync()
        {
            try
            {
                IsBusy = true;

                await _apiService.SearchUserAsync(SearchText);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanExecute(object arg)
        {
            return !IsBusy;
        }

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }
    }
}