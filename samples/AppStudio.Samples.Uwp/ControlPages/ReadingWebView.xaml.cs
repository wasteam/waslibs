using AppStudio.Uwp.Commands;
using System;
using System.IO;
using System.Windows.Input;
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
                //return "/Assets/Phone.png";
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
            //WebContent =  "01. Lorem ipsum dolor sit amet (03:35)\r\n02. Duis sit (04:02)\r\n03. Aenean (02:09)\r\n04. In in massa id sem varius tincidunt (05:43)\r\n05. Nullam urna arcu egestas non augue a tincidunt lobortis sapien (14:52)\r\n06. Maecenas (01:30)\r\n07. Ut (06:32)\r\n08. 888 (08:00)\r\n09. Cras eu purus elementum (06:35)\r\n10. Bye! (01:12)";
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Assets/WebContent.html"));
            using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync()))
            {
                WebContent = await sRead.ReadToEndAsync();
            }
        }

        public ICommand ChangeFontSizeCommand
        {
            get
            {
                return new RelayCommand<string>(async (parameter) =>
                {
                    await readingWebView.TryApplyFontSizes(Int32.Parse(parameter));
                });
            }
        }
    }
}
