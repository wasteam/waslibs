using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace AppStudio.Uwp.Controls
{
    public partial class SliderViewPanel : Panel
    {
        public SliderViewPanel()
        {
            this.HorizontalAlignment = HorizontalAlignment.Left;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            double availableWidth = Math.Min(Window.Current.Bounds.Width, availableSize.Width);

            this.ArrangePanes(availableWidth);

            int index = this.Index;
            int count = _items.Count;

            double x = 0;
            double itemWidth = this.ItemWidth;
            double maxHeight = 0;

            if (count > 0)
            {
                int itemsPerThird = (int)Math.Ceiling(availableWidth / this.ItemWidth);
                for (int n = 0; n < base.Children.Count; n++)
                {
                    int i = n - itemsPerThird;

                    int inx = (index + i).Mod(base.Children.Count);
                    var pane = base.Children[inx] as ContentControl;

                    if (i < itemsPerThird + 1)
                    {
                        pane.ContentTemplate = ItemTemplate;
                        pane.Content = _items[(index + i).Mod(count)];
                        pane.Tag = inx;
                    }
                    else
                    {
                        pane.ContentTemplate = null;
                        pane.Content = null;
                        pane.Tag = null;
                    }

                    pane.Measure(new Size(itemWidth, availableSize.Height));
                    maxHeight = Math.Max(maxHeight, pane.DesiredSize.Height);

                    x += itemWidth;
                }
            }

            return new Size(availableWidth, maxHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double finalWidth = finalSize.Width;

            int index = this.Index;
            int count = _items.Count;

            double x = 0;
            double itemWidth = this.ItemWidth;

            if (count > 0)
            {
                int itemsPerThird = (int)Math.Ceiling(finalWidth / this.ItemWidth);
                for (int n = -itemsPerThird; n < itemsPerThird * 2; n++)
                {
                    if (index + n >= 0 && index + n < count)
                    {
                        int inx = (index + n).Mod(base.Children.Count);
                        var pane = base.Children[inx] as ContentControl;
                        pane.Arrange(new Rect(index * ItemWidth + x - itemWidth * itemsPerThird, 0, itemWidth, finalSize.Height));
                    }

                    x += itemWidth;
                }
            }

            return new Size(0, finalSize.Height);
        }

        private void ArrangePanes(double availableWidth)
        {
            int count = 3 * (int)Math.Ceiling(availableWidth / this.ItemWidth);
            count = Math.Max(count, _items.Count);

            int diff = count - base.Children.Count;

            if (diff > 0)
            {
                for (int n = 0; n < diff; n++)
                {
                    var pane = CreatePane();
                    pane.Tapped += OnItemTapped;
                    base.Children.Add(pane);
                }
            }
            else
            {
                for (int n = 0; n < -diff; n++)
                {
                    base.Children.RemoveAt(base.Children.Count - 1);
                }
            }
        }

        private static ContentControl CreatePane()
        {
            return new ContentControl
            {
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch
            };
        }
    }
}
