using System;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "Misc", Name = "Prototype")]
    public sealed partial class PrototypePage : SamplePage
    {
        public PrototypePage()
        {
            this.InitializeComponent();
        }

        public override string Caption
        {
            get { return "Prototype Sample"; }
        }

        protected override void OnSettings()
        {
            AppShell.Current.Shell.ShowRightPane(new PrototypeSettings() { DataContext = control });
        }
    }
}
