using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.Media.Casting;

namespace AppStudio.Uwp.Controls
{
    partial class ImageEx
    {
        public event ExceptionRoutedEventHandler ImageFailed;
        public event RoutedEventHandler ImageOpened;

        #region Stretch
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(Stretch), typeof(ImageEx), new PropertyMetadata(Stretch.Uniform));
        #endregion

        #region NineGrid
        public Thickness NineGrid
        {
            get { return (Thickness)GetValue(NineGridProperty); }
            set { SetValue(NineGridProperty, value); }
        }

        private static void NineGridChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ImageEx;
            control.SetNineGrid((Thickness)e.NewValue);
        }

        private void SetNineGrid(Thickness nineGrid)
        {
            if (_isInitialized)
            {
                _image.NineGrid = nineGrid;
                _imageGif.NineGrid = nineGrid;
            }
        }

        public static readonly DependencyProperty NineGridProperty = DependencyProperty.Register("NineGrid", typeof(Thickness), typeof(ImageEx), new PropertyMetadata(new Thickness(), NineGridChanged));
        #endregion

        public CastingSource GetAsCastingSource()
        {
            if (_isInitialized)
            {
                if (_image.Visibility == Visibility.Visible)
                {
                    return _image.GetAsCastingSource();
                }
                else if (_imageGif.Visibility == Visibility.Visible)
                {
                    return _imageGif.GetAsCastingSource();
                }
            }
            return null;
        }
    }
}
