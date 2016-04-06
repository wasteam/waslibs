using AppStudio.Uwp.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace AppStudio.Uwp.Controls.Html.Writers
{
    class ListItemWriter : HtmlWriter
    {
        public override string[] TargetTags
        {
            get { return new string[] { "li" }; }
        }

        public override DependencyObject GetControl(HtmlFragment fragment)
        {
            var p = new Paragraph();
            p.Inlines.Add(new Run
            {
                //TODO: THIS COULD BE CONFIGURED IN STYLES
                Text = "\u25CF  ",
            });

            return p;
        }
    }
}
