using AppStudio.Uwp.Controls;
using AppStudio.Uwp.Samples.Pages.HtmlBlock;
using System;
using System.Collections.Generic;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AppStudio.Uwp.Samples
{
    sealed partial class HtmlBlockSettings : UserControl
    {
        public HtmlBlockSettings()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var htmlBlock = DataContext as HtmlBlock;
            var r = this.Resources["style2"];
            htmlBlock.Style = r as Style;
        }
    }
}
