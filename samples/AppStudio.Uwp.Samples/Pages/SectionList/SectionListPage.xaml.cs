using System.Linq;
using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "LayoutControls", Name = "SectionList", Order = 50)]
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

        #region NatureItems
        public ObservableCollection<object> NatureItems
        {
            get { return (ObservableCollection<object>)GetValue(NatureItemsProperty); }
            set { SetValue(NatureItemsProperty, value); }
        }

        public static readonly DependencyProperty NatureItemsProperty = DependencyProperty.Register("NatureItems", typeof(ObservableCollection<object>), typeof(SectionListPage), new PropertyMetadata(null));
        #endregion

        #region MotorItems        
        public ObservableCollection<object> MotorItems
        {
            get { return (ObservableCollection<object>)GetValue(MotorItemsProperty); }
            set { SetValue(MotorItemsProperty, value); }
        }        

        public static readonly DependencyProperty MotorItemsProperty = DependencyProperty.Register("MotorItems", typeof(ObservableCollection<object>), typeof(SectionListPage), new PropertyMetadata(null));
        #endregion

        #region CityItems        
        public ObservableCollection<object> CityItems
        {
            get { return (ObservableCollection<object>)GetValue(CityItemsProperty); }
            set { SetValue(CityItemsProperty, value); }
        }

        public static readonly DependencyProperty CityItemsProperty = DependencyProperty.Register("CityItems", typeof(ObservableCollection<object>), typeof(SectionListPage), new PropertyMetadata(null));
        #endregion

        #region AnimalItems        
        public ObservableCollection<object> AnimalItems
        {
            get { return (ObservableCollection<object>)GetValue(AnimalItemsProperty); }
            set { SetValue(AnimalItemsProperty, value); }
        }

        public static readonly DependencyProperty AnimalItemsProperty = DependencyProperty.Register("AnimalItems", typeof(ObservableCollection<object>), typeof(SectionListPage), new PropertyMetadata(null));
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
            var items = new PhotosDataSource().GetItems();

            this.NatureItems = new ObservableCollection<object>(items.Where(x => x.Category == "Nature"));
            this.AnimalItems = new ObservableCollection<object>(items.Where(x => x.Category == "Animal"));
            this.CityItems = new ObservableCollection<object>(items.Where(x => x.Category == "City"));
            this.MotorItems = new ObservableCollection<object>(items.Where(x => x.Category == "Motor"));            
            this.ItemTemplate = Resources["PhotoItemTemplate"] as DataTemplate;

            base.OnNavigatedTo(e);
        }

        protected override void OnSettings()
        {
            AppShell.Current.Shell.ShowRightPane(new SectionListSettings() { DataContext = control });
        }
    }
}
