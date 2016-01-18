// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

using AppStudio.Uwp.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AppStudio.Samples.Uwp.ControlPages
{
    public sealed partial class Converters : BaseControlPage
    {
        private bool _boolValue;
        public bool BoolValue
        {
            get { return _boolValue; }
            set { SetProperty(ref _boolValue, value); }
        }
        private bool _secondBoolValue;
        public bool SecondBoolValue
        {
            get { return _secondBoolValue; }
            set { SetProperty(ref _secondBoolValue, value); }
        }
        private string _stringValue;
        public string StringValue
        {
            get { return _stringValue; }
            set { SetProperty(ref _stringValue, value); }
        }
        private string _stringToSizeImagePath;
        public string StringToSizeImagePath
        {
            get { return _stringToSizeImagePath; }
            set { SetProperty(ref _stringToSizeImagePath, value); }
        }
        private ObservableCollection<string> _collection;
        public ObservableCollection<string> Collection
        {
            get { return _collection; }
            set { SetProperty(ref _collection, value); }
        }
        public ICommand AddItemsToCollection
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Collection = new ObservableCollection<string>()
                    {
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString(),
                        Guid.NewGuid().ToString()
                    };

                });
            }
        }
        public ICommand ClearCollection
        {
            get
            {
                return new RelayCommand(() =>
                {
                    this.Collection = null;
                });
            }
        }
        public Converters()
        {
            this.InitializeComponent();
            Collection = new ObservableCollection<string>()
            {
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString()
            };
            StringValue = "Lorem ipsum";
            BoolValue = true;
            SecondBoolValue = true;
            StringToSizeImagePath = "ms-appx:///Assets/SampleImage.png";
        }
    }
}
