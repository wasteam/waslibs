using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace AppStudio.Uwp.Controls
{
    public sealed partial class ImageEx : Control
    {
        private Image _image = null;
        private GifControl _imageGif = null;
        private ProgressRing _progress = null;

        private bool _isInitialized = false;

        public ImageEx()
        {
            this.DefaultStyleKey = typeof(ImageEx);
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        protected override void OnApplyTemplate()
        {
            _image = base.GetTemplateChild("image") as Image;
            _imageGif = base.GetTemplateChild("imageGif") as GifControl;
            _progress = base.GetTemplateChild("progress") as ProgressRing;

            _isInitialized = true;

            _image.ImageOpened += OnImageOpened;
            _image.ImageFailed += OnImageFailed;
            _imageGif.ImageOpened += OnImageOpened;
            _imageGif.ImageFailed += OnImageFailed;

            this.SetSource(this.Source);
            this.SetNineGrid(this.NineGrid);

            this.SizeChanged += OnSizeChanged;

            base.OnApplyTemplate();
        }

        private void OnImageOpened(object sender, RoutedEventArgs e)
        {
            if (this.ImageOpened != null)
            {
                this.ImageOpened(sender, e);
            }
        }

        private void OnImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (this.ImageFailed != null)
            {
                this.ImageFailed(sender, e);
            }
        }

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

        private Size _currentSize = new Size(960, 960);

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var imageSize = _image.Source != null ? _image.DesiredSize : _imageGif.DesiredSize;
            if (imageSize.Width > 0 && imageSize.Height > 0)
            {
                if (_isHttpSource && BitmapCache.GetSizeLevel(_currentSize) != BitmapCache.GetSizeLevel(imageSize))
                {
                    _currentSize = imageSize;
                    RefreshImage();
                }
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_image.Source == null && _imageGif.Source == null)
            {
                RefreshImage();
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _image.Source = null;
            _imageGif.Source = null;
        }

        private async void RefreshImage()
        {
            await LoadImageAsync();
        }
    }
}
