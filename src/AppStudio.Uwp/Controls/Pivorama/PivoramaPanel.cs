using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace AppStudio.Uwp.Controls
{
    public partial class PivoramaPanel : Panel
    {
        public PivoramaPanel()
        {
            this.BuildPanes();
            this.HorizontalAlignment = HorizontalAlignment.Left;
        }

        protected virtual int MaxItems
        {
            get { return 16; }
        }

        private void BuildPanes()
        {
            for (int n = 0; n < MaxItems; n++)
            {
                var pane = new ContentControl
                {
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Stretch
                };
                pane.Tapped += OnItemTapped;
                this.Children.Add(pane);
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            int inx = this.Index;
            int count = _items.Count;

            double x = 0;
            double itemWidth = this.ItemWidth;
            double maxHeight = 0;

            if (count > 0)
            {
                for (int n = 0; n < MaxItems; n++)
                {
                    var pane = this.Children[(inx + n).Mod(MaxItems)] as ContentControl;
                    if (x < availableSize.Width + itemWidth * 2 && n <= count)
                    {
                        pane.Content = _items[(inx + n - 1).Mod(count)];
                        pane.Measure(new Size(itemWidth, availableSize.Height));
                        if (n > 0 && x < availableSize.Width + itemWidth)
                        {
                            maxHeight = Math.Max(maxHeight, pane.DesiredSize.Height);
                        }
                        x += itemWidth;
                    }
                    else
                    {
                        pane.Content = null;
                        pane.Measure(new Size(0, 0));
                    }
                }
            }

            return new Size(x, maxHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            int inx = this.Index;
            int count = _items.Count;

            double x = 0;
            double itemWidth = this.ItemWidth;

            if (count > 0)
            {
                for (int n = 0; n < MaxItems; n++)
                {
                    var pane = this.Children[(inx + n).Mod(MaxItems)] as ContentControl;
                    if (x < finalSize.Width)
                    {
                        pane.Arrange(new Rect(inx * ItemWidth + x - itemWidth, 0, itemWidth, finalSize.Height));
                    }
                    else
                    {
                        break;
                    }
                    x += itemWidth;
                }
            }

            return new Size(0, finalSize.Height);
        }
    }
}
