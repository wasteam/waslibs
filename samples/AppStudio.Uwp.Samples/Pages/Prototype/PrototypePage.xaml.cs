using System;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

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

        protected override IEnumerable<ICommandBarElement> CreateSecondaryCommands()
        {
            yield return base.CreateAppBarButton("Secondary Command 1", OnSecondaryCommand);
            yield return base.CreateAppBarButton("Secondary Command 2", OnSecondaryCommand);
            yield return base.CreateAppBarButton("Secondary Command 3", OnSecondaryCommand);
        }

        private async void OnSecondaryCommand(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog((sender as AppBarButton).Label);
            await dialog.ShowAsync();
        }

        protected override void OnSettings()
        {
            AppShell.Current.Shell.ShowRightPane(new PrototypeSettings() { DataContext = control });
        }
    }
}
