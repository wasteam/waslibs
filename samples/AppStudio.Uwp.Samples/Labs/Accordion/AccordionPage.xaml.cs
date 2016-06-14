using System.Linq;
using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Uwp.Samples.Labs
{
    [SamplePage(Category = "Labs", Name = "Accordion", Order = 20)]
    public sealed partial class AccordionPage : SamplePage
    {
        public AccordionPage() : base(true)
        {
            this.InitializeComponent();
            this.DataContext = this;
            commandBar.DataContext = this;
            paneHeader.DataContext = this;
        }

        public override string Caption
        {
            get { return "Accordion"; }
        }

        #region Items
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<object>), typeof(AccordionPage), new PropertyMetadata(null));
        #endregion

        #region ItemTemplate
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(AccordionPage), new PropertyMetadata(null));
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.Items = new ObservableCollection<object>(PhotosDataSource.GetItems().Skip(4));
            base.OnNavigatedTo(e);
        }

        protected override void OnSettings()
        {
            AppShell.Current.Shell.ShowRightPane(new AccordionSettings() { DataContext = control });
        }
    }
}
