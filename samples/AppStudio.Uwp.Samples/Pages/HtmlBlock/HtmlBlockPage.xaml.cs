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

namespace AppStudio.Uwp.Samples.Pages.HtmlBlock
{
    [SamplePage(Category = "FoundationControls", Name = "HtmlBlock", Order = 30)]
    public sealed partial class HtmlBlockPage : SamplePage
    {
        public string Html { get; set; }

        public HtmlBlockPage()
        {
            this.InitializeComponent();

            Html = "<p>hola don pepito!</p><p>hola don josé!</p><div><div>adios don pepito!</div><div>adios don josé!</div></div>";
            //Html = "<div><div>adios don pepito!</div><div>adios don josé!</div></div>";
            DataContext = Html;
        }

        public override string Caption
        {
            get { return "HtmlBlock Sample"; }
        }
    }
}
