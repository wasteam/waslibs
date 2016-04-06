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
    class SpanWriter : HtmlWriter
    {
        public override string[] TargetTags
        {
            get { return new string[] { "span" }; }
        }

        public override DependencyObject GetControl(HtmlFragment fragment)
        {
            //TODO: GET FROM STYLES
            return new Span
            {
                Foreground = new SolidColorBrush(Colors.Lime)
            };
        }
    }
}
