using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Uwp.Samples
{
    public sealed partial class AppCachePage : SamplePage
    {
        public ObservableCollection<DeviceDataItem> Items;        

        #region MemoryItems
        public ObservableCollection<DeviceDataItem> MemoryItems
        {
            get { return (ObservableCollection<DeviceDataItem>)GetValue(MemoryItemsProperty); }
            set { SetValue(MemoryItemsProperty, value); }
        }
        public static readonly DependencyProperty MemoryItemsProperty = DependencyProperty.Register("MemoryItems", typeof(ObservableCollection<DeviceDataItem>), typeof(AppCachePage), new PropertyMetadata(null));
        #endregion

        #region Commands
        public ICommand AddItemCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var randomITem = Items[new Random().Next(0, Items.Count - 1)];
                    MemoryItems.Add(randomITem);                    
                });
            }
        }
        public ICommand CleanMemoryCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    MemoryItems.Clear();                    
                });
            }
        }        
        public ICommand LoadFromCacheCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    var cacheData = await AppCache.GetItemsAsync<DeviceDataItem>("AppCacheSample");
                    if (cacheData != null && cacheData.Items != null)
                    {
                        MemoryItems.Clear();
                        foreach (var item in cacheData.Items)
                        {
                            MemoryItems.Add(item);
                        }
                    }
                });
            }
        }
        public ICommand CleanCacheCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await AppCache.AddItemsAsync<DeviceDataItem>("AppCacheSample", new CachedContent<DeviceDataItem>() { Items = new ObservableCollection<DeviceDataItem>() });                    
                });
            }
        }
        public ICommand SaveMemoryToCacheCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    await AppCache.AddItemsAsync<DeviceDataItem>("AppCacheSample", new CachedContent<DeviceDataItem>() { Items = MemoryItems });
                });
            }
        }
        #endregion

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.Items = new ObservableCollection<DeviceDataItem>(new DevicesDataSource().GetItems());
            this.MemoryItems = new ObservableCollection<DeviceDataItem>();
        }
    }
}
