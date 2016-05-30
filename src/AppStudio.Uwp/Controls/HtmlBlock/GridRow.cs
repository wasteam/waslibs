using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    class GridRow : DependencyObject
    {
        public int Index { get; set; }
        public Grid Container { get; set; }
    }
}
