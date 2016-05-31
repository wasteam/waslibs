using System;

using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Samples
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SamplePageAttribute : Attribute
    {
        public SamplePageAttribute()
        {
            this.Symbol = Symbol.Document;
            this.Order = 1000;
        }

        public string Category { get; set; }
        public string Name { get; set; }

        public Symbol Symbol { get; set; }
        public string Glyph { get; set; }
        public string IconPath { get; set; }

        public int Order { get; set; }
    }
}
