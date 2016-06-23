using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace AppStudio.Uwp.Labs
{
    partial class ShapeImage
    {
        #region Source
        public object Source
        {
            get { return (object)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private static void SourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ShapeImage;
            control.SetSource(e.NewValue);
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(object), typeof(ShapeImage), new PropertyMetadata(null, SourceChanged));
        #endregion

        #region SetSource
        private void SetSource(object source)
        {
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
                        }
                    }
                }
            }
            else
            {
                ClearImage();
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
                }
            }
            else
            {
                ClearImage();
            }
        }

        private async void SetSourceUri(Uri uri)
        {
            if (uri.IsAbsoluteUri)
            {
                var cachedUri = uri;
                if (uri.Scheme == "http" || uri.Scheme == "https")
                {
                    if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                    {
                        cachedUri = await AppStudio.Uwp.Controls.BitmapCache.GetImageUriAsync(uri, AppStudio.Uwp.Controls.BitmapCache.MAXRESOLUTION, AppStudio.Uwp.Controls.BitmapCache.MAXRESOLUTION);
                    }
                }
                this.SetImage(new BitmapImage(cachedUri));
            }
            else
            {
                ClearImage();
            }
        }
        #endregion

        private void SetImage(ImageSource imageSource)
        {
            var imageBrush = new ImageBrush { ImageSource = imageSource };
            this.Fill = imageBrush;
            this.RefreshFill();
        }

        private void ClearImage()
        {
            this.Fill = null;
        }
    }
}
