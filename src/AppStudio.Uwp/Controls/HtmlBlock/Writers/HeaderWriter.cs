using AppStudio.Uwp.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace AppStudio.Uwp.Controls.Html.Writers
{
    class HeaderWriter : HtmlWriter
    {
        public override string[] TargetTags
        {
            get { return new string[] { "h1", "h2", "h3", "h4", "h5", "h6" }; }
        }


        public override DependencyObject GetControl(HtmlFragment fragment)
        {
            return new Paragraph();
        }

        public override void ApplyStyles(DocumentStyle style, DependencyObject ctrl, HtmlFragment fragment)
        {
            ApplyParagraphStyles(ctrl as Paragraph, GetDocumentStyle(fragment, style));
        }

        private ParagraphStyle GetDocumentStyle(HtmlFragment fragment, DocumentStyle style)
        {
            if (style == null)
            {
                return null;
            }
            switch (fragment.Name.ToLowerInvariant())
            {
                case "h1":
                    return style.H1;
                case "h2":
                    return style.H2;
                case "h3":
                    return style.H3;
                case "h4":
                    return style.H4;
                case "h5":
                    return style.H5;
                case "h6":
                    return style.H6;
                default:
                    return null;
            }
        }
    }
}
