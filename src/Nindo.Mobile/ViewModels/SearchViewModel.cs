using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nindo.Common.Common;
using Nindo.Mobile.Services;
using Nindo.Mobile.ViewModels.BaseViewModels;
using Nindo.Net.Models;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Nindo.Mobile.ViewModels
{
    public class SearchViewModel : ViewModelBase
    {
        private readonly IApiService _apiService;

        #region command

        public IAsyncCommand SearchCommand { get; }

        #endregion

        public SearchViewModel(IApiService apiService)
        {
            _apiService = apiService;
            Title = "Nindo";

            SearchResult = new RangeObservableCollection<Search>();

            SearchCommand = new AsyncCommand(SearchAsync, CanExecute);
        }

        private async Task SearchAsync()
        {
            try
            {
                IsBusy = true;

                if (SearchText != null && SearchText.Length > 2 && SearchText.Length % 2 == 0 && SearchText.Length < 15)
                {
                    SearchResult = await _apiService.SearchUserAsync(SearchText);
                }
            }
            catch (Exception e)
            {
                if (e is Refit.ApiException)
                {
                    await Application.Current.MainPage.DisplayAlert($"Error", $"{e.Message}", "Ok");
                }
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

        private IList<Search> _searchResult;

        public IList<Search> SearchResult
        {
            get => _searchResult;
            set
            {
                _searchResult = value;
                OnPropertyChanged();
            }
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