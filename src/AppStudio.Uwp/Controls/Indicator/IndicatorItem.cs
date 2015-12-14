using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    public sealed class IndicatorItem : ContentControl
    {
        internal event EventHandler IsSelectedChanged;

        public IndicatorItem()
        {
            this.DefaultStyleKey = typeof(IndicatorItem);
        }

        #region IsSelected
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        private static void SelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as IndicatorItem;
            control.SelectionChanged((bool)e.NewValue);
        }

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(IndicatorItem), new PropertyMetadata(null, SelectionChanged));
        #endregion

        protected override void OnApplyTemplate()
        {
            SetCurrentStateManager("Selected", "Normal");

            base.OnApplyTemplate();
        }

        private void SelectionChanged(bool isSelected)
        {
            SetCurrentStateManager("Selected", "Normal");
            if (IsSelectedChanged != null)
            {
                IsSelectedChanged(this, EventArgs.Empty);
            }
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            SetCurrentStateManager("SelectedPointerOver", "PointerOver");
            base.OnPointerEntered(e);
        }

        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            SetCurrentStateManager("Selected", "Normal");
            base.OnPointerExited(e);
        }

        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            SetCurrentStateManager("SelectedPressed", "Pressed");
            base.OnPointerPressed(e);
        }

        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            SetCurrentStateManager("Selected", "Normal");
            base.OnPointerReleased(e);
        }

        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            IsSelected = true;
            VisualStateManager.GoToState(this, "Selected", false);
            base.OnTapped(e);
        }

        private void SetCurrentStateManager(string selected, string unselected)
        {
            if (this.IsSelected)
            {
                VisualStateManager.GoToState(this, selected, false);
            }
            else
            {
                VisualStateManager.GoToState(this, unselected, false);
            }
        }
    }
}
