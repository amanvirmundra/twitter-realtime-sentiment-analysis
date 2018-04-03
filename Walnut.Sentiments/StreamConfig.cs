using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Walnut.Sentiments
{
    public class StreamConfig
    {
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string AccessToken { get; set; }
        public string AccessSecret { get; set; }
        public bool GeoOnly { get; set; }

        public List<string> TrackKeywords = new List<string>();
    }
}