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

        public bool ItemsFitContent { get; private set; }

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
            int index = this.Index;
            int count = _items.Count;

            double x = 0;
            double itemWidth = this.ItemWidth;
            double maxHeight = 0;

            if (count > 0)
            {
                for (int n = 0; n < MaxItems; n++)
                {
                    var pane = this.Children[(index + n).Mod(MaxItems)] as ContentControl;
                    if (x < availableSize.Width + itemWidth * 2 && n <= count)
                    {
                        int inx = (index + n - 1).Mod(count);
                        pane.ContentTemplate = ItemTemplate;
                        pane.Content = _items[inx];
                        pane.Tag = inx;

                        pane.Measure(new Size(itemWidth, availableSize.Height));
                        if (n > 0 && x < availableSize.Width + itemWidth)
                        {
                            maxHeight = Math.Max(maxHeight, pane.DesiredSize.Height);
                        }
                        x += itemWidth;
                    }
                    else
                    {
                        pane.ContentTemplate = null;
                        pane.Content = null;
                        pane.Tag = null;

                        pane.Measure(new Size(itemWidth, availableSize.Height));
                    }
                }
            }

            ItemsFitContent = x - itemWidth < availableSize.Width;

            return new Size(x, maxHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            int index = this.Index;
            int count = _items.Count;

            double x = 0;
            double itemWidth = this.ItemWidth;

            if (count > 0)
            {
                for (int n = 0; n < MaxItems; n++)
                {
                    var pane = this.Children[(index + n).Mod(MaxItems)] as ContentControl;
                    if (x < finalSize.Width)
                    {
                        pane.Arrange(new Rect(index * ItemWidth + x - itemWidth, 0, itemWidth, finalSize.Height));
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
