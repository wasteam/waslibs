using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Foundation.Metadata;

using AppStudio.Uwp.Controls;
using AppStudio.Uwp.Samples.Extensions;

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
                yield return new NavigationItem(Symbol.Home, this.GetResourceString("ShellMenuHome"), GoHome);
                yield return NavigationItem.Separator;

                yield return new NavigationItem(Symbol.SelectAll, this.GetResourceString("ShellMenuLayoutControls"), GetControlsByCategory("Layout"));
                yield return new NavigationItem(new Uri("ms-appx:///Assets/Icons/Misc.png"), this.GetResourceString("ShellMenuMiscControls"), GetControlsByCategory("Misc"));
                yield return new NavigationItem(Symbol.Repair, this.GetResourceString("ShellMenuTools"), GetControlsByCategory("Tools"));
                yield return new NavigationItem(Symbol.CalendarWeek, this.GetResourceString("ShellMenuDataProviders"), GetControlsByCategory("DataProviders"));

                //yield return NavigationItem.Separator;
                //yield return new NavigationItem(Symbol.FullScreen, this.GetResourceString("ShellMenuEnterFullScreen"), (m) => { shell.EnterFullScreen(); });

                yield return NavigationItem.Separator;
                yield return new NavigationItem(Symbol.Help, "About", new About());
            }
        }

        public IEnumerable<NavigationItem> GetControlsByCategory(string category)
        {
            var currentAssembly = this.GetType().GetTypeInfo().Assembly;
            var samplePages = currentAssembly.DefinedTypes.Where(type => type.CustomAttributes.Any(attr => IsSamplePageByCategory(attr, category)));

            foreach (var page in samplePages.OrderBy(t => t.GetCustomAttribute<SamplePageAttribute>().Order))
            {
                var attr = page.GetCustomAttribute<SamplePageAttribute>();

                if (attr.Glyph != null)
                {
                    yield return new NavigationItem(attr.Glyph, attr.Name, (ni) => NavigateToSample(page.AsType()));
                }
                else if (attr.IconPath != null)
                {
                    yield return new NavigationItem(new Uri(attr.IconPath), attr.Name, (ni) => NavigateToSample(page.AsType()));
                }
                else
                {
                    yield return new NavigationItem(attr.Symbol, attr.Name, (ni) => NavigateToSample(page.AsType()));
                }
            }
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

        #region Reflection Helpers
        private bool IsSamplePageByCategory(CustomAttributeData attr, string category)
        {
            return attr.NamedArguments.Any(arg => attr.AttributeType == typeof(SamplePageAttribute) && arg.MemberName == "Category" && arg.TypedValue.Value.ToString() == category);
        }
        #endregion
    }
}
