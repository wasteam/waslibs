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
    class YouTubeWriter : HtmlWriter
    {
        public override string[] TargetTags
        {
            get { throw new NotImplementedException(); }
        }

        public override bool Match(HtmlFragment fragment)
        {
            var node = fragment.AsNode();
            if (node != null)
            {
                var src = GetIframeSrc(node);
                return !string.IsNullOrWhiteSpace(src) && src.ToLowerInvariant().Contains("www.youtube.com");
            }
            return false;
        }

        public override DependencyObject GetControl(HtmlFragment fragment)
        {
            var node = fragment as HtmlNode;
            var src = GetImageSrc(node);
            if (node != null && !string.IsNullOrEmpty(src))
            {
                var grid = new Grid();

                grid.Tapped += (sender, e) =>
                {
                    NavigationService.NavigateTo(new Uri(GetIframeSrc(node))).RunAndForget();
                };

                AddColumn(grid);
                AddColumn(grid);
                AddColumn(grid);

                var screenShot = GetImageControl((i) => i.Source = src);

                Grid.SetColumn(screenShot, 0);
                Grid.SetColumnSpan(screenShot, 3);
                grid.Children.Add(screenShot);

                var player = GetImageControl((i) => i.Source = GetPlayerImage());
                player.MaxWidth = 80;

                Grid.SetColumn(player, 1);
                grid.Children.Add(player);

                return grid;
            }

            return null;
        }

        public override void ApplyStyles(DocumentStyle style, DependencyObject ctrl, HtmlFragment fragment)
        {
            ApplyImageStyles(ctrl as Grid, style.YouTube);
        }

        public static BitmapImage GetPlayerImage()
        {
            var assembly = typeof(YouTubeWriter).GetTypeInfo().Assembly;

            using (var stream = assembly.GetManifestResourceStream("AppStudio.Uwp.Controls.HtmlBlock.YouTubePlay.png"))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    ms.Position = 0;
                    var image = new BitmapImage();
                    image.SetSource(ms.AsRandomAccessStream());
                    return image;  
                }
            }

        }

        private static Viewbox GetImageControl(Action<ImageEx> setSource)
        {
            var viewbox = new Viewbox
            {
                StretchDirection = StretchDirection.DownOnly
            };

            var image = new ImageEx
            {
                Stretch = Stretch.Uniform,
                Background = new SolidColorBrush(Colors.Transparent),
                Foreground = new SolidColorBrush(Colors.Transparent)
            };
            setSource(image);
            viewbox.Child = image;

            return viewbox;
        }

        private static void AddColumn(Grid grid)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
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

        private static string GetIframeSrc(HtmlNode node)
        {
            if (node.Attributes.ContainsKey("src"))
            {
                return node.Attributes["src"];
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
