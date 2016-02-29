using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace AppStudio.Uwp.Controls
{
    public class VariableSizedGrid : ListViewBase
    {
        private ScrollViewer _scrollViewer = null;
        private VariableSizedGridPanel _panel = null;

        private bool _isInitialized = false;

        public VariableSizedGrid()
        {
            this.DefaultStyleKey = typeof(VariableSizedGrid);
            this.LayoutUpdated += OnLayoutUpdated;
        }

        #region ItemMargin
        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register("ItemMargin", typeof(Thickness), typeof(VariableSizedGrid), new PropertyMetadata(new Thickness(2)));
        #endregion

        #region ItemPadding
        public Thickness ItemPadding
        {
            get { return (Thickness)GetValue(ItemPaddingProperty); }
            set { SetValue(ItemPaddingProperty, value); }
        }

        public static readonly DependencyProperty ItemPaddingProperty = DependencyProperty.Register("ItemPadding", typeof(Thickness), typeof(VariableSizedGrid), new PropertyMetadata(new Thickness(2)));
        #endregion

        #region MaximumRowsOrColumns
        public int MaximumRowsOrColumns
        {
            get { return (int)GetValue(MaximumRowsOrColumnsProperty); }
            set { SetValue(MaximumRowsOrColumnsProperty, value); }
        }

        private static void MaximumRowsOrColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as VariableSizedGrid;
            control.InvalidateMeasure();
        }

        public static readonly DependencyProperty MaximumRowsOrColumnsProperty = DependencyProperty.Register("MaximumRowsOrColumns", typeof(int), typeof(VariableSizedGrid), new PropertyMetadata(4, MaximumRowsOrColumnsChanged));
        #endregion

        #region AspectRatio
        public double AspectRatio
        {
            get { return (double)GetValue(AspectRatioProperty); }
            set { SetValue(AspectRatioProperty, value); }
        }

        private static void AspectRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as VariableSizedGrid;
            control.InvalidateMeasure();
        }

        public static readonly DependencyProperty AspectRatioProperty = DependencyProperty.Register("AspectRatio", typeof(double), typeof(VariableSizedGrid), new PropertyMetadata(1.0, AspectRatioChanged));
        #endregion

        #region Orientation
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        private static void OrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as VariableSizedGrid;
            control.SetOrientation((Orientation)e.NewValue);
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(VariableSizedGrid), new PropertyMetadata(Orientation.Horizontal, OrientationChanged));
        #endregion

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ListViewItem();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            var container = element as ListViewItem;
            container.Margin = this.ItemMargin;
            container.Padding = this.ItemPadding;
            base.PrepareContainerForItemOverride(element, item);
        }

        protected override void OnApplyTemplate()
        {
            _scrollViewer = base.GetTemplateChild("scrollViewer") as ScrollViewer;

            _isInitialized = true;

            SetOrientation(this.Orientation);

            base.OnApplyTemplate();
        }

        private void SetOrientation(Orientation orientation)
        {
            if (_isInitialized)
            {
                if (orientation == Orientation.Horizontal)
                {
                    _scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                    _scrollViewer.HorizontalScrollMode = ScrollMode.Disabled;
                    _scrollViewer.VerticalScrollBarVisibility = (ScrollBarVisibility)this.GetValue(ScrollViewer.VerticalScrollBarVisibilityProperty);
                    _scrollViewer.VerticalScrollMode = ScrollMode.Auto;
                }
                else
                {
                    _scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                    _scrollViewer.VerticalScrollMode = ScrollMode.Disabled;
                    if ((ScrollBarVisibility)this.GetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty) == ScrollBarVisibility.Disabled)
                    {
                        _scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                    }
                    else
                    {
                        _scrollViewer.HorizontalScrollBarVisibility = (ScrollBarVisibility)this.GetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty);
                    }
                    _scrollViewer.HorizontalScrollMode = ScrollMode.Auto;
                }
            }
        }

        private void OnLayoutUpdated(object sender, object e)
        {
            if (_panel == null)
            {
                _panel = base.ItemsPanelRoot as VariableSizedGridPanel;
                if (_panel != null)
                {
                    _panel.IsReady = true;
                    _panel.SetBinding(VariableSizedGridPanel.OrientationProperty, new Binding { Source = this, Path = new PropertyPath("Orientation") });
                    _panel.SetBinding(VariableSizedGridPanel.AspectRatioProperty, new Binding { Source = this, Path = new PropertyPath("AspectRatio") });
                    _panel.SetBinding(VariableSizedGridPanel.MaximumRowsOrColumnsProperty, new Binding { Source = this, Path = new PropertyPath("MaximumRowsOrColumns") });
                }
            }
        }
    }
}
