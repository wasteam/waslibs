using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace AppStudio.Uwp.Controls
{
    public sealed class ResponsiveGridViewPanelEx : Panel
    {
        internal bool IsReady { get; set; } = false;

        #region DesiredItemWidth
        public double DesiredItemWidth
        {
            get { return (double)GetValue(DesiredItemWidthProperty); }
            set { SetValue(DesiredItemWidthProperty, value); }
        }

        public static readonly DependencyProperty DesiredItemWidthProperty = DependencyProperty.Register("DesiredItemWidth", typeof(double), typeof(ResponsiveGridViewPanelEx), new PropertyMetadata(0.0, DesiredSizeChanged));
        #endregion

        #region DesiredItemHeight
        public double DesiredItemHeight
        {
            get { return (double)GetValue(DesiredItemHeightProperty); }
            set { SetValue(DesiredItemHeightProperty, value); }
        }

        public static readonly DependencyProperty DesiredItemHeightProperty = DependencyProperty.Register("DesiredItemHeight", typeof(double), typeof(ResponsiveGridViewPanelEx), new PropertyMetadata(0.0, DesiredSizeChanged));
        #endregion

        #region OneRowModeEnabled
        public bool OneRowModeEnabled
        {
            get { return (bool)GetValue(OneRowModeEnabledProperty); }
            set { SetValue(OneRowModeEnabledProperty, value); }
        }

        public static readonly DependencyProperty OneRowModeEnabledProperty = DependencyProperty.Register("OneRowModeEnabled", typeof(bool), typeof(ResponsiveGridViewPanelEx), new PropertyMetadata(false, DesiredSizeChanged));
        #endregion

        private static void DesiredSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ResponsiveGridViewPanelEx;
            control.InvalidateMeasure();
        }

        private double InternalItemWidth
        {
            get
            {
                double itemWidth = this.DesiredItemWidth;
                itemWidth = itemWidth > 0.0 ? itemWidth : this.DesiredItemHeight;
                itemWidth = itemWidth > 0.0 ? itemWidth : 200.0;
                return itemWidth;
            }
        }

        private double InternalItemHeight
        {
            get
            {
                double itemHeight = this.DesiredItemHeight;
                itemHeight = itemHeight > 0.0 ? itemHeight : this.InternalItemWidth;
                return itemHeight;
            }
        }

        public double AspectRatio
        {
            get { return this.InternalItemHeight / this.InternalItemWidth; }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (this.IsReady && base.Children.Count > 0)
            {
                availableSize = NormalizeSize(availableSize);
                var itemSize = GetItemSize(availableSize);

                foreach (var item in base.Children)
                {
                    item.Measure(itemSize);
                }

                int columns = InferColumns(availableSize.Width);
                int rows = (int)Math.Ceiling((double)base.Children.Count / (double)columns);
                if (this.OneRowModeEnabled)
                {
                    rows = 1;
                }

                return new Size(availableSize.Width, rows * itemSize.Height);
            }

            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (this.IsReady && base.Children.Count > 0)
            {
                finalSize = NormalizeSize(finalSize);

                var itemSize = GetItemSize(finalSize);

                double x = 0;
                double y = 0;
                double h = 0;
                foreach (var item in base.Children)
                {
                    h = Math.Max(y, h);

                    item.Arrange(new Rect(new Point(x, y), itemSize));
                    x += itemSize.Width;

                    double nextX = x + itemSize.Width;
                    if (nextX > finalSize.Width + 1)
                    {
                        if (!this.OneRowModeEnabled)
                        {
                            x = 0;
                            y += itemSize.Height;
                        }
                    }
                }

                return new Size(finalSize.Width, h + itemSize.Height);
            }

            return base.ArrangeOverride(finalSize);
        }

        private Size GetItemSize(Size availableSize)
        {
            int columns = InferColumns(availableSize.Width);

            double maxItemWidth = this.InternalItemWidth * 1.25;
            if (columns > base.Children.Count)
            {
                if (maxItemWidth * base.Children.Count < availableSize.Width)
                {
                    return CreateSize(maxItemWidth);
                }
                return CreateSize(availableSize.Width / (columns - 1));
            }

            double cw = availableSize.Width / columns;
            double ch = cw * this.AspectRatio;

            return new Size(cw, ch);
        }

        private int InferColumns(double availableWidth)
        {
            return (int)Math.Max(1, availableWidth / this.InternalItemWidth);
        }

        private Size CreateSize(double width)
        {
            return new Size(width, width * this.AspectRatio);
        }

        #region NormalizeSize
        private Size NormalizeSize(Size size)
        {
            double width = size.Width;
            double height = size.Height;

            if (double.IsInfinity(width))
            {
                width = Window.Current.Bounds.Width;
            }
            if (double.IsInfinity(height))
            {
                height = Window.Current.Bounds.Height;
            }

            return new Size(width, height);
        }
        #endregion
    }
}
