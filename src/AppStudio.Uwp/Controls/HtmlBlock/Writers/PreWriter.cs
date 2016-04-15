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
    class PreWriter : HtmlWriter
    {
        public override string[] TargetTags
        {
            get { return new string[] { "pre" }; }
        }

        public override DependencyObject GetControl(HtmlFragment fragment)
        {
            return new Paragraph();
        }

        public override void ApplyStyles(DocumentStyle style, DependencyObject ctrl, HtmlFragment fragment)
        {
            var p = ctrl as Paragraph;
            ApplyParagraphStyles(ctrl as Paragraph, style.Pre);
        }
    }
}
