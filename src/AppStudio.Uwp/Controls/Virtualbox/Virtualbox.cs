using System;
using System.IO;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AppStudio.Uwp.Controls
{
    public sealed partial class Virtualbox : ContentControl
    {
        private ContentPresenter _content = null;

        private CompositeTransform _transform;
        private RectangleGeometry _clip = null;

        public Virtualbox()
        {
            this.DefaultStyleKey = typeof(Virtualbox);
            this.SizeChanged += OnSizeChanged;
        }

        protected override void OnApplyTemplate()
        {
            _content = base.GetTemplateChild("content") as ContentPresenter;
            _clip = base.GetTemplateChild("clip") as RectangleGeometry;

            _transform = _content.RenderTransform as CompositeTransform;

            base.OnApplyTemplate();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            double width = this.VirtualWidth;
            double height = this.VirtualHeight;

            if (width > 0.0 & height > 0.0)
            {
                if (Double.IsInfinity(availableSize.Width) || Double.IsInfinity(availableSize.Height))
                {
                    double aspectRatio = width / height;

                    if (!Double.IsInfinity(availableSize.Width))
                    {
                        width = availableSize.Width;
                        height = width / aspectRatio;
                        var size = new Size(width, height);
                        base.MeasureOverride(size);
                        return size;
                    }
                    else if (!Double.IsInfinity(availableSize.Height))
                    {
                        height = availableSize.Height;
                        width = height * aspectRatio;
                        var size = new Size(width, height);
                        base.MeasureOverride(size);
                        return size;
                    }
                }
            }

            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double width = this.VirtualWidth;
            double height = this.VirtualHeight;

            if (width > 0.0 & height > 0.0)
            {
                if (Double.IsInfinity(finalSize.Width) || Double.IsInfinity(finalSize.Height))
                {
                    double aspectRatio = width / height;

                    if (!Double.IsInfinity(finalSize.Width))
                    {
                        width = finalSize.Width;
                        height = width / aspectRatio;
                        var size = new Size(width, height);
                        base.ArrangeOverride(size);
                        return size;
                    }
                    else if (!Double.IsInfinity(finalSize.Height))
                    {
                        height = finalSize.Height;
                        width = height * aspectRatio;
                        var size = new Size(width, height);
                        base.ArrangeOverride(size);
                        return size;
                    }
                }
            }

            return base.ArrangeOverride(finalSize);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ArrangeContent();
        }
    }
}
