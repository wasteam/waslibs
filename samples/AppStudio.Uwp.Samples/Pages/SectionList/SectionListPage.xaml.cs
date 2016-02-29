using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "Layout", Name = "SectionList")]
    public sealed partial class SectionListPage : SamplePage
    {
        public SectionListPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public override string Caption
        {
            get { return "VariableSizedGrid Control"; }
        }

        #region Items
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<object>), typeof(SectionListPage), new PropertyMetadata(null));
        #endregion

        #region ItemTemplate
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(SectionListPage), new PropertyMetadata(null));
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.Items = new ObservableCollection<object>(new BingDataSource().GetItems());
            //this.ItemTemplate = Resources["BingTemplate"] as DataTemplate;
        }

        protected override void OnSettings()
        {
            AppShell.Current.Shell.ShowRightPane(new VariableSizedGridSettings() { DataContext = control });
        }
    }
}
