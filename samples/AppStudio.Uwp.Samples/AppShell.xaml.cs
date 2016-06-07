using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Foundation.Metadata;

using AppStudio.Uwp.Navigation;
using AppStudio.Uwp.Controls;

namespace AppStudio.Uwp.Samples
{
    public sealed partial class AppShell : Page
    {
        private SystemNavigationManager _navigationManager = null;
        private IEnumerable<NavigationItem> _navigationItems = null;

        public AppShell()
        {
            Current = this;
            this.InitializeComponent();
            this.SetupNavigation();
            this.DataContext = this;
            this.SizeChanged += OnSizeChanged;
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
            get { return _navigationItems ?? (_navigationItems = GetNavigationItems().ToArray()); }
        }

        private IEnumerable<NavigationItem> GetNavigationItems()
        {
            yield return new NavigationItem("Home", Symbol.Home, this.GetResourceString("ShellMenuHome"), (ni) => NavigationService.NavigateToRoot());
            yield return NavigationItem.Separator;

            yield return new NavigationItem("LayoutControls", new Uri("ms-appx:///Assets/Icons/IconLayouts.png"), this.GetResourceString("ShellMenuLayoutControls"), GetControlsByCategory("LayoutControls"), this.GetCategoryBackground("LayoutControls"), this.GetCategoryBackground("LayoutControls"));
            yield return new NavigationItem("FoundationControls", new Uri("ms-appx:///Assets/Icons/IconFoundation.png"), this.GetResourceString("ShellMenuFoundationControls"), GetControlsByCategory("FoundationControls"), this.GetCategoryBackground("FoundationControls"), this.GetCategoryBackground("FoundationControls"));
            yield return new NavigationItem("AppServices", new Uri("ms-appx:///Assets/Icons/IconAppServices.png"), this.GetResourceString("ShellMenuAppServices"), GetControlsByCategory("AppServices"), this.GetCategoryBackground("AppServices"), this.GetCategoryBackground("AppServices"));
            yield return new NavigationItem("Utilities", new Uri("ms-appx:///Assets/Icons/IconUtilities.png"), this.GetResourceString("ShellMenuUtilities"), GetControlsByCategory("Utilities"), this.GetCategoryBackground("Utilities"), this.GetCategoryBackground("Utilities"));
            yield return new NavigationItem("DataProviders", new Uri("ms-appx:///Assets/Icons/IconDataProviders.png"), this.GetResourceString("ShellMenuDataProviders"), GetControlsByCategory("DataProviders"), this.GetCategoryBackground("DataProviders"), this.GetCategoryBackground("DataProviders"));

            yield return NavigationItem.Separator;
            yield return new NavigationItem("Labs", new Uri("ms-appx:///Assets/Icons/Flask.png"), this.GetResourceString("ShellMenuLabsControls"), GetControlsByCategory("Labs"), this.GetCategoryBackground("Labs"), this.GetCategoryBackground("Labs"));

            yield return NavigationItem.Separator;
            yield return new NavigationItem("About", new Uri("ms-appx:///Assets/Icons/about.png"), "About", new About()) { ClearSelection = true };
        }

        public IEnumerable<NavigationItem> GetControlsByCategory(string category)
        {
            var currentAssembly = this.GetType().GetTypeInfo().Assembly;
            var samplePages = currentAssembly.DefinedTypes.Where(type => type.CustomAttributes.Any(attr => this.IsSamplePageByCategory(attr, category)));

            foreach (var page in samplePages.OrderBy(t => t.GetCustomAttribute<SamplePageAttribute>().Order))
            {
                var attr = page.GetCustomAttribute<SamplePageAttribute>();

                if (attr.Glyph != null)
                {
                    yield return new NavigationItem(attr.Name, attr.Glyph, attr.Name, (ni) => NavigationService.NavigateToPage(page.AsType()), this.GetCategoryBackground(category));
                }
                else if (attr.IconPath != null)
                {
                    yield return new NavigationItem(attr.Name, new Uri(attr.IconPath), attr.Name, (ni) => NavigationService.NavigateToPage((page.AsType())), this.GetCategoryBackground(category), this.GetCategoryBackground(category));
                }
                else
                {
                    yield return new NavigationItem(attr.Name, attr.Symbol, attr.Name, (ni) => NavigationService.NavigateToPage((page.AsType()), this.GetCategoryBackground(category)));
                }
            }
        }

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
                shell.ClearSelection();
                AppFrame.GoBack();
                e.Handled = true;
            }
        }
        #endregion

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.shell.DisplayMode = e.NewSize.Width > 640 ? SplitViewDisplayMode.CompactOverlay : SplitViewDisplayMode.Overlay;
            this.Shell.CommandBarVerticalAlignment = e.NewSize.Width > 640 ? VerticalAlignment.Top : VerticalAlignment.Bottom;
        }
    }
}
