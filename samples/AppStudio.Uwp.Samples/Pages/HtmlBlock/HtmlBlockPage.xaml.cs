using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Uwp.Samples.Pages.HtmlBlock
{
    [SamplePage(Category = "FoundationControls", Name = "HtmlBlock", Order = 1)]
    sealed partial class HtmlBlockPage : SamplePage
    {
        public HtmlBlockViewModel ViewModel { get; set; } = new HtmlBlockViewModel();

        public HtmlBlockStyle DocumentStyle
        {
            get { return (HtmlBlockStyle)GetValue(DocumentStyleProperty); }
            set { SetValue(DocumentStyleProperty, value); }
        }

        public static readonly DependencyProperty DocumentStyleProperty = DependencyProperty.Register(nameof(DocumentStyle), typeof(HtmlBlockPage), typeof(HtmlBlockPage), new PropertyMetadata(HtmlBlockStyle.None, DocumentStyleChanged));

        private static void DocumentStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as HtmlBlockPage;
            if (self.DocumentStyle == HtmlBlockStyle.None)
            {
                self.Control.Style = null;
            }
            else
            {
                var r = self.Resources["sampleStyle"];
                self.Control.Style = r as Style;
            }
        }

        public FrameworkElement Control
        {
            get
            {
                return control;
            }
        }

        public HtmlBlockPage()
        {
            this.InitializeComponent();

            DataContext = ViewModel;

            commandBar.DataContext = this;
            paneHeader.DataContext = this;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadStateAsync();

            base.OnNavigatedTo(e);
        }

        public override string Caption
        {
            get { return "HtmlBlock Sample"; }
        }

        protected override void OnSettings()
        {
            AppShell.Current.Shell.ShowRightPane(new HtmlBlockSettings() { DataContext = this });
        }
    }

    enum HtmlBlockStyle
    {
        None,
        Sample
    }
}
