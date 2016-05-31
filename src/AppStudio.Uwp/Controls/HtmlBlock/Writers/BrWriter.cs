using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppStudio.Uwp.Html;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;

namespace AppStudio.Uwp.Controls.Html.Writers
{
    class BrWriter : HtmlWriter
    {
        public override string[] TargetTags
        {
            get { return new string[] { "br" }; }
        }

        public override DependencyObject GetControl(HtmlFragment fragment)
        {
            return new LineBreak();
        }
    }
}
