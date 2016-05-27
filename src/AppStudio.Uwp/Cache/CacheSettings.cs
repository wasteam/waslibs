using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.Uwp.Cache
{
    [Obsolete("Use a custom loading strategy in your app")]
    public class CacheSettings
    {
        public string Key { get; set; }
        public TimeSpan Expiration { get; set; }
        public bool NeedsNetwork { get; set; }
        public bool UseStorage { get; set; }
    }
}
