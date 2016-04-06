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
            get { return new string[] { "h1", "h2", "h3", "h4", "h5", "h6", }; }
        }

        public override DependencyObject GetControl(HtmlFragment fragment)
        {
            var p = new Paragraph();

            p.FontSize *= GetFontSizeWeight(fragment);
            p.FontWeight = FontWeights.SemiBold;

            return p;
        }

        private float GetFontSizeWeight(HtmlFragment fragment)
        {
            //TODO: GET IT FROM STYLES
            switch (fragment.Name.ToLowerInvariant())
            {
                case "h1":
                    return 2;
                case "h2":
                    return 1.5f;
                case "h3":
                    return 1.17f;
                case "h4":
                    return 1;
                case "h5":
                    return 0.83f;
                case "h6":
                    return 0.67f;
                default:
                    return 1;
            }
        }
    }
}
