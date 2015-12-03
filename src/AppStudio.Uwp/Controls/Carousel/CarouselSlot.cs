using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace AppStudio.Uwp.Controls
{
    class CarouselSlot : ContentControl
    {
        private Storyboard _storyboard = null;

        public double X1 { get; set; }

        public void MoveX(double x, double duration = 0)
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
            X1 = x;
        }
    }
}
