using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Foundation;

namespace AppStudio.Uwp.Controls
{
    public sealed partial class SliderView : Control
    {
        private Panel _frame = null;
        private SliderViewPanel _panel = null;
        private Button _prevArrow = null;
        private Button _nextArrow = null;
        private RectangleGeometry _clip;

        public SliderView()
        {
            this.DefaultStyleKey = typeof(SliderView);
            this.SizeChanged += OnSizeChanged;
        }

        protected override void OnApplyTemplate()
        {
            _frame = base.GetTemplateChild("frame") as Panel;
            _panel = base.GetTemplateChild("panel") as SliderViewPanel;
            _prevArrow = base.GetTemplateChild("prevArrow") as Button;
            _nextArrow = base.GetTemplateChild("nextArrow") as Button;
            _clip = base.GetTemplateChild("clip") as RectangleGeometry;

            _frame.ManipulationDelta += OnManipulationDelta;
            _frame.ManipulationCompleted += OnManipulationCompleted;
            _frame.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateInertia | ManipulationModes.System;

            _prevArrow.Click += OnPrevArrowClick;
            _nextArrow.Click += OnNextArrowClick;

            base.OnApplyTemplate();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            base.MeasureOverride(availableSize);
            return _panel.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return base.ArrangeOverride(_panel.DesiredSize);
        }

        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            _prevArrow.FadeIn(500.0);
            _nextArrow.FadeIn(500.0);
            base.OnPointerEntered(e);
        }
        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            _prevArrow.FadeOut(500.0);
            _nextArrow.FadeOut(500.0);
            base.OnPointerExited(e);
        }

        private void OnPrevArrowClick(object sender, RoutedEventArgs e)
        {
            if (!_isBusy && this.Position < 0)
            {
                _panel.TranslateDeltaX(1);
                this.AnimatePrev();
            }
        }
        private void OnNextArrowClick(object sender, RoutedEventArgs e)
        {
            if (!_isBusy && this.Position > -(this.ItemWidth * _panel.ItemsCount - this.ActualWidth))
            {
                _panel.TranslateDeltaX(-1);
                this.AnimateNext();
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _prevArrow.Height = e.NewSize.Height;
            _nextArrow.Height = e.NewSize.Height;
            _clip.Rect = new Rect(new Point(), e.NewSize);
        }
    }
}
