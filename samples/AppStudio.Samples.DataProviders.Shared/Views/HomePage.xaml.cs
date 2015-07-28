using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AppStudio.Samples.DataProviders.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        private List<string> _dataProvidersList;
        public List<string> DataProvidersList
        {
            get
            {
                if (_dataProvidersList == null)
                {
                    _dataProvidersList = new List<string>()
                    {
                        "RSS",
                        "YouTube",
                        "Flickr",
                        "Bing",
                        "Facebook",
                        "Instagram",
                        "Twitter",
                        "LocalStorage"
                    };
                }
                return _dataProvidersList;
            }
        }
        public HomePage()
        {
            this.InitializeComponent();
        }

        private void DataProvidersListItemClick(object sender, ItemClickEventArgs e)
        {            
            if (e != null && e.ClickedItem != null)
            {
                if (e.ClickedItem.ToString() == "RSS")
                    AppStudio.Samples.DataProviders.App.Navigate(typeof(RSSView));
                else if (e.ClickedItem.ToString() == "YouTube")
                    AppStudio.Samples.DataProviders.App.Navigate(typeof(YouTubeView));
                else if (e.ClickedItem.ToString() == "Flickr")
                    AppStudio.Samples.DataProviders.App.Navigate(typeof(FlickrView));
                else if (e.ClickedItem.ToString() == "Bing")
                    AppStudio.Samples.DataProviders.App.Navigate(typeof(BingView));
                else if (e.ClickedItem.ToString() == "Facebook")
                    AppStudio.Samples.DataProviders.App.Navigate(typeof(FacebookView));
                else if (e.ClickedItem.ToString() == "Instagram")
                    AppStudio.Samples.DataProviders.App.Navigate(typeof(InstagramView));
                else if (e.ClickedItem.ToString() == "Twitter")
                    AppStudio.Samples.DataProviders.App.Navigate(typeof(TwitterView));
                else if (e.ClickedItem.ToString() == "LocalStorage")
                    AppStudio.Samples.DataProviders.App.Navigate(typeof(LocalStorageView));
            }
        }
    }
}
