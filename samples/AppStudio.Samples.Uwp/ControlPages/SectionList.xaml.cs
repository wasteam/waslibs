using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AppStudio.Samples.Uwp.ControlPages
{
    public sealed partial class SectionList : BaseControlPage
    {
        private ObservableCollection<SolidColorBrush> _items;
        public ObservableCollection<SolidColorBrush> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }
        public SectionList()
        {
            this.InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            this.Items = new ObservableCollection<SolidColorBrush>();
            this.Items.Add(new SolidColorBrush(Colors.Gray));
            this.Items.Add(new SolidColorBrush(Colors.LightGray));
            this.Items.Add(new SolidColorBrush(Colors.DarkGray));
            this.Items.Add(new SolidColorBrush(Colors.Gray));
            this.Items.Add(new SolidColorBrush(Colors.LightGray));
            this.Items.Add(new SolidColorBrush(Colors.DarkGray));
            this.Items.Add(new SolidColorBrush(Colors.Gray));
            this.Items.Add(new SolidColorBrush(Colors.LightGray));
            this.Items.Add(new SolidColorBrush(Colors.DarkGray));
        }
    }
}
