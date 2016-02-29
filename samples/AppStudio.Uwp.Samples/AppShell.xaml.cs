using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Foundation.Metadata;

using AppStudio.Uwp.Controls;

namespace AppStudio.Uwp.Samples
{
    public sealed partial class AppShell : Page
    {
        private SystemNavigationManager _navigationManager;

        public AppShell()
        {
            this.InitializeComponent();
            this.SetupNavigation();
            this.DataContext = this;
            Current = this;
        }

        static public AppShell Current { get; private set; }

        public ShellControl Shell
        {
            get { return shell; }
        }

        public Frame AppFrame
        {
            get { return frame; }
        }

        public IEnumerable<NavigationItem> NavigationItems
        {
            get
            {
                yield return new NavigationItem(Symbol.Home, "Home", GoHome);
                yield return NavigationItem.Separator;

                yield return new NavigationItem(Symbol.SelectAll, "Layout Controls", GetControlsByCategory("Layout"));
                yield return new NavigationItem(Symbol.Library, "Misc Controls", GetControlsByCategory("Misc"));
                yield return new NavigationItem(Symbol.Repair, "Tools", GetControlsByCategory("Tools"));
                yield return new NavigationItem(Symbol.CalendarWeek, "Data Providers", GetControlsByCategory("DataProviders"));

                yield return NavigationItem.Separator;
                yield return new NavigationItem(Symbol.FullScreen, "Enter Full Screen", (m) => { shell.EnterFullScreen(); });
            }
        }

        public IEnumerable<NavigationItem> GetControlsByCategory(string category)
        {
            var currentAssembly = this.GetType().GetTypeInfo().Assembly;
            var samplePages = currentAssembly.DefinedTypes.Where(type => type.CustomAttributes.Any(attr => IsSamplePageByCategory(attr, category)));
            foreach (var page in samplePages)
            {
                var args = page.CustomAttributes.Where(attr => attr.AttributeType == typeof(SamplePageAttribute)).Select(attr => attr.NamedArguments).First();
                var name = args.Where(a => a.MemberName == "Name").Select(a => a.TypedValue.Value.ToString()).First();
                yield return new NavigationItem(name, (ni) => NavigateToSample(page.AsType()));
            }
        }

        private bool IsSamplePageByCategory(CustomAttributeData attr, string category)
        {
            return attr.NamedArguments.Any(arg => attr.AttributeType == typeof(SamplePageAttribute) && arg.MemberName == "Category" && arg.TypedValue.Value.ToString() == category);
        }

        #region GoHome
        public void GoHome(NavigationItem menuItem)
        {
            while (this.AppFrame.BackStackDepth > 1)
            {
                this.AppFrame.BackStack.RemoveAt(this.AppFrame.BackStackDepth - 1);
            }
            if (this.AppFrame.CanGoBack)
            {
                this.AppFrame.GoBack();
            }
        }
        #endregion

        #region Navigation
        private void SetupNavigation()
        {
            this.AppFrame.Navigated += OnNavigated;

            _navigationManager = SystemNavigationManager.GetForCurrentView();
            _navigationManager.BackRequested += OnBackRequested;
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            if (!ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                _navigationManager.AppViewBackButtonVisibility = AppFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            }
            this.Shell.HideTopPane();
            this.Shell.HideRightPane();
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (AppFrame.CanGoBack)
            {
                e.Handled = true;
                AppFrame.GoBack();
                shell.ClearSelection();
            }
        }
        #endregion

        #region NavigateToSample
        private void NavigateToSample<T>() where T : Page
        {
            if (AppFrame.Content.GetType() != typeof(T))
            {
                AppFrame.Navigate(typeof(T));
            }
        }

        private void NavigateToSample(Type type)
        {
            if (AppFrame.Content.GetType() != type)
            {
                AppFrame.Navigate(type);
            }
        }
        #endregion
    }
}
