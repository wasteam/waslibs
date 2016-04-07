using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "FoundationControls", Name = "ImageEx", Order = 10)]
    public sealed partial class ImageExPage : SamplePage
    {
        public ImageExPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            commandBar.DataContext = this;
            paneHeader.DataContext = this;
        }

        public override string Caption
        {
            get { return "ImageEx Control"; }
        }

        #region Items
        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<object>), typeof(ImageExPage), new PropertyMetadata(null));
        #endregion

        #region ItemTemplate
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(ImageExPage), new PropertyMetadata(null));
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.Refresh();
            this.ItemTemplate = Resources["PhotoTemplate"] as DataTemplate;

            base.OnNavigatedTo(e);
        }

        public void Clear()
        {
            this.Items = null;
        }

        public void Refresh()
        {
            this.Items = new ObservableCollection<object>(new PhotosDataSource().GetItems());
        }

        protected override void OnSettings()
        {
            AppShell.Current.Shell.ShowRightPane(new ImageExSettings() { Page = this });
        }
    }
}
