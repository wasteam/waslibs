using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    class GridColumn : DependencyObject
    {
        public int Index { get; set; }
        public int ColSpan { get; set; }
        public int RowSpan { get; set; }
        public GridRow Row { get; set; }
    }
}
