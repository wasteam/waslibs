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
            get { return new string[] { "p", "div", "ul", "ol" };  }
        }

        public override DependencyObject GetControl(HtmlFragment fragment)
        {
            return new Grid();
        }

        public override void ApplyStyles(DocumentStyle style, DependencyObject ctrl, HtmlFragment fragment)
        {
            ApplyContainerStyles(ctrl as Grid, GetDocumentStyle(fragment, style));
        }

        private ContainerStyle GetDocumentStyle(HtmlFragment fragment, DocumentStyle style)
        {
            if (style == null)
            {
                return null;
            }
            switch (fragment.Name.ToLowerInvariant())
            {
                case "p":
                    return style.P;
                case "div":
                    return style.Div;
                case "ul":
                    return style.Ul;
                case "ol":
                    return style.Ol;
                default:
                    return null;
            }
        }
    }
}
