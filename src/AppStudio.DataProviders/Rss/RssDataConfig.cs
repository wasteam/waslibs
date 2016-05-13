using System;

namespace AppStudio.DataProviders.Rss
{
    public class RssDataConfig
    {
        public Uri Url { get; set; }

        public string OrderBy { get; set; }

        public SortDirection SortDirection { get; set; }
    }   
}
