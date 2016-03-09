using System;
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
                    _image.FadeIn();
                }
            }
        }

        private bool _isLoadingImage = false;

        private async Task LoadImageAsync()
        {
            if (!_isLoadingImage)
            {
                _isLoadingImage = true;
                if (_isHttpSource)
                {
                    _image.Source = await BitmapCache.LoadFromCacheAsync(_uri, (int)_currentSize.Width, (int)_currentSize.Height);
                }
                else
                {
                    _image.Source = new BitmapImage(_uri);
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
