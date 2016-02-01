using Windows.UI.Xaml;

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
    }
}
