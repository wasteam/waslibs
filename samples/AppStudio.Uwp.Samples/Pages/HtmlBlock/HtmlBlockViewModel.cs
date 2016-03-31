using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace AppStudio.Uwp.Samples.Pages.HtmlBlock
{
    class HtmlBlockViewModel : ObservableBase
    {
        private string _html;
        public string Html
        {
            get { return _html; }
            set { SetProperty(ref _html, value); }
        }

        public async Task LoadStateAsync()
        {
            var uri = new Uri("ms-appx:///Assets/HtmlBlock/Sample.html");

            var file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            var randomStream = await file.OpenReadAsync();

            using (StreamReader r = new StreamReader(randomStream.AsStreamForRead()))
            {
                Html = await r.ReadToEndAsync();
            }
        }
    }
}
