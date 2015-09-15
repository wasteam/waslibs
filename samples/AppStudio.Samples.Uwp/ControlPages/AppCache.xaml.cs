using AppStudio.Uwp.Cache;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AppStudio.Samples.Uwp.ControlPages
{
    public sealed partial class AppCache : BaseControlPage
    {
        private ObservableCollection<string> _items;
        public ObservableCollection<string> Items
        {
            get
            {
                if (_items == null) { _items = new ObservableCollection<string>(); }
                return _items;
            }
        }
        public AppCache()
        {
            this.InitializeComponent();
        }

        private void CleanListClick(object sender, RoutedEventArgs e)
        {
            Items.Clear();
        }

        private void AddToListItem(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ItemString.Text))
            {
                Items.Add(ItemString.Text);
                ItemString.Text = string.Empty;
            }
        }        

        private async void LoadToCache(object sender, RoutedEventArgs e)
        {
            Items.Clear();
            var cacheData = await AppStudio.Uwp.Cache.AppCache.GetItemsAsync<string>("AppCacheList");
            if (cacheData != null && cacheData.Items != null)
            {
                foreach (var item in cacheData.Items)
                {
                    Items.Add(item);
                }
            }
        }

        private async void SaveToCache(object sender, RoutedEventArgs e)
        {
            CachedContent<string> cacheData = new CachedContent<string>() { Items = Items };
            await AppStudio.Uwp.Cache.AppCache.AddItemsAsync<string>("AppCacheList", cacheData);
        }
    }
}
