using System;
using System.Collections.Generic;

namespace AppStudio.Uwp.Cache
{
    public class CachedContent<T>
    {
        public DateTime Timestamp { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
