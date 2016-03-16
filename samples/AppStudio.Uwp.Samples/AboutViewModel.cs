using Windows.ApplicationModel;

namespace AppStudio.Uwp.Samples
{
    public class AboutViewModel : ObservableBase
    {
        private string _appName;
        public string AppName
        {
            get { return _appName; }
            set { base.SetProperty(ref _appName, value); }
        }

        private string _publisher;
        public string Publisher
        {
            get { return _publisher; }
            set { base.SetProperty(ref _publisher, value); }
        }

        private string _version;
        public string Version
        {
            get { return _version; }
            set { base.SetProperty(ref _version, value); }
        }

        public void LoadState()
        {
            AppName = Package.Current.DisplayName;
            Publisher = Package.Current.PublisherDisplayName;

            var v = Package.Current.Id.Version;
            Version = $"{v.Major}.{v.Minor}.{v.Build}.{v.Revision}";
        }
    }
}

