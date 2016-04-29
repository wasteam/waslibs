using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;
using Windows.Foundation.Metadata;

namespace AppStudio.Uwp.Labs
{
    public sealed class ResponsiveGridView : ListViewBase
    {
        private ResponsiveGridViewPanel _panel = null;

        public ResponsiveGridView()
        {
            this.DefaultStyleKey = typeof(ResponsiveGridView);
            this.LayoutUpdated += OnLayoutUpdated;
            this.IsItemClickEnabled = true;
            this.ItemClick += OnItemClick;
        }

        #region DesiredWidth
        public double DesiredWidth
        {
            get { return (double)GetValue(DesiredWidthProperty); }
            set { SetValue(DesiredWidthProperty, value); }
        }

        public static readonly DependencyProperty DesiredWidthProperty = DependencyProperty.Register("DesiredWidth", typeof(double), typeof(ResponsiveGridView), new PropertyMetadata(0.0));
        #endregion

        #region DesiredHeight
        public double DesiredHeight
        {
            get { return (double)GetValue(DesiredHeightProperty); }
            set { SetValue(DesiredHeightProperty, value); }
        }

        public static readonly DependencyProperty DesiredHeightProperty = DependencyProperty.Register("DesiredHeight", typeof(double), typeof(ResponsiveGridView), new PropertyMetadata(0.0));
        #endregion

        #region ItemMargin
        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        public static readonly DependencyProperty ItemMarginProperty = DependencyProperty.Register("ItemMargin", typeof(Thickness), typeof(ResponsiveGridView), new PropertyMetadata(new Thickness(2)));
        #endregion

        #region ItemPadding
        public Thickness ItemPadding
        {
            get { return (Thickness)GetValue(ItemPaddingProperty); }
            set { SetValue(ItemPaddingProperty, value); }
        }

        public static readonly DependencyProperty ItemPaddingProperty = DependencyProperty.Register("ItemPadding", typeof(Thickness), typeof(ResponsiveGridView), new PropertyMetadata(new Thickness(2)));
        #endregion

        #region OneRowModeEnabled
        public bool OneRowModeEnabled
        {
            get { return (bool)GetValue(OneRowModeEnabledProperty); }
            set { SetValue(OneRowModeEnabledProperty, value); }
        }

        public static readonly DependencyProperty OneRowModeEnabledProperty = DependencyProperty.Register("OneRowModeEnabled", typeof(bool), typeof(ResponsiveGridView), new PropertyMetadata(false));
        #endregion

        #region ItemClickCommand
        public ICommand ItemClickCommand
        {
            get { return (ICommand)GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        public static readonly DependencyProperty ItemClickCommandProperty = DependencyProperty.Register("ItemClickCommand", typeof(ICommand), typeof(ResponsiveGridView), new PropertyMetadata(null));
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

        private void OnLayoutUpdated(object sender, object e)
        {
            if (_panel == null)
            {
                _panel = base.ItemsPanelRoot as ResponsiveGridViewPanel;
                if (_panel != null)
                {
                    _panel.IsReady = true;
                    _panel.SetBinding(ResponsiveGridViewPanel.DesiredItemWidthProperty, new Binding { Source = this, Path = new PropertyPath("DesiredWidth") });
                    _panel.SetBinding(ResponsiveGridViewPanel.DesiredItemHeightProperty, new Binding { Source = this, Path = new PropertyPath("DesiredHeight") });
                    _panel.SetBinding(ResponsiveGridViewPanel.OneRowModeEnabledProperty, new Binding { Source = this, Path = new PropertyPath("OneRowModeEnabled") });
                    _panel.InvalidateMeasure();
                }
            }
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (ItemClickCommand != null)
            {
                if (ItemClickCommand.CanExecute(e.ClickedItem))
                {
                    ItemClickCommand.Execute(e.ClickedItem);
                }
            }
        }

        // Obsolete
        #region ItemWidth
        [Deprecated("ItemWidth property will be removed in future versions.", DeprecationType.Deprecate, 65536)]
        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(double), typeof(ResponsiveGridView), new PropertyMetadata(0.0));
        #endregion

        #region ItemHeight
        [Deprecated("ItemHeight property will be removed in future versions.", DeprecationType.Deprecate, 65536)]
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        private static void ItemHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ResponsiveGridView;
            control.DesiredHeight = (double)e.NewValue;
        }

        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(double), typeof(ResponsiveGridView), new PropertyMetadata(0.0, ItemHeightChanged));
        #endregion
    }
}
