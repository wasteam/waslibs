using AppStudio.Uwp.Html;
using AppStudio.Uwp.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Reflection;
using System.IO;
using Windows.UI.Xaml.Input;
using Windows.UI.Core;

namespace AppStudio.Uwp.Controls.Html.Writers
{
    abstract class IFrameVideoWriter : HtmlWriter
    {
        private static readonly CoreCursor _arrowCursor = new CoreCursor(CoreCursorType.Hand, 0);
        private static readonly CoreCursor _handCursor = new CoreCursor(CoreCursorType.Hand, 1);

        protected abstract void SetScreenshot(ImageEx img, HtmlNode node);
        protected abstract ImageStyle GetStyle(DocumentStyle style);

        public override DependencyObject GetControl(HtmlFragment fragment)
        {
            var node = fragment as HtmlNode;
            if (node != null)
            {
                var grid = new Grid
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                };

                grid.Tapped += (sender, e) =>
                {
                    NavigationService.NavigateTo(new Uri(GetIframeSrc(node))).RunAndForget();
                };

                AddColumn(grid);
                AddColumn(grid);
                AddColumn(grid);

                var screenShot = GetImageControl((i) => SetScreenshot(i, node));

                screenShot.PointerEntered += (sender, e) =>
                {
                    Window.Current.CoreWindow.PointerCursor = _handCursor;
                };
                screenShot.PointerExited += (sender, e) =>
                {
                    Window.Current.CoreWindow.PointerCursor = _arrowCursor;
                };

                Grid.SetColumn(screenShot, 0);
                Grid.SetColumnSpan(screenShot, 3);
                grid.Children.Add(screenShot);

                var player = GetImageControl((i) => i.Source = GetPlayerImage());

                Grid.SetColumn(player, 1);
                grid.Children.Add(player);

                return grid;
            }

            return null;
        }

        public override void ApplyStyles(DocumentStyle style, DependencyObject ctrl, HtmlFragment fragment)
        {
            var grid = ctrl as Grid;
            var vb = grid.GetChild<Viewbox>(0, 0);

            ApplyImageStyles(vb, GetStyle(style));
        }

        protected static string GetIframeSrc(HtmlFragment fragment)
        {
            var node = fragment.AsNode();
            if (node != null)
            {
                if (node.Attributes.ContainsKey("src"))
                {
                    return node.Attributes["src"];
                }
            }
            return string.Empty;
        }

        protected static BitmapImage GetEmbebedImage(string name)
        {
            var assembly = typeof(IFrameVideoWriter).GetTypeInfo().Assembly;

            using (var stream = assembly.GetManifestResourceStream(name))
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

        private static BitmapImage GetPlayerImage()
        {
            return GetEmbebedImage("AppStudio.Uwp.Controls.HtmlBlock.PlayButton.png");
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
    }
}
