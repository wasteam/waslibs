using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AppStudio.DataProviders.Facebook;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AppStudio.Samples.DataProviders.ControlPages
{
    public sealed partial class FacebookControl : BaseFacebookControl
    {
        public FacebookControl()
        {
            this.InitializeComponent();
        }
    }
}
