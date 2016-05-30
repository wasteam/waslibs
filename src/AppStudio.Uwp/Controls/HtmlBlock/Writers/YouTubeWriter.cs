using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppStudio.Uwp.Html;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using Windows.UI.Xaml.Media;
using Windows.UI;
using AppStudio.Uwp.Navigation;
using System.Text.RegularExpressions;
using Windows.UI.Xaml.Media.Imaging;
using System.Reflection;
using System.IO;

namespace AppStudio.Uwp.Controls.Html.Writers
{
    class YouTubeWriter : IFrameVideoWriter
    {
        public override string[] TargetTags
        {
            get { throw new NotImplementedException(); }
        }

        public override bool Match(HtmlFragment fragment)
        {
            var src = GetIframeSrc(fragment);
            return fragment.Name == "iframe" && !string.IsNullOrWhiteSpace(src) && src.ToLowerInvariant().Contains("www.youtube.com");
        }

        protected override ImageStyle GetStyle(DocumentStyle style)
        {
            return style.YouTube;
        }

        protected override void SetScreenshot(ImageEx img, HtmlNode node)
        {
            img.Source = GetImageSrc(node);
        }

        private static string GetImageSrc(HtmlNode node)
        {
            var regex = new Regex(@"(?:http(?:s?)://www\.youtube\.com/embed/)(?<videoid>[\w_-]*)(?:\??)(?:/?)(?:\.*)");
            var src = GetIframeSrc(node);
            if (!string.IsNullOrEmpty(src))
            {
                var match = regex.Match(src);
                if (match.Success)
                {
                    return $"https://i.ytimg.com/vi/{match.Groups["videoid"].Value}/maxresdefault.jpg";
                }
            }
            return null;
        }
    }
}
