using System;

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

        private Grid _arrows = null;
        private Button _left = null;
        private Button _right = null;

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

            _arrows = base.GetTemplateChild("arrows") as Grid;
            _left = base.GetTemplateChild("left") as Button;
            _right = base.GetTemplateChild("right") as Button;

            _clip = base.GetTemplateChild("clip") as RectangleGeometry;

            _frame.ManipulationDelta += OnManipulationDelta;
            _frame.ManipulationCompleted += OnManipulationCompleted;
            _frame.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.System;

            this.PointerEntered += OnPointerEntered;
            this.PointerExited += OnPointerExited;
            _left.Click += OnPrevArrowClick;
            _right.Click += OnNextArrowClick;

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

        private void OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SetArrowsOpacity(this.Position);
            _arrows.FadeIn(500);
        }

        private void OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            _arrows.FadeOut(500);
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
            this.Position = Math.Min(0, Math.Max(this.Position, -(this.ItemWidth * _panel.ItemsCount - this.ActualWidth)));
            this.Index = (int)(-this.Position / this.ItemWidth);
            _clip.Rect = new Rect(new Point(), e.NewSize);
        }
    }
}
