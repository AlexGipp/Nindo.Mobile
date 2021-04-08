using System;
using System.Threading.Tasks;
using Nindo.Net.Models;

namespace Nindo.Mobile.Models
{
    public class ChartsFilters
    {
        public string FilterTitle { get; set; }
        public Task<Rank[]> FilterMethod { get; set; }
    }
}