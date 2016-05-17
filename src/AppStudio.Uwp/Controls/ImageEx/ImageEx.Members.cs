using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.Media.Casting;

namespace AppStudio.Uwp.Controls
{
    partial class ImageEx
    {
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

        public static readonly DependencyProperty NineGridProperty = DependencyProperty.Register("NineGrid", typeof(Thickness), typeof(ImageEx), new PropertyMetadata(null));
        #endregion

        #region AnimateGif
        public bool AnimateGif
        {
            get { return (bool)GetValue(AnimateGifProperty); }
            set { SetValue(AnimateGifProperty, value); }
        }

        public static readonly DependencyProperty AnimateGifProperty = DependencyProperty.Register("AnimateGif", typeof(bool), typeof(ImageEx), new PropertyMetadata(false));
        #endregion

        public ProgressRing Progress
        {
            get { return this.Content as ProgressRing; }
        }

        public Image Image
        {
            get { return this.Content as Image; }
        }

        public GifControl ImageGif
        {
            get { return this.Content as GifControl; }
        }

        public CastingSource GetAsCastingSource()
        {
            if (this.Image != null)
            {
                return this.Image.GetAsCastingSource();
            }
            return null;
        }
    }
}
