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
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.Foundation;
using AppStudio.Uwp.Navigation;
using Windows.UI.Xaml;

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
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(320, 600));
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
                
                yield return new NavigationItem(new Uri("ms-appx:///Assets/Icons/IconLayouts.png"), this.GetResourceString("ShellMenuLayoutControls"), GetControlsByCategory("LayoutControls"), this.GetCategoryBackground("LayoutControls"));
                yield return new NavigationItem(new Uri("ms-appx:///Assets/Icons/IconFoundation.png"), this.GetResourceString("ShellMenuFoundationControls"), GetControlsByCategory("FoundationControls"), this.GetCategoryBackground("FoundationControls"));
                yield return new NavigationItem(new Uri("ms-appx:///Assets/Icons/IconAppServices.png"), this.GetResourceString("ShellMenuAppServices"), GetControlsByCategory("AppServices"), this.GetCategoryBackground("AppServices"));
                yield return new NavigationItem(new Uri("ms-appx:///Assets/Icons/IconUtilities.png"), this.GetResourceString("ShellMenuUtilities"), GetControlsByCategory("Utilities"), this.GetCategoryBackground("Utilities"));
                yield return new NavigationItem(new Uri("ms-appx:///Assets/Icons/IconDataProviders.png"), this.GetResourceString("ShellMenuDataProviders"), GetControlsByCategory("DataProviders"), this.GetCategoryBackground("DataProviders"));

                //yield return NavigationItem.Separator;
                //yield return new NavigationItem(Symbol.FullScreen, this.GetResourceString("ShellMenuEnterFullScreen"), (m) => { shell.EnterFullScreen(); });

                yield return NavigationItem.Separator;
                yield return new NavigationItem(Symbol.Help, "About", new About());
            }
        }

        //private Brush GetBackgroundByCategory(string category)
        //{
        //    string resourceName = string.Format("{0}Background", category);
        //    if (!Resources.ContainsKey(resourceName))
        //    {
        //        return null;
        //    }
        //    return Resources[resourceName] as Brush;
        //}

        public IEnumerable<NavigationItem> GetControlsByCategory(string category)
        {
            var currentAssembly = this.GetType().GetTypeInfo().Assembly;
            var samplePages = currentAssembly.DefinedTypes.Where(type => type.CustomAttributes.Any(attr => this.IsSamplePageByCategory(attr, category)));

            foreach (var page in samplePages.OrderBy(t => t.GetCustomAttribute<SamplePageAttribute>().Order))
            {
                var attr = page.GetCustomAttribute<SamplePageAttribute>();

                if (attr.Glyph != null)
                {
                    yield return new NavigationItem(attr.Glyph, attr.Name, (ni) => NavigateToSample(page.AsType()), this.GetCategoryBackground(category));
                }
                else if (attr.IconPath != null)
                {
                    yield return new NavigationItem(new Uri(attr.IconPath), attr.Name, (ni) => NavigateToSample(page.AsType()), this.GetCategoryBackground(category));
                }
                else
                {
                    yield return new NavigationItem(attr.Symbol, attr.Name, (ni) => NavigateToSample(page.AsType()), this.GetCategoryBackground(category));
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
            NavigationService.Initialize(typeof(App), AppFrame);

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

            //SamplePage page = AppFrame.Content as SamplePage;
            //if (page != null)
            //{
            //    this.CommandBarBackground = GetBackgroundByCategory();
            //}            
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
            NavigationService.NavigateToPage(typeof(T));
        }

        private void NavigateToSample(Type type)
        {
            NavigationService.NavigateToPage(type);
        }
        #endregion        
    }
}
