using AppStudio.Uwp.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml;

namespace AppStudio.Uwp.Controls.Html.Writers
{
    class StrongWriter : HtmlWriter
    {
        public override string[] TargetTags
        {
            get { return new string[] { "strong", "b" }; }
        }

        public override DependencyObject GetControl(HtmlFragment fragment)
        {
            return new Bold();
        }
    }
}
