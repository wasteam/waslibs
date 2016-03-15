using AppStudio.DataProviders;

namespace AppStudio.Uwp.Samples
{
    public class LocalStorageDataSchema : SchemaBase
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public string Category { get; set; }

        public string ImageUrl { get; set; }
    }
}
