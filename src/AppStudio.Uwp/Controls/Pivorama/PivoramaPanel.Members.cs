using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace AppStudio.Uwp.Controls
{
    partial class PivoramaPanel
    {
        public event SelectionChangedEventHandler SelectionChanged;

        #region ItemTemplate
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        private static void ItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PivoramaPanel;
            control.SetItemTemplate(e.NewValue as DataTemplate);
        }

        private void SetItemTemplate(DataTemplate dataTemplate)
        {
            foreach (ContentControl control in this.Children)
            {
                control.ContentTemplate = dataTemplate;
            }
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(PivoramaPanel), new PropertyMetadata(null, ItemTemplateChanged));
        #endregion

        #region ItemWidth
        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        private static void ItemWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PivoramaPanel;
            control.InvalidateMeasure();
        }

        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(double), typeof(PivoramaPanel), new PropertyMetadata(440.0, ItemWidthChanged));
        #endregion

        private void OnItemTapped(object sender, TappedRoutedEventArgs e)
        {
            if (SelectionChanged != null)
            {
                var contentControl = sender as ContentControl;
                SelectionChanged(this, new SelectionChangedEventArgs(new object[] { }, new object[] { contentControl.Content }));
            }
        }
    }
}
