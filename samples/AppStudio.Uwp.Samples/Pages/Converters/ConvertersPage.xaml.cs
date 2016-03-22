using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "Utilities", Name = "Converters", Order = 40)]
    public sealed partial class ConvertersPage : SamplePage
    {
        public ConvertersPage()
        {
            this.StringVisibilityValue = this.GetResourceString("ConvertersStringVisibilityConverterDefault");
            this.InitializeComponent();
            this.DataContext = this;
        }

        public override string Caption
        {
            get { return "Converters"; }
        }

        #region IsImageVisible        
        public bool IsImageVisible
        {
            get { return (bool)GetValue(IsImageVisibleProperty); }
            set { SetValue(IsImageVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsImageVisibleProperty = DependencyProperty.Register("IsImageVisible", typeof(bool), typeof(ConvertersPage), new PropertyMetadata(true));
        #endregion

        #region StringVisibilityValue        
        public string StringVisibilityValue
        {
            get { return (string)GetValue(StringVisibilityValueProperty); }
            set { SetValue(StringVisibilityValueProperty, value); }
        }

        public static readonly DependencyProperty StringVisibilityValueProperty = DependencyProperty.Register("StringVisibilityValue", typeof(string), typeof(ConvertersPage), new PropertyMetadata(string.Empty));
        #endregion
    }
}
