using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AppStudio.Common.Commands;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Common.Navigation
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors", Justification = "This class needs to be instantiated from XAML.")]
    public class NavigationService
    {
        public static event EventHandler<NavigatedEventArgs> NavigatedToPage;

        private static Assembly _appAssembly;
        private static Frame _rootFrame;

        public static void Initialize(Type app, Frame rootFrame)
        {
            _appAssembly = app.GetTypeInfo().Assembly;
            _rootFrame = rootFrame;
        }

        public static void NavigateToPage(Type page)
        {
            NavigateToPage(page, null);
        }

        public static void NavigateToPage(Type page, object parameter)
        {
            if (IsInitialized() && page != null)
            {
                _rootFrame.Navigate(page, parameter);

                var navigatedToPage = NavigatedToPage;
                if (navigatedToPage != null)
                {
                    NavigatedToPage(null, new NavigatedEventArgs(page, parameter));
                }
            }
        }

        public static void NavigateToPage(string page)
        {
            NavigateToPage(page, null);
        }

        public static void NavigateToPage(string page, object parameter)
        {
            var targetPage = _appAssembly.DefinedTypes.FirstOrDefault(t => t.Name == page);

            if (targetPage != null)
            {
                NavigateToPage(targetPage.AsType(), parameter);
            }
        }

        public static async Task NavigateTo(Uri uri)
        {
            if (uri != null)
            {
                await Launcher.LaunchUriAsync(uri);
            }
        }

        public static void NavigateTo(INavigable item)
        {
            if (item != null && item.NavigationInfo != null)
            {
                if (item.NavigationInfo.NavigationType == NavigationType.Page)
                {
                    var navParam = item.NavigationInfo.IncludeState ? item : null;

                    NavigationService.NavigateToPage(item.NavigationInfo.TargetPage, navParam);
                }
                else if (item.NavigationInfo.NavigationType == NavigationType.DeepLink)
                {
                    NavigationService.NavigateTo(item.NavigationInfo.TargetUri).RunAndForget();
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
                var targetBackStack = _rootFrame.BackStack.Last();

                _rootFrame.GoBack();

                if (targetBackStack != null && NavigatedToPage != null)
                {
                    NavigatedToPage(null, new NavigatedEventArgs(targetBackStack.SourcePageType, targetBackStack.Parameter));
                }
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

        private static bool IsInitialized()
        {
            return _rootFrame != null;
        }
    }

    public class NavigatedEventArgs : EventArgs
    {
        public NavigatedEventArgs(Type page, object parameter)
        {
            Page = page;
            Parameter = parameter;
        }

        public Type Page { get; private set; }
        public object Parameter { get; private set; }
    }
}
