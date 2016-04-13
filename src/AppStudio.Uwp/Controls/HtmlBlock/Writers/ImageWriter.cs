using AppStudio.Uwp.Html;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace AppStudio.Uwp.Controls.Html.Writers
{
    class ImageWriter : HtmlWriter
    {
        public override string[] TargetTags
        {
            get { return new string[] { "img" }; }
        }

        public override DependencyObject GetControl(HtmlFragment fragment)
        {
            //TODO: SET STYLES (MAX WIDTH) & HIDE IMAGES LESS THAN X WIDTH
            var node = fragment as HtmlNode;
            if (node != null && node.Attributes.ContainsKey("src"))
            {
                Uri uri;

                if (Uri.TryCreate(node.Attributes["src"], UriKind.Absolute, out uri))
                {
                    try
                    {
                        return new ImageEx
                        {
                            Source = new BitmapImage(uri),
                            Stretch = Stretch.Uniform
                        };
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error loading img@src '{uri?.ToString()}': {ex.Message}");
                    }
                }
            }
            return null;
        }
    }
}
