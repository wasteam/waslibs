using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace AppStudio.Uwp.Controls
{
    public class CarouselSlot : ContentControl
    {
        private Storyboard _storyboard = null;

        internal CarouselSlot()
        {
            this.Tapped += OnTapped;
        }

        internal double X1 { get; set; }

        #region ItemClickCommand
        internal ICommand ItemClickCommand
        {
            get { return (ICommand)GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        internal static readonly DependencyProperty ItemClickCommandProperty = DependencyProperty.Register("ItemClickCommand", typeof(ICommand), typeof(CarouselSlot), new PropertyMetadata(null));
        #endregion

        internal void MoveX(double x, double duration = 0)
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

        private void OnTapped(object sender, TappedRoutedEventArgs e)
        {
            object parameter = null;
            if (sender != null && sender is CarouselSlot)
            {
                CarouselSlot carouselSlot = sender as CarouselSlot;
                parameter = carouselSlot.Content;
            }

            if (ItemClickCommand != null)
            {
                if (ItemClickCommand.CanExecute(parameter))
                {
                    ItemClickCommand.Execute(parameter);
                }
            }
        }
    }
}
