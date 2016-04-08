using AppStudio.DataProviders;

namespace AppStudio.Uwp.Samples
{
    public class LocalStorageDataSchema : SchemaBase
    {
        public string Title { get; set; }       

        public string Category { get; set; }

        public string Thumbnail { get; set; }
    }
}
