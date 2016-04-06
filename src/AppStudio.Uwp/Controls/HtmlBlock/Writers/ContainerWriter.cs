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
    }
}
