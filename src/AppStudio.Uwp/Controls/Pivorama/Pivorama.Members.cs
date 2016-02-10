using Windows.UI.Xaml;

namespace AppStudio.Uwp.Controls
{
    partial class Pivorama
    {
        #region ItemsSource
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(Pivorama), new PropertyMetadata(null));
        #endregion

        #region ContentTemplate
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(Pivorama), new PropertyMetadata(null));
        #endregion

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


        #region FitToScreen
        public bool FitToScreen
        {
            get { return (bool)GetValue(FitToScreenProperty); }
            set { SetValue(FitToScreenProperty, value); }
        }

        private static void FitToScreenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Pivorama;
            control.SetFitToScreen((bool)e.NewValue);
        }

        private void SetFitToScreen(bool fitToScreen)
        {
            if (_isInitialized)
            {
                if (fitToScreen)
                {
                    this.ItemWidthEx = this.ActualWidth;
                }
                else
                {
                    this.ItemWidthEx = this.ItemWidth;
                }
                RefreshLayout();
            }
        }

        public static readonly DependencyProperty FitToScreenProperty = DependencyProperty.Register("FitToScreen", typeof(bool), typeof(Pivorama), new PropertyMetadata(false, FitToScreenChanged));
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

        private void SetItemWidth(double newValue, double oldValue)
        {
            if (_isInitialized)
            {
                if (this.FitToScreen)
                {
                    this.ItemWidthEx = this.ActualWidth;
                }
                else
                {
                    this.ItemWidthEx = newValue;
                }
            }
        }

        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(double), typeof(Pivorama), new PropertyMetadata(440.0, ItemWidthChanged));
        #endregion

        #region ItemWidthEx
        public double ItemWidthEx
        {
            get { return (double)GetValue(ItemWidthExProperty); }
            set { SetValue(ItemWidthExProperty, value); }
        }

        private static void ItemWidthExChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Pivorama;
            control.SetItemWidthEx((double)e.NewValue, (double)e.OldValue);
        }

        private void SetItemWidthEx(double newValue, double oldValue)
        {
            if (_isInitialized)
            {
                int oldIndex = (int)(-Position / oldValue);
                Position = -oldIndex * newValue;
                this.Index = (int)(-Position / this.ItemWidthEx);
            }
        }

        public static readonly DependencyProperty ItemWidthExProperty = DependencyProperty.Register("ItemWidthEx", typeof(double), typeof(Pivorama), new PropertyMetadata(440.0, ItemWidthExChanged));
        #endregion


        #region Index
        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        private static void IndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Pivorama;
            control.SetIndex((int)e.NewValue);
        }

        private void SetIndex(int newValue)
        {
            if (_isInitialized)
            {
                Position = -newValue * this.ItemWidthEx;
            }
        }

        public static readonly DependencyProperty IndexProperty = DependencyProperty.Register("Index", typeof(int), typeof(Pivorama), new PropertyMetadata(0, IndexChanged));
        #endregion
    }
}
