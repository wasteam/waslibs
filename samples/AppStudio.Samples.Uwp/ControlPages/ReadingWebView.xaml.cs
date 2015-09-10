using System;
using System.IO;
using Windows.Storage;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AppStudio.Samples.Uwp.ControlPages
{
    public sealed partial class ReadingWebView : BaseControlPage
    {
        public string ImageUrl
        {
            get
            {
                return "http://az648995.vo.msecnd.net/win/2015/08/Beagles.jpg";
            }
        }
        public string Title
        {
            get
            {
                return "Continuing our Mission to #UpgradeYourWorld";
            }
        }
        public string SubTitle
        {
            get
            {
                return "On July 13, we outlined how we would approach our launch to Windows 10. Since then we‘ve heard an overwhelming response from the Windows community around the world.We’re at 75 million users and running in 192 countries, going strong. We’re also making fast progress on our broader goal of celebrating people and organizations who[…]";
            }
        }
        private string _webContent;
        public string WebContent
        {
            get
            {
                return _webContent;
            }
            set
            {
                SetProperty(ref _webContent, value);
            }
        }
        public ReadingWebView()
        {
            this.InitializeComponent();
            LoadData();
        }

        private async void LoadData()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Assets/WebContent.html"));
            using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync()))
            {
                WebContent = await sRead.ReadToEndAsync();
            }
        }
    }
}
