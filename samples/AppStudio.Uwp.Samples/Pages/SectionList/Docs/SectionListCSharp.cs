using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Uwp.Samples
{
    public sealed partial class SectionListSample : Page
    {
        public SectionListSample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }
        #region TabletPCItems
        public ObservableCollection<object> TabletPCItems
        {
            get { return (ObservableCollection<object>)GetValue(TabletPCItemsProperty); }
            set { SetValue(TabletPCItemsProperty, value); }
        }
        public static readonly DependencyProperty TabletPCItemsProperty = DependencyProperty.Register("TabletPCItems", typeof(ObservableCollection<object>), typeof(SectionListPage), new PropertyMetadata(null));
        #endregion
        //...
        //Create regions for all item categories
        //...

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.TabletPCItems = LoadTabletPCItems(); new ObservableCollection<object>(Items);
        }

        private ObservableCollection<object> LoadTabletPCItems()
        {
            yield return new DeviceDataItem("Surface Pro 4", "/Images/SurfacePro4.jpg");
            yield return new DeviceDataItem("Surface Book", "/Images/SurfaceBook.jpg");            
            // ...
        }
        //...
        //Load Data Methods for all item categories
        //...
    }
}
