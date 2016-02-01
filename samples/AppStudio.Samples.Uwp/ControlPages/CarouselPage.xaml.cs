using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;

using AppStudio.Uwp.Commands;
using AppStudio.Uwp.Controls;
using AppStudio.Uwp.Navigation;

namespace AppStudio.Samples.Uwp.ControlPages
{
    public sealed partial class CarouselPage : Page
    {
        public CarouselPage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        public IEnumerable<FeedSchema> Items1 { get; set; }
        public IEnumerable<FeedSchema> Items2 { get; set; }
        public IEnumerable<FeedSchema> Items3 { get; set; }
        public IEnumerable<FeedSchema> Items4 { get; set; }
        public IEnumerable<FeedSchema> Items5 { get; set; }
        public IEnumerable<FeedSchema> Items6 { get; set; }
        public IEnumerable<FeedSchema> Items7 { get; set; }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Items1 = new List<FeedSchema>(await GetItems("StarWars1.xml"));
            this.Items2 = new List<FeedSchema>(await GetItems("StarWars2.xml"));
            this.Items3 = new List<FeedSchema>(await GetItems("StarWars3.xml"));
            this.Items4 = new List<FeedSchema>(await GetItems("StarWars4.xml"));
            this.Items5 = new List<FeedSchema>(await GetItems("StarWars5.xml"));
            this.Items6 = new List<FeedSchema>(await GetItems("StarWars6.xml"));
            this.Items7 = new List<FeedSchema>(await GetItems("StarWars7.xml"));

            this.DataContext = this;

            //carousel.SelectedIndex = 0;
        }

        private async Task<IEnumerable<FeedSchema>> GetItems(string name)
        {
            return FeedParser.Parse(await ReadFile("Assets/Content/" + name));
        }

        public ICommand ItemClickCommand
        {
            get
            {
                return new RelayCommand<FeedSchema>(async (item) =>
                {
                    await NavigationService.NavigateTo(new Uri(item.MediaUrl, UriKind.Absolute));
                });
            }
        }

        #region GetResponse
        private async Task<string> ReadFile(string path)
        {
            string url = String.Format("ms-appx:///{0}", path);
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(url));
            var randomStream = await file.OpenReadAsync();

            using (var r = new StreamReader(randomStream.AsStreamForRead()))
            {
                return await r.ReadToEndAsync();
            }
        }
        #endregion
    }
}
