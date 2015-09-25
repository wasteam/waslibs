using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace AppStudio.Uwp.Controls
{
    public sealed class ResponsiveGridView : Control
    {
        public static readonly DependencyProperty DesiredWidthProperty =
            DependencyProperty.Register("DesiredWidth", typeof(double), typeof(ResponsiveGridView), new PropertyMetadata(0D, DesiredWidthChanged));

        public static readonly DependencyProperty ItemClickCommandProperty =
            DependencyProperty.Register("ItemClickCommand", typeof(ICommand), typeof(ResponsiveGridView), new PropertyMetadata(null));

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(ResponsiveGridView), new PropertyMetadata(null));

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(ResponsiveGridView), new PropertyMetadata(null));

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register("ItemHeight", typeof(double), typeof(ResponsiveGridView), new PropertyMetadata(0D));

        public static readonly DependencyProperty OneRowModeEnabledProperty =
            DependencyProperty.Register("OneRowModeEnabled", typeof(bool), typeof(ResponsiveGridView), new PropertyMetadata(false, ((o, e) => { OnOneRowModeEnabledChanged(o, e.NewValue); })));

        private static readonly DependencyProperty VerticalScrollProperty =
            DependencyProperty.Register("VerticalScroll", typeof(ScrollMode), typeof(ResponsiveGridView), new PropertyMetadata(ScrollMode.Auto));

        private static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register("ItemWidth", typeof(double), typeof(ResponsiveGridView), new PropertyMetadata(0D));

        private static void OnOneRowModeEnabledChanged(DependencyObject d, object newValue)
        {
            var self = d as ResponsiveGridView;

            if ((bool)newValue)
            {
                if (self._isInitialized)
                {
                    var b = new Binding()
                    {
                        Source = self,
                        Path = new PropertyPath("ItemHeight")
                    };

                    self.gridView.SetBinding(GridView.MaxHeightProperty, b);
                    self.VerticalScroll = ScrollMode.Disabled;
                }
            }
        }

        private static void DesiredWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as ResponsiveGridView;
            if (self._isInitialized)
            {
                self.RecalculateLayout(self.gridView.ActualWidth);
            }
        }

        private int _columns;
        private bool _isInitialized;

        GridView gridView;

        public ResponsiveGridView()
        {
            this.DefaultStyleKey = typeof(ResponsiveGridView);
        }

        public double DesiredWidth
        {
            get { return (double)GetValue(DesiredWidthProperty); }
            set { SetValue(DesiredWidthProperty, value); }
        }

        public ICommand ItemClickCommand
        {
            get { return (ICommand)GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        public object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public bool OneRowModeEnabled
        {
            get { return (bool)GetValue(OneRowModeEnabledProperty); }
            set { SetValue(OneRowModeEnabledProperty, value); }
        }

        private ScrollMode VerticalScroll
        {
            get { return (ScrollMode)GetValue(VerticalScrollProperty); }
            set { SetValue(VerticalScrollProperty, value); }
        }

        private double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }


        public void RefreshLayout(double desiredWidth)
        {
            this.DesiredWidth = desiredWidth;
            if (gridView != null)
            {
                RecalculateLayout(gridView.ActualWidth);
            }
        }

        private void RecalculateLayout(double containerWidth)
        {
            if (containerWidth == 0 || DesiredWidth == 0)
            {
                return;
            }
            if (_columns == 0)
            {
                _columns = CalculateColumns(containerWidth, DesiredWidth);
            }
            else
            {
                var desiredColumns = CalculateColumns(containerWidth, DesiredWidth);
                if (desiredColumns != _columns)
                {
                    _columns = desiredColumns;
                }
            }
            ItemWidth = (containerWidth / _columns) - 5;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            gridView = GetTemplateChild("gridView") as GridView;
            gridView.SizeChanged += GridView_SizeChanged;
            _isInitialized = true;
            OnOneRowModeEnabledChanged(this, OneRowModeEnabled);
        }
        private static int CalculateColumns(double containerWidth, double itemWidth)
        {
            var columns = (int)(containerWidth / itemWidth);
            if (columns == 0)
            {
                columns = 1;
            }
            return columns;
        }

        private void GridView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            gridView = sender as GridView;

            if (e.PreviousSize.Width != e.NewSize.Width)
            {
                RecalculateLayout(e.NewSize.Width);
            }
        }
    }
}
