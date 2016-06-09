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
    class ContainerWriter : HtmlWriter
    {
        public override string[] TargetTags
        {
            get { return new string[] { "div", "ul", "ol", "dl", "section", "article", "header", "footer", "main", "figure", "details", "summary", "tbody" };  }
        }

        public override DependencyObject GetControl(HtmlFragment fragment)
        {
            return new Grid();
        }

        public override void ApplyStyles(DocumentStyle style, DependencyObject ctrl, HtmlFragment fragment)
        {
            ApplyContainerStyles(ctrl as Grid, GetDocumentStyle(fragment, style));
        }

        private static ContainerStyle GetDocumentStyle(HtmlFragment fragment, DocumentStyle style)
        {
            if (style == null)
            {
                return null;
            }
            switch (fragment.Name.ToLowerInvariant())
            {
                case "div":
                case "tbody":
                    return style.Div;
                case "ul":
                    return style.Ul;
                case "ol":
                    return style.Ol;
                case "dl":
                    return style.Dl;
                case "section":
                    return style.Section;
                case "article":
                    return style.Article;
                case "header":
                    return style.Header;
                case "footer":
                    return style.Footer;
                case "main":
                    return style.Main;
                case "figure":
                    return style.Figure;
                case "details":
                    return style.Details;
                case "summary":
                    return style.Summary;
                default:
                    return null;
            }
        }
    }
}
