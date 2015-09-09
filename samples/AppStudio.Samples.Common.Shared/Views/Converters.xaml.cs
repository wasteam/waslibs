using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace AppStudio.Samples.Common
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Converters : Page, INotifyPropertyChanged
    {
        private bool _boolValue;
        public bool BoolValue
        {
            get { return _boolValue; }
            set { _boolValue = value; OnPropertyChanged("BoolValue"); }
        }
        private string _stringValue;
        public string StringValue
        {
            get { return _stringValue; }
            set { _stringValue = value; OnPropertyChanged("StringValue"); }
        }
        private string _stringToSizeImagePath;
        public string StringToSizeImagePath
        {
            get { return _stringToSizeImagePath; }
            set { _stringToSizeImagePath = value; OnPropertyChanged("StringToSizeImagePath"); }
        }
        public Converters()
        {
            this.InitializeComponent();
            StringValue = "Lorem ipsum";
            BoolValue = true;
            StringToSizeImagePath = "ms-appx:///Assets/SampleImage.png";
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
        #endregion
    }
}
