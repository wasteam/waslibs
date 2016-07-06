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
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
            this.SizeChanged += OnSizeChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CreateFadeTimer();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            DisposeFadeTimer();
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

            _frame.PointerMoved += OnPointerMoved;
            _left.Click += OnPrevArrowClick;
            _right.Click += OnNextArrowClick;
            _left.PointerEntered += OnArrowPointerEntered;
            _left.PointerExited += OnArrowPointerExited;
            _right.PointerEntered += OnArrowPointerEntered;
            _right.PointerExited += OnArrowPointerExited;

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

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Position = Math.Min(0, Math.Max(this.Position, -(this.ItemWidth * _panel.ItemsCount - this.ActualWidth)));
            this.Index = (int)(-this.Position / this.ItemWidth);

            _clip.Rect = new Rect(new Point(), e.NewSize);
        }
    }
}
