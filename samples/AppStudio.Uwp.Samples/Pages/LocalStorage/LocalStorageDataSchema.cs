using AppStudio.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
