using AppStudio.DataProviders;

namespace AppStudio.Samples.DataProviders.ControlPages
{
    public class LocalStorageDataSchema : SchemaBase
    {
        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }
    }
}