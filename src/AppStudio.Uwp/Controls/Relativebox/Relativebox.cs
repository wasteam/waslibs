using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace AppStudio.Uwp.Controls
{
    public sealed class Relativebox : ContentControl
    {
        private ContentPresenter _content = null;

        public Relativebox()
        {
            this.DefaultStyleKey = typeof(Relativebox);
            //this.HorizontalAlignment = HorizontalAlignment.Center;
            //this.VerticalAlignment = VerticalAlignment.Center;
        }

        #region AspectRatio
        public double AspectRatio
        {
            get { return (double)GetValue(AspectRatioProperty); }
            set { SetValue(AspectRatioProperty, value); }
        }

        private static void AspectRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // TODO: Review
            var control = d as Relativebox;
            control.InvalidateMeasure();
        }

        public static readonly DependencyProperty AspectRatioProperty = DependencyProperty.Register("AspectRatio", typeof(double), typeof(Relativebox), new PropertyMetadata(1.0, AspectRatioChanged));
        #endregion

        protected override void OnApplyTemplate()
        {
            _content = base.GetTemplateChild("content") as ContentPresenter;

            base.OnApplyTemplate();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            double width;
            double height;
            double aspectRatio = Math.Max(0.0, this.AspectRatio);

            if (!Double.IsInfinity(availableSize.Width))
            {
                width = availableSize.Width;
                height = width / aspectRatio;
            }
            else if (!Double.IsInfinity(availableSize.Height))
            {
                height = availableSize.Height;
                width = height * aspectRatio;
            }
            else
            {
                if (this.MinWidth > 0.0)
                {
                    width = this.MinWidth;
                    height = width / aspectRatio;
                }
                else if (this.MinHeight > 0.0)
                {
                    height = this.MinHeight;
                    width = height * aspectRatio;
                }
                else
                {
                    // TODO: Try by asking base.MeasureOverride(size);
                    width = 512.0;
                    height = width / aspectRatio;
                }
            }

            if (height < MinHeight)
            {
                height = MinHeight;
                width = height * AspectRatio;
            }
            if (height > MaxHeight)
            {
                height = MaxHeight;
                width = height * AspectRatio;
            }

            if (width < MinWidth)
            {
                width = MinWidth;
                height = width * AspectRatio;
            }
            if (width > MaxWidth)
            {
                width = MaxWidth;
                height = width * AspectRatio;
            }

            var size = new Size(width, height);
            base.MeasureOverride(size);
            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var size = base.ArrangeOverride(finalSize);
            return size;
        }
    }
}
