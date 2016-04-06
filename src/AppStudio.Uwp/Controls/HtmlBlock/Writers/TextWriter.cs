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
    class TextWriter : HtmlWriter
    {
        public override string[] TargetTags
        {
            get { return new string[] { "text" }; }
        }

        public override DependencyObject GetControl(HtmlFragment fragment)
        {
            var text = fragment as HtmlText;
            if (text != null && !string.IsNullOrEmpty(text.Content))
            {
                return new Run
                {
                    Text = text.Content
                };
            }
            return null;
        }
    }
}
