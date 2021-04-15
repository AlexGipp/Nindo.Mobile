using System;
using System.Threading.Tasks;
using Nindo.Net.Models;

namespace Nindo.Mobile.Models
{
    public class ChartsFilter
    {
        public string FilterTitle { get; set; }
        public Func<Task<Rank[]>> FilterMethod { get; set; }
    }
}