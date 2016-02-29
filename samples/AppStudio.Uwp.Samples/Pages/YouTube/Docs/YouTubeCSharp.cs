using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Uwp.Samples
{
    public sealed partial class YouTubeSample : Page
    {
        public YouTubeSample()
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
            .Register("Items", typeof(ObservableCollection<object>), typeof(YouTubeSample), new PropertyMetadata(null));

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Initialize items collection
            this.Items = new ObservableCollection<object>(Items);
        }

        private IEnumerable<object> Items
        {
            get
            {
                yield return "/Images/Sample01.jpg";
                yield return "/Images/Sample02.jpg";
                yield return "/Images/Sample03.jpg";
                // ...
            }
        }
    }
}
