using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppStudio.Uwp.Html;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls.Html.Writers
{
    class TableRowWriter : HtmlWriter
    {
        public override string[] TargetTags
        {
            get { return new string[] { "tr" }; }
        }

        public override DependencyObject GetControl(HtmlFragment fragment)
        {
            return new GridRow();
        }
    }
}
