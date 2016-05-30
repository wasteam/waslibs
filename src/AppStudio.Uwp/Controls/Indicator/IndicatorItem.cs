using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    public sealed class IndicatorItem : ListViewItem
    {
        public IndicatorItem()
        {
            this.DefaultStyleKey = typeof(IndicatorItem);
        }

        #region PressedBackground
        public Brush PressedBackground
        {
            get { return (Brush)GetValue(PressedBackgroundProperty); }
            set { SetValue(PressedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty PressedBackgroundProperty = DependencyProperty.Register("PressedBackground", typeof(Brush), typeof(IndicatorItem), new PropertyMetadata(null));
        #endregion

        #region SelectedBackground
        public Brush SelectedBackground
        {
            get { return (Brush)GetValue(SelectedBackgroundProperty); }
            set { SetValue(SelectedBackgroundProperty, value); }
        }

        private static void SelectedBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as IndicatorItem;
            if (control.IsSelected)
            {
                VisualStateManager.GoToState(control, "Selected", false);
            }
        }

        public static readonly DependencyProperty SelectedBackgroundProperty = DependencyProperty.Register("SelectedBackground", typeof(Brush), typeof(IndicatorItem), new PropertyMetadata(null, SelectedBackgroundChanged));
        #endregion
    }
}
