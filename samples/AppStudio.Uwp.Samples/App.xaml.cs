using System;
using System.IO;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;

using AppStudio.Uwp.Navigation;

namespace AppStudio.Uwp.Samples
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.UnhandledException += OnUnhandledException;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.PreferredLaunchViewSize = new Size { Width = 1056, Height = 790 };
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(320, 600));

            await BingDataSource.Load();
            await DevicesDataSource.Load();
            await FeaturedControlsDataSource.Load();

            var shell = Window.Current.Content as AppShell;

            if (shell == null)
            {
                shell = new AppShell()
                {
                    Language = Windows.Globalization.ApplicationLanguages.Languages[0]
                };
                NavigationService.Initialize(typeof(App), shell.AppFrame);
                shell.AppFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }
            }

            Window.Current.Content = shell;

            if (shell.AppFrame.Content == null)
            {
                // When the navigation stack isn't restored, navigate to the first page suppressing the initial entrance animation.
                shell.AppFrame.Navigate(typeof(MainPage), e.Arguments, new SuppressNavigationTransitionInfo());
            }

            Window.Current.Activate();
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        private async void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            var file = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("ErrorLog.txt", Windows.Storage.CreationCollisionOption.GenerateUniqueName);
            using (var fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
            {
                using (var writer = new StreamWriter(fileStream.AsStreamForWrite()))
                {
                    await writer.WriteLineAsync(e.Exception.ToString());
                }
            }
            App.Current.Exit();
        }
    }
}
