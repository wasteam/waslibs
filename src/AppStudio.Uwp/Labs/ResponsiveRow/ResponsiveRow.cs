using System;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Labs
{
    public sealed class ResponsiveRow : ContentControl
    {
        public ResponsiveRow()
        {
            this.DefaultStyleKey = typeof(ResponsiveRow);
            this.VerticalAlignment = VerticalAlignment.Top;
            this.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            this.VerticalContentAlignment = VerticalAlignment.Stretch;
        }

        #region AspectRatio
        public double AspectRatio
        {
            get { return (double)GetValue(AspectRatioProperty); }
            set { SetValue(AspectRatioProperty, value); }
        }

        public static readonly DependencyProperty AspectRatioProperty = DependencyProperty.Register("AspectRatio", typeof(double), typeof(ResponsiveRow), new PropertyMetadata(3.0, RefreshLayout));
        #endregion

        #region RelativePadding
        public Thickness RelativePadding
        {
            get { return (Thickness)GetValue(RelativePaddingProperty); }
            set { SetValue(RelativePaddingProperty, value); }
        }

        public static readonly DependencyProperty RelativePaddingProperty = DependencyProperty.Register("RelativePadding", typeof(Thickness), typeof(ResponsiveRow), new PropertyMetadata(null));
        #endregion

        private static void RefreshLayout(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ResponsiveRow;
            control.InvalidateMeasure();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            double height = availableSize.Width / this.AspectRatio;
            height = Math.Max(height, this.MinHeight);
            height = Math.Min(height, this.MaxHeight);

            double ratio = Math.Min(2, height / this.MinHeight);
            this.RelativePadding = new Thickness(this.Padding.Left * ratio, this.Padding.Top * ratio, this.Padding.Right * ratio, this.Padding.Bottom * ratio);

            var size = new Size(availableSize.Width, height);
            base.MeasureOverride(size);
            return size;
        }
    }
}
