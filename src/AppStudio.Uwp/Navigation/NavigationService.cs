using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Windows.System;
using Windows.UI.Xaml.Controls;

using AppStudio.Uwp.Commands;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;

namespace AppStudio.Uwp.Navigation
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors", Justification = "This class needs to be instantiated from XAML.")]
    public class NavigationService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public static event EventHandler<NavigationEventArgs> Navigated;

        private static Assembly _appAssembly;
        private static Frame _rootFrame;
        private static Type _rootPage;
        private static bool _handleSystemButtons;

        public static void Initialize(Type app, Frame rootFrame, bool handleSystemButtons = false, Type rootPage = null)
        {
            _appAssembly = app.GetTypeInfo().Assembly;
            _rootFrame = rootFrame;
            _rootPage = rootPage;
            _handleSystemButtons = handleSystemButtons;

            _rootFrame.Navigated += _rootFrame_Navigated;

            if (_handleSystemButtons && SystemNavigationManager.GetForCurrentView() != null)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            }
        }

        public static Frame RootFrame
        {
            get { return _rootFrame; }
        }

        public static void NavigateToRoot()
        {
            if (_rootPage == null)
            {
                while (_rootFrame.BackStackDepth > 1)
                {
                    _rootFrame.BackStack.RemoveAt(_rootFrame.BackStackDepth - 1);
                }

                GoBack();
            }
            else
            {
                _rootFrame.BackStack.Clear();

                NavigateToPage(_rootPage, true);
            }
        }

        public static void NavigateToPage<T>(object parameter = null, bool force = false)
        {
            NavigateToPage(typeof(T), parameter, force);
        }

        public static void NavigateToPage(Type page, bool force = false)
        {
            NavigateToPage(page, null, force);
        }

        public static void NavigateToPage(string page, bool force = false)
        {
            NavigateToPage(page, null, force);
        }

        public static void NavigateToPage(string page, object parameter, bool force = false)
        {
            CheckIsInitialized();

            var targetPage = _appAssembly.DefinedTypes.FirstOrDefault(t => t.Name == page);

            if (targetPage != null)
            {
                NavigateToPage(targetPage.AsType(), parameter, force);
            }
        }

        public static void NavigateToPage(Type page, object parameter, bool force = false)
        {
            CheckIsInitialized();
            if (page == null) return;

            if (!IsSamePage(page) || force)
            {
                _rootFrame.Navigate(page, parameter);
            }
        }

        private static bool IsSamePage(Type page)
        {
            if (_rootFrame.Content != null)
            {
                return _rootFrame.Content.GetType() == page;
            }
            return false;
        }

        [Obsolete("Use Launcher.LaunchUriAsync instead")]
        public static async Task NavigateTo(Uri uri)
        {
            if (uri != null)
            {
                await Launcher.LaunchUriAsync(uri);
            }
        }

        [Obsolete("Implement your custom navigation logic")]
        public static void NavigateTo(INavigable item, bool force = false)
        {
            if (item != null && item.NavigationInfo != null)
            {
                if (item.NavigationInfo.NavigationType == NavigationType.Page)
                {
                    var navParam = item.NavigationInfo.IncludeState ? item : null;

                    NavigationService.NavigateToPage(item.NavigationInfo.TargetPage, navParam, force);
                }
                else if (item.NavigationInfo.NavigationType == NavigationType.DeepLink)
                {
                    NavigationService.NavigateTo(item.NavigationInfo.TargetUri).FireAndForget();
                }
                else
                {
                    throw new NotSupportedException("Navigation type provided is not supported.");
                }
            }
        }

        public static RelayCommand GoBackCommand
        {
            get
            {
                return new RelayCommand(
                        () => GoBack(),
                        () => CanGoBack());
            }
        }

        public static bool CanGoBack()
        {
            return IsInitialized() && _rootFrame.CanGoBack;
        }

        public static void GoBack()
        {
            if (CanGoBack())
            {
                _rootFrame.GoBack();
            }
        }

        public static RelayCommand GoForwardCommand
        {
            get
            {
                return new RelayCommand(
                        () => GoForward(),
                        () => CanGoForward());
            }
        }

        public static bool CanGoForward()
        {
            return IsInitialized() && _rootFrame.CanGoForward;
        }

        public static void GoForward()
        {
            if (CanGoForward())
            {
                _rootFrame.GoForward();
            }
        }

        private static void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (NavigationService.CanGoBack())
            {
                NavigationService.GoBack();
                e.Handled = true;
            }
        }

        private static void _rootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            Navigated?.Invoke(sender, e);

            if (_handleSystemButtons && SystemNavigationManager.GetForCurrentView() != null)
            {
                if (CanGoBack())
                {
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                }
                else
                {
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                }
            }
        }

        private static void CheckIsInitialized()
        {
            if (!IsInitialized())
            {
                throw new NavigationInitializationException();
            }
        }

        private static bool IsInitialized()
        {
            return _rootFrame != null && _appAssembly != null;
        }
    }
}
