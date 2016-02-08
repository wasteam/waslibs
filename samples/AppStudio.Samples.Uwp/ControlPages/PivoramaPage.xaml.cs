using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Samples.Uwp.ControlPages
{
    public sealed partial class PivoramaPage : Page
    {
        public PivoramaPage()
        {
            this.InitializeComponent();
            this.Items = new ObservableCollection<ItemData>();
            this.DataContext = this;
            this.Loaded += OnLoaded;
        }

        #region Items
        public ObservableCollection<ItemData> Items
        {
            get { return (ObservableCollection<ItemData>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<ItemData>), typeof(MainPage), new PropertyMetadata(null));
        #endregion

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in ItemData.GetItems(7))
            {
                this.Items.Add(item);
            }
        }
    }
}
