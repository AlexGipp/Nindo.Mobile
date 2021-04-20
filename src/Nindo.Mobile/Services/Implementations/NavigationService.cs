using System.Collections.Generic;
using System.Threading.Tasks;
using Nindo.Mobile.Views;
using Nindo.Mobile.Views.DetailPages;
using Nindo.Net.Models;
using Xamarin.Forms;

namespace Nindo.Mobile.Services.Implementations
{
    public class NavigationService : INavigationService
    {
        public async Task OpenViralDetailPage(IList<Viral> entries)
        {
            if (entries != null)
                await Application.Current.MainPage.Navigation.PushAsync(new ViralDetailPage(entries), true);
        }
    }
}