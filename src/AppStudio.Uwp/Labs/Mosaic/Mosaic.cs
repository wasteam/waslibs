using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Labs
{
    public partial class Mosaic : Control
    {
        public Mosaic()
        {
            this.DefaultStyleKey = typeof(Mosaic);
            this.VerticalAlignment = VerticalAlignment.Top;
        }

        #region SeedWidth
        public double SeedWidth
        {
            get { return (double)GetValue(SeedWidthProperty); }
            set { SetValue(SeedWidthProperty, value); }
        }

        public static readonly DependencyProperty SeedWidthProperty = DependencyProperty.Register("SeedWidth", typeof(double), typeof(Mosaic), new PropertyMetadata(400.0));
        #endregion

        #region ItemsSource
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(Mosaic), new PropertyMetadata(null));
        #endregion

        #region ItemTemplate
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(Mosaic), new PropertyMetadata(null));
        #endregion
    }
}
