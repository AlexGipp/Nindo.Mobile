using System.Threading.Tasks;

namespace Nindo.Mobile.Models
{
    public class ChartsFilter
    {
        public string FilterTitle { get; set; }
        public Task FilterMethod { get; set; }
    }
}