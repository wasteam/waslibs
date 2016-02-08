using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    partial class Pivorama
    {
        #region HeaderTemplate
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(Pivorama), new PropertyMetadata(null));
        #endregion

        #region TabTemplate
        public DataTemplate TabTemplate
        {
            get { return (DataTemplate)GetValue(TabTemplateProperty); }
            set { SetValue(TabTemplateProperty, value); }
        }

        public static readonly DependencyProperty TabTemplateProperty = DependencyProperty.Register("TabTemplate", typeof(DataTemplate), typeof(Pivorama), new PropertyMetadata(null));
        #endregion

        #region ContentTemplate
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(Pivorama), new PropertyMetadata(null));
        #endregion

        #region ItemWidth
        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        private static void ItemWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Pivorama;
            control.SetItemWidth((double)e.NewValue, (double)e.OldValue);
        }

        private void SetItemWidth(double newWidth, double oldWidth)
        {
            if (_isInitialized)
            {
                int oldIndex = (int)(Position / oldWidth);

                foreach (Control control in _headerItems.Children)
                {
                    control.Width = newWidth;
                }
                foreach (Control control in _container.Children)
                {
                    control.Width = newWidth;
                }

                Position = oldIndex * newWidth;
                this.ArrangeTabs();
                this.ArrangeItems();
            }
        }

        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(double), typeof(Pivorama), new PropertyMetadata(440.0, ItemWidthChanged));
        #endregion

        private int Index
        {
            get { return (int)(Position / this.ItemWidth); }
        }

        private double PanelWidth
        {
            get { return _items.Count * this.ItemWidth; }
        }

        private bool IsTabVisible
        {
            get { return this.ActualWidth < this.ItemWidth * 1.5; }
        }
    }
}
