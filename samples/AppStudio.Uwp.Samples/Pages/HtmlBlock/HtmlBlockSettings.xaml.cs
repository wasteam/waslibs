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

namespace AppStudio.Uwp.Samples
{
    sealed partial class HtmlBlockSettings : UserControl
    {
        public HtmlBlockSettings()
        {
            this.InitializeComponent();
            Loaded += HtmlBlockSettings_Loaded;
        }

        private void HtmlBlockSettings_Loaded(object sender, RoutedEventArgs e)
        {
            stylesCombo.Items.Add("None");
            stylesCombo.Items.Add("Sample style");
            stylesCombo.SelectedIndex = 0;
        }

        private void stylesCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var htmlBlock = DataContext as HtmlBlock;

            if (stylesCombo.SelectedIndex == 0)
            {
                htmlBlock.Style = null;
            }
            else if(stylesCombo.SelectedIndex == 1)
            {
                var r = this.Resources["sampleStyle"];
                htmlBlock.Style = r as Style;
            }
        }
    }
}
