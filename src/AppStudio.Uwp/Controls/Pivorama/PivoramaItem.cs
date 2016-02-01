using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace AppStudio.Uwp.Controls
{
    public sealed class PivoramaItem : ContentControl
    {
        private Storyboard _storyboard = null;
        private object _storyboardLock = new object();

        public PivoramaItem()
        {
            this.DefaultStyleKey = typeof(PivoramaItem);
        }

        internal double X { get; set; }

        internal void MoveX(double x)
        {
            if (x != this.X)
            {
                if (_storyboard != null)
                {
                    lock (_storyboardLock)
                    {
                        _storyboard.Pause();
                        _storyboard = null;
                    }
                }
                this.TranslateX(x);
                this.X = x;
            }
        }

        internal void MoveX(double x, double duration)
        {
            lock (_storyboardLock)
            {
                if (_storyboard != null)
                {
                    _storyboard.Pause();
                    _storyboard = null;
                }
                if (duration > 0)
                {
                    _storyboard = this.AnimateX(x, duration);
                }
                else
                {
                    this.TranslateX(x);
                }
                this.X = x;
            }
        }
    }
}
