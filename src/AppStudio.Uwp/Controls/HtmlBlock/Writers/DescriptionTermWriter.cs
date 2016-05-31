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
    class DescriptionTermWriter : HtmlWriter
    {
        public override string[] TargetTags
        {
            get { return new string[] { "dt" }; }
        }

        public override DependencyObject GetControl(HtmlFragment fragment)
        {
            return new Paragraph();
        }

        public override void ApplyStyles(DocumentStyle style, DependencyObject ctrl, HtmlFragment fragment)
        {
            ApplyParagraphStyles(ctrl as Paragraph, style.Dt);
        }
    }
}
