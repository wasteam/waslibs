using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Foundation;

namespace AppStudio.Uwp.Controls
{
    public sealed class Virtualbox : ContentControl
    {
        private ContentPresenter _content = null;

        private CompositeTransform _transform;

        public Virtualbox()
        {
            this.DefaultStyleKey = typeof(Virtualbox);
            this.SizeChanged += OnSizeChanged;
        }

        #region VirtualWidth
        public double VirtualWidth
        {
            get { return (double)GetValue(VirtualWidthProperty); }
            set { SetValue(VirtualWidthProperty, value); }
        }

        private static void VirtualWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Virtualbox;
            control.ArrangeContent();
        }

        public static readonly DependencyProperty VirtualWidthProperty = DependencyProperty.Register("VirtualWidth", typeof(double), typeof(Virtualbox), new PropertyMetadata(1.0, VirtualWidthChanged));
        #endregion

        #region VirtualHeight
        public double VirtualHeight
        {
            get { return (double)GetValue(VirtualHeightProperty); }
            set { SetValue(VirtualHeightProperty, value); }
        }

        private static void VirtualHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Virtualbox;
            control.ArrangeContent();
        }

        public static readonly DependencyProperty VirtualHeightProperty = DependencyProperty.Register("VirtualHeight", typeof(double), typeof(Virtualbox), new PropertyMetadata(1.0, VirtualHeightChanged));
        #endregion

        #region Stretch
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        private static void StretchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Virtualbox;
            control.ArrangeContent();
        }

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(Virtualbox), new PropertyMetadata(Stretch.Uniform, StretchChanged));
        #endregion

        protected override void OnApplyTemplate()
        {
            _content = base.GetTemplateChild("content") as ContentPresenter;

            _transform = _content.RenderTransform as CompositeTransform;

            base.OnApplyTemplate();
        }

        private void ArrangeContent()
        {
            if (double.IsInfinity(this.ActualWidth) || double.IsInfinity(this.ActualHeight))
            {
                return;
            }

            if (this.ActualWidth > 0 && this.ActualHeight > 0)
            {
                var containerSize = new Size(this.ActualWidth, this.ActualHeight);
                var contentSize = new Size(this.VirtualWidth, this.VirtualHeight);

                Size size = GetStretchSize(containerSize, contentSize);

                double offsetX = (this.ActualWidth - size.Width) / 2.0;
                double offsetY = (this.ActualHeight - size.Height) / 2.0;
                double factorX = size.Width / this.VirtualWidth;
                double factorY = size.Height / this.VirtualHeight;

                _transform.TranslateX = offsetX;
                _transform.TranslateY = offsetY;
                _transform.ScaleX = factorX;
                _transform.ScaleY = factorY;
            }
        }

        private Size GetStretchSize(Size containerSize, Size contentSize)
        {
            switch (this.Stretch)
            {
                case Stretch.Fill:
                    return new Size(this.ActualWidth, this.ActualHeight);
                case Stretch.Uniform:
                    return contentSize.ToUniform(containerSize);
                case Stretch.UniformToFill:
                    return contentSize.ToUniformToFill(containerSize);
                default:
                    return new Size(this.VirtualWidth, this.VirtualHeight);
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ArrangeContent();
        }
    }
}
