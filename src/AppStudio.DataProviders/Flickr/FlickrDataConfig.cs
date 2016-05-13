namespace AppStudio.DataProviders.Flickr
{
    public class FlickrDataConfig
    {
        public FlickrQueryType QueryType { get; set; }

        public string Query { get; set; }

        public string OrderBy { get; set; }

        public SortDirection SortDirection { get; set; }
    }

    public enum FlickrQueryType
    {
        Id,
        Tags
    }
}
