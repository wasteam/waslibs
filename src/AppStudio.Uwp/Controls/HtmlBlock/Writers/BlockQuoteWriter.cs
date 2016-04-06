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

        //TODO: SHOULD RETURN A GRID?
        public override DependencyObject GetControl(HtmlFragment fragment)
        {
            //TODO: GET FROM STYLES
            return new Paragraph
            {
                Margin = new Thickness(20, 0, 0, 0)
            };
        }
    }
}
