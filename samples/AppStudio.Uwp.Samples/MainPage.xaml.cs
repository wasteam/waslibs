using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Samples
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.TopContentTemplate = this.Resources["WideTopTemplate"] as DataTemplate;
            this.DataContext = this;
            this.SizeChanged += OnSizeChanged;
        }

        public string Caption
        {
            get { return "WasLibs Samples"; }
        }

        public bool HideCommandBar
        {
            get { return true; }
        }

        public DataTemplate HeaderTemplate
        {
            get { return App.Current.Resources["HomeHeaderTemplate"] as DataTemplate; }
        }

        #region TopContentTemplate
        public DataTemplate TopContentTemplate
        {
            get { return (DataTemplate)GetValue(TopContentTemplateProperty); }
            set { SetValue(TopContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty TopContentTemplateProperty = DependencyProperty.Register("TopContentTemplate", typeof(DataTemplate), typeof(MainPage), new PropertyMetadata(null));
        #endregion

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (AppShell.Current.Shell.DisplayMode == SplitViewDisplayMode.CompactOverlay)
            {
                this.TopContentTemplate = this.Resources["WideTopTemplate"] as DataTemplate;
            }
            else
            {
                this.TopContentTemplate = this.Resources["TinyTopTemplate"] as DataTemplate;
            }
        }
    }
}
