using AppStudio.Uwp.Commands;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "FoundationControls", Name = "InfiniteScroll", Order = 50)]
    public sealed partial class InfiniteScrollPage : SamplePage
    {
        public InfiniteScrollPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public override string Caption
        {
            get { return "Infinite Scroll Control"; }
        }

        #region Items
        public ObservableCollection<string> Items
        {
            get { return (ObservableCollection<string>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<string>), typeof(InfiniteScrollPage), new PropertyMetadata(new ObservableCollection<string>()));
        #endregion

        #region ItemTemplate
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(InfiniteScrollPage), new PropertyMetadata(null));
        #endregion

        #region IsBusy
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register("IsBusy", typeof(bool), typeof(InfiniteScrollPage), new PropertyMetadata(false));
        #endregion

        #region EndOfScrollCommand
        public ICommand EndOfScrollCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await LoadData(40);
                });
            }
        }
        #endregion

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.ItemTemplate = Resources["NumberTemplate"] as DataTemplate;
            Items.Clear();
            await LoadData(80);
            base.OnNavigatedTo(e);
        }

        private async Task LoadData(int dataCount)
        {
            IsBusy = true;
            await Task.Delay(3000);
            int from = Items.Count;
            int max = Items.Count + dataCount;
            for (int i = from; i < max; i++)
            {
                Items.Add(i.ToString());
            }
            IsBusy = false;
        }
    }
}
