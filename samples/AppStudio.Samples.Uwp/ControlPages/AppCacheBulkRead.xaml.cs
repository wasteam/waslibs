using AppStudio.Uwp.Cache;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Samples.Uwp.ControlPages
{
    public sealed partial class AppCacheBulkRead : BaseControlPage
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

        public AppCacheBulkRead()
        {
            this.InitializeComponent();
        }

        private void AddItemToList(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ItemString.Text))
            {
                Items.Add(ItemString.Text);
                ItemString.Text = string.Empty;
            }
        }
        private void ClearList(object sender, RoutedEventArgs e)
        {
            Items.Clear();
        }

        private async void SaveItemsToCache(object sender, RoutedEventArgs e)
        {
            foreach (string item in Items)
            {
                await AppStudio.Uwp.Cache.AppCache.AddItemAsync<string>($"{PrefixString.Text}_key_{item}", item);
            }
        }

        private async void LoadItemsFromCache(object sender, RoutedEventArgs e)
        {
            List<string> cacheItems = await AppStudio.Uwp.Cache.AppCache.GetItemsByPrefixAsync<string>(PrefixString.Text);
            if (cacheItems != null)
            {
                foreach (var item in cacheItems)
                {
                    Items.Add(item);
                }
            }
        }
    }
}
