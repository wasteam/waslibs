using AppStudio.Uwp.Html;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI;
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
            //TODO: HIDE IMAGES LESS THAN CERTAIN WIDTH?

            var node = fragment as HtmlNode;
            var src = GetImageSrc(node);
            if (node != null && !string.IsNullOrEmpty(src))
            {
                Uri uri;

                if (Uri.TryCreate(src, UriKind.Absolute, out uri))
                {
                    try
                    {
                        var viewbox = new Viewbox
                        {
                            StretchDirection = StretchDirection.DownOnly
                        };
                        viewbox.Child = new ImageEx
                        {
                            Source = src,
                            Stretch = Stretch.Uniform,
                            Background = new SolidColorBrush(Colors.Transparent),
                            Foreground = new SolidColorBrush(Colors.Transparent)
                        };

                        return viewbox;

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error loading img@src '{uri?.ToString()}': {ex.Message}");
                    }
                }
            }
            return null;
        }

        public override void ApplyStyles(DocumentStyle style, DependencyObject ctrl, HtmlFragment fragment)
        {
            ApplyImageStyles(ctrl as Viewbox, style.Img);
        }

        private static string GetImageSrc(HtmlNode img)
        {
            if (img.Attributes.ContainsKey("src"))
            {
                return img.Attributes["src"];
            }
            else if (img.Attributes.ContainsKey("srcset"))
            {
                var regex = new Regex(@"(?:(?<src>[^\""'\s,]+)\s*(?:\s+\d+[wx])(?:,\s*)?)");
                var matches = regex.Matches(img.Attributes["srcset"]);

                if (matches.Count > 0)
                {
                    var m = matches[matches.Count - 1];
                    return m?.Groups["src"].Value;
                }
            }

            return null;
        }
    }
}
