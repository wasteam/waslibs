using AppStudio.Uwp.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;

namespace AppStudio.Uwp.Controls.Html.Writers
{
    class CodeWriter : HtmlWriter
    {
        public override string[] TargetTags
        {
            get { return new string[] { "code" }; }
        }

        public override DependencyObject GetControl(HtmlFragment fragment)
        {
            return new Span
            {
                //TODO: GET FRON STYLES
                FontFamily = new FontFamily("Courier New")
            };
        }
    }
}
