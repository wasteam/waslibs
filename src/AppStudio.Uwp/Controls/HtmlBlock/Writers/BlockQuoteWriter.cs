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
    class BlockQuoteWriter : HtmlWriter
    {
        public override string[] TargetTags
        {
            get { return new string[] { "blockquote" }; }
        }

        public override DependencyObject GetControl(HtmlFragment fragment)
        {
            return new Paragraph();
        }

        public override void ApplyStyles(DocumentStyle style, DependencyObject ctrl, HtmlFragment fragment)
        {
            ApplyParagraphStyles(ctrl as Paragraph, style.BlockQuote);
        }
    }
}
