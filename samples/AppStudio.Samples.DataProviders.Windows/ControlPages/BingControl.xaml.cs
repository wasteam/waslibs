using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using AppStudio.DataProviders.Bing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

namespace AppStudio.Samples.DataProviders.ControlPages
{
    public sealed partial class BingControl : BaseBingControl
    {         
        public BingControl()
        {
            this.InitializeComponent();
        }
    }
}
