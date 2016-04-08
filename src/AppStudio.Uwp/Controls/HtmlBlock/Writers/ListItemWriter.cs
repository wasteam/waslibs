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
            return new Paragraph();
        }

        public override void ApplyStyles(DocumentStyle style, DependencyObject ctrl, HtmlFragment fragment)
        {
            var li = ctrl as Paragraph;
            var currentStyle = style.Li;

            if (!string.IsNullOrEmpty(currentStyle?.Bullet))
            {
                li.Inlines.Insert(0, new Run
                {
                    Text = currentStyle.Bullet
                });
                li.Inlines.Insert(1, new Run
                {
                    Text = " "
                });
            }
            ApplyParagraphStyles(li, currentStyle);
        }
    }
}
