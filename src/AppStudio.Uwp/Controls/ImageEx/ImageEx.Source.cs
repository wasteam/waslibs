using System;
using System.IO;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace AppStudio.Uwp.Controls
{
    partial class ImageEx
    {
        #region Source
        public object Source
        {
            get { return (object)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private static void SourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ImageEx;
            control.SetSource(e.NewValue);
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(object), typeof(ImageEx), new PropertyMetadata(null, SourceChanged));
        #endregion

        private Uri _uri;
        private bool _isHttpSource = false;

        private async void SetSource(object source)
        {
            if (_isInitialized)
            {
                _image.Source = null;
                _imageGif.Source = null;
                if (source != null)
                {
                    if (source.GetType() == typeof(string))
                    {
                        string url = source as String;
                        if (Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out _uri))
                        {
                            _isHttpSource = IsHttpUri(_uri);
                            if (_isHttpSource)
                            {
                                _image.Opacity = 0.0;
                                _imageGif.Opacity = 0.0;
                                _progress.IsActive = true;
                            }
                            else
                            {
                                if (!_uri.IsAbsoluteUri)
                                {
                                    _uri = new Uri("ms-appx:///" + url.TrimStart('/'));
                                }
                            }
                            await LoadImageAsync();
                        }
                    }
                    else
                    {
                        _image.Source = source as ImageSource;
                    }
                    _progress.IsActive = false;
                    if (_image.Source != null)
                    {
                        _image.FadeIn();
                    }
                    else
                    {
                        _imageGif.FadeIn();
                    }
                }
            }
        }

        private bool _isLoadingImage = false;

        private async Task LoadImageAsync()
        {
            if (!_isLoadingImage && _uri != null)
            {
                _isLoadingImage = true;
                if (_isHttpSource)
                {
                    if (Path.GetExtension(_uri.LocalPath).Equals(".gif", StringComparison.OrdinalIgnoreCase))
                    {
                        _imageGif.Source = await BitmapCache.LoadGifFromCacheAsync(_uri);
                        _image.Visibility = Visibility.Collapsed;
                        _imageGif.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        _image.Source = await BitmapCache.LoadFromCacheAsync(_uri, (int)_currentSize.Width, (int)_currentSize.Height);
                        _image.Visibility = Visibility.Visible;
                        _imageGif.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    if (Path.GetExtension(_uri.LocalPath).Equals(".gif", StringComparison.OrdinalIgnoreCase))
                    {
                        _imageGif.Source = _uri;
                        _image.Visibility = Visibility.Collapsed;
                        _imageGif.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        _image.Source = new BitmapImage(_uri);
                        _image.Visibility = Visibility.Visible;
                        _imageGif.Visibility = Visibility.Collapsed;
                    }
                }
                _isLoadingImage = false;
            }
        }

        private static bool IsHttpUri(Uri uri)
        {
            return uri.IsAbsoluteUri && (uri.Scheme == "http" || uri.Scheme == "https");
        }
    }
}
