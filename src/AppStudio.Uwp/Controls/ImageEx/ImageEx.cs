using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace AppStudio.Uwp.Controls
{
    public sealed partial class ImageEx : Control
    {
        private Image _image = null;
        private ProgressRing _progress = null;

        private bool _isInitialized = false;

        public ImageEx()
        {
            this.DefaultStyleKey = typeof(ImageEx);
        }

        #region Stretch
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(ImageEx), new PropertyMetadata(Stretch.Uniform));
        #endregion

        protected override void OnApplyTemplate()
        {
            _image = base.GetTemplateChild("image") as Image;
            _progress = base.GetTemplateChild("progress") as ProgressRing;

            _isInitialized = true;

            this.SetSource(this.Source);

            this.SizeChanged += OnSizeChanged;

            base.OnApplyTemplate();
        }

        private Size _currentSize = new Size(240, 240);

        protected override Size MeasureOverride(Size availableSize)
        {
            _progress.Width = _progress.Height = Math.Min(1024, Math.Min(availableSize.Width, availableSize.Height)) / 8.0;
            var size = base.MeasureOverride(availableSize);
            if (double.IsInfinity(availableSize.Width) || double.IsInfinity(availableSize.Height))
            {
                return size;
            }
            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return base.ArrangeOverride(finalSize);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var imageSize = _image.DesiredSize;
            if (imageSize.Width > 0 && imageSize.Height > 0)
            {
                if (_isHttpSource && BitmapCache.GetSizeLevel(_currentSize) != BitmapCache.GetSizeLevel(imageSize))
                {
                    _currentSize = imageSize;
                    RefreshImage();
                }
            }
        }

        private async void RefreshImage()
        {
            await LoadImageAsync();
        }
    }
}
