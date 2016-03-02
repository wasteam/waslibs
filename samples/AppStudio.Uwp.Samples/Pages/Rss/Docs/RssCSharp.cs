using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;


namespace AppStudio.Uwp.Samples
{
    public sealed partial class RssSample : Page
    {
        public RssSample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty
            .Register("Items", typeof(ObservableCollection<object>), typeof(VariableSizedGridSample), new PropertyMetadata(null));

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {           
            GetItems();
        }

        public void GetItems()
        {
           
        }

    }
}
