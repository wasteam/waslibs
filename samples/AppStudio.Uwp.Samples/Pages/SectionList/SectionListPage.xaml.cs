using System.Collections.ObjectModel;
using System.Linq;
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
            get { return "SectionList Control"; }
        }

        #region TabletPCItems
        public ObservableCollection<object> TabletPCItems
        {
            get { return (ObservableCollection<object>)GetValue(TabletPCItemsProperty); }
            set { SetValue(TabletPCItemsProperty, value); }
        }
        public static readonly DependencyProperty TabletPCItemsProperty = DependencyProperty.Register("TabletPCItems", typeof(ObservableCollection<object>), typeof(SectionListPage), new PropertyMetadata(null));
        #endregion

        #region AccessoryItems        
        public ObservableCollection<object> AccessoryItems
        {
            get { return (ObservableCollection<object>)GetValue(AccessoryItemsProperty); }
            set { SetValue(AccessoryItemsProperty, value); }
        }        
        public static readonly DependencyProperty AccessoryItemsProperty = DependencyProperty.Register("AccessoryItems", typeof(ObservableCollection<object>), typeof(SectionListPage), new PropertyMetadata(null));
        #endregion
        #region PhoneItems        
        public ObservableCollection<object> PhoneItems
        {
            get { return (ObservableCollection<object>)GetValue(PhoneItemsProperty); }
            set { SetValue(PhoneItemsProperty, value); }
        }
        public static readonly DependencyProperty PhoneItemsProperty = DependencyProperty.Register("PhoneItems", typeof(ObservableCollection<object>), typeof(SectionListPage), new PropertyMetadata(null));
        #endregion
        #region ConsoleItems        
        public ObservableCollection<object> ConsoleItems
        {
            get { return (ObservableCollection<object>)GetValue(ConsoleItemsProperty); }
            set { SetValue(ConsoleItemsProperty, value); }
        }
        public static readonly DependencyProperty ConsoleItemsProperty = DependencyProperty.Register("ConsoleItems", typeof(ObservableCollection<object>), typeof(SectionListPage), new PropertyMetadata(null));
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
            var items = new DevicesDataSource().GetItems();
            this.TabletPCItems = new ObservableCollection<object>(items.Where(x => x.Category == "Tablet/PC"));
            this.AccessoryItems = new ObservableCollection<object>(items.Where(x => x.Category == "Accessory"));
            this.PhoneItems = new ObservableCollection<object>(items.Where(x => x.Category == "Phone"));
            this.ConsoleItems = new ObservableCollection<object>(items.Where(x => x.Category == "Console"));
            this.ItemTemplate = Resources["DeviceItemTemplate"] as DataTemplate;
        }

        protected override void OnSettings()
        {
            AppShell.Current.Shell.ShowRightPane(new SectionListSettings() { DataContext = control });
        }
    }
}
