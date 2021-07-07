using System;
using System.Collections.Generic;
using System.Text;

namespace Nindo.Mobile.Models
{
    public class YtChannelHistory
    {
        public ulong Follower { get; set; }
        public ulong? Views { get; set; }
        public string Timestamp { get; set; }
        public ulong Difference { get; set; }
    }
}
