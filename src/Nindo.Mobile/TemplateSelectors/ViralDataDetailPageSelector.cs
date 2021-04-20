using Nindo.Mobile.Models;
using Xamarin.Forms;

namespace Nindo.Mobile.TemplateSelectors
{
    public class ViralDataDetailPageSelector : DataTemplateSelector
    {
        public DataTemplate TwitchDataTemplate { get; set; }
        public DataTemplate RegularDataTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((ExtendedViral)item).ViralEntry.Platform == "twitch" ? TwitchDataTemplate : RegularDataTemplate;
        }
    }
}