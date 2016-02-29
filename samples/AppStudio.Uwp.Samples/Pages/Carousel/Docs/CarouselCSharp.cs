using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using AppStudio.Uwp.Samples;

namespace AppStudio.Uwp.Samples
{
    public sealed partial class CarouselSample : Page
    {
        public CarouselSample()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }
    }
}
