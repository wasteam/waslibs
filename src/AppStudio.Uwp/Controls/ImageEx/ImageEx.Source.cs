using System;
using System.IO;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace AppStudio.Uwp.Controls
{
    partial class ImageEx
    {
        private bool _isHttpSource = false;

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

        #region SetSource
        private void SetSource(object source)
        {
            _isHttpSource = false;
            if (source != null)
            {
                string url = source as String;
                if (url != null)
                {
                    SetSourceString(url);
                }
                else
                {
                    Uri uri = source as Uri;
                    if (uri != null)
                    {
                        SetSourceUri(uri);
                    }
                    else
                    {
                        ImageSource imageSource = source as ImageSource;
                        if (imageSource != null)
                        {
                            SetImage(imageSource);
                        }
                        else
                        {
                            ClearImage();
                            ClearImageGif();
                        }
                    }
                }
            }
            else
            {
                ClearImage();
                ClearImageGif();
            }
        }

        private void SetSourceString(string url)
        {
            Uri uri = null;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                SetSourceUri(uri);
            }
            else if (Uri.IsWellFormedUriString(url, UriKind.Relative))
            {
                if (Uri.TryCreate("ms-appx:///" + url.TrimStart('/'), UriKind.Absolute, out uri))
                {
                    SetSourceUri(uri);
                }
                else
                {
                    ClearImage();
                    ClearImageGif();
                }
            }
            else
            {
                ClearImage();
                ClearImageGif();
            }
        }

        private Uri _currentUri = null;

        private async void SetSourceUri(Uri uri)
        {
            _currentUri = uri;
            try
            {
                if (uri.IsAbsoluteUri)
                {
                    var cachedUri = uri;
                    if (uri.Scheme == "http" || uri.Scheme == "https")
                    {
                        SetProgress();
                        _isHttpSource = true;
                        if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                        {
                            cachedUri = await BitmapCache.GetImageUriAsync(uri, (int)_currentSize.Width, (int)_currentSize.Height);
                            if (cachedUri == null)
                            {
                                ClearProgress();
                                ClearImage();
                                ClearImageGif();
                                return;
                            }
                        }
                    }
                    if (Path.GetExtension(uri.LocalPath).Equals(".gif", StringComparison.OrdinalIgnoreCase))
                    {
                        this.SetImageGif(cachedUri);
                    }
                    else
                    {
                        this.SetImage(new BitmapImage(cachedUri));
                    }
                }
                else
                {
                    ClearImage();
                    ClearImageGif();
                }
            }
            catch
            {
                // Invalid Uri
                ClearImage();
                ClearImageGif();
            }
        }
        #endregion

        private async void RefreshSourceUri(Uri uri)
        {
            try
            {
                if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                {
                    uri = await BitmapCache.GetImageUriAsync(uri, (int)_currentSize.Width, (int)_currentSize.Height);
                }
                if (uri != null)
                {
                    this.SetImage(new BitmapImage(uri));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("RefreshSourceUri. {0}", ex.Message);
            }
        }

        private static int _progressCount = 0;
        private object _progressCountLock = new object();

        private void SetProgress()
        {
            if (this.Progress != null)
            {
                return;
            }

            bool available = false;

            lock (_progressCountLock)
            {
                if (_progressCount < 100)
                {
                    _progressCount++;
                    available = true;
                }
            }

            if (available)
            {
                var progress = new ProgressRing
                {
                    IsActive = true
                };
                progress.SetBinding(ProgressRing.BackgroundProperty, new Binding { Source = this, Path = new PropertyPath("Background") });
                progress.SetBinding(ProgressRing.ForegroundProperty, new Binding { Source = this, Path = new PropertyPath("Foreground") });
                this.Content = progress;
            }
        }

        private void SetImage(ImageSource imageSource)
        {
            ClearProgress();
            ClearImageGif();

            var image = this.Image;
            if (image == null)
            {
                image = new Image();
                image.SetBinding(Image.StretchProperty, new Binding { Source = this, Path = new PropertyPath("Stretch") });
                image.SetBinding(Image.HorizontalAlignmentProperty, new Binding { Source = this, Path = new PropertyPath("HorizontalAlignment") });
                image.SetBinding(Image.VerticalAlignmentProperty, new Binding { Source = this, Path = new PropertyPath("VerticalAlignment") });
                image.SetBinding(Image.NineGridProperty, new Binding { Source = this, Path = new PropertyPath("NineGrid") });
                this.Content = image;
            }
            image.Source = imageSource;
        }

        private void SetImageGif(Uri uri)
        {
            ClearProgress();
            ClearImage();
            ClearImageGif();

            var imageGif = new GifControl();
            imageGif.SetBinding(GifControl.StretchProperty, new Binding { Source = this, Path = new PropertyPath("Stretch") });
            imageGif.SetBinding(GifControl.HorizontalAlignmentProperty, new Binding { Source = this, Path = new PropertyPath("HorizontalAlignment") });
            imageGif.SetBinding(GifControl.VerticalAlignmentProperty, new Binding { Source = this, Path = new PropertyPath("VerticalAlignment") });
            imageGif.SetBinding(GifControl.NineGridProperty, new Binding { Source = this, Path = new PropertyPath("NineGrid") });
            imageGif.SetBinding(GifControl.AutoPlayProperty, new Binding { Source = this, Path = new PropertyPath("AnimateGif") });
            imageGif.Source = uri;
            this.Content = imageGif;
        }

        private void ClearProgress()
        {
            if (this.Progress != null)
            {
                this.Progress.IsActive = false;
                this.Content = null;
                lock (_progressCountLock)
                {
                    _progressCount--;
                }
            }
        }

        private void ClearImage()
        {
            if (this.Image != null)
            {
                this.Image.Source = null;
                this.Content = null;
            }
        }

        private void ClearImageGif()
        {
            if (this.ImageGif != null)
            {
                this.ImageGif.Source = null;
                this.Content = null;
            }
        }
    }
}
