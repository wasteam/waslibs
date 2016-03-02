using System;

namespace AppStudio.Uwp.Samples
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SamplePageAttribute : Attribute
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public int Order { get; set; } = 1000;
    }
}
