using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AppStudio.Samples.Controls.W10.ControlPages
{
    public sealed partial class ReadingWebView : UserControl, INotifyPropertyChanged
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

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion INotifyPropertyChanged
    }
}
