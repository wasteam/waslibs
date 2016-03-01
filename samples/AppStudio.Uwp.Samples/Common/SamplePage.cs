using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;
using Windows.Storage;

namespace AppStudio.Uwp.Samples
{
    abstract public class SamplePage : Page
    {
        private bool _restoreContent = true;
        private UIElement _content = null;

        public SamplePage()
        {
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _content = this.Content;
        }

        abstract public string Caption { get; }

        protected string SampleName
        {
            get { return this.GetType().Name.Substring(0, this.GetType().Name.Length - 4); }
        }

        #region PrimaryCommands
        public IEnumerable<ICommandBarElement> PrimaryCommands
        {
            get { return (IEnumerable<ICommandBarElement>)GetValue(PrimaryCommandsProperty); }
            set { SetValue(PrimaryCommandsProperty, value); }
        }

        public static readonly DependencyProperty PrimaryCommandsProperty = DependencyProperty.Register("PrimaryCommands", typeof(IEnumerable<ICommandBarElement>), typeof(SamplePage), new PropertyMetadata(null));
        #endregion

        private bool _xamlExists = false;
        private bool _codeExists = false;

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            _xamlExists = await DocumentFileExists($"{SampleName}Xaml.xml");
            _codeExists = await DocumentFileExists($"{SampleName}CSharp.cs");

            this.PrimaryCommands = CreatePrimaryCommands().ToArray();

            base.OnNavigatedTo(e);
        }

        protected virtual IEnumerable<ICommandBarElement> CreatePrimaryCommands()
        {
            yield return CreateAppBarToggleButton(Symbol.Help, "Help", OnHelpButton);
            yield return CreateAppBarToggleButton(Symbol.Setting, "Settings", OnSettingsButton);

            if (_xamlExists || _codeExists)
            {
                yield return new AppBarSeparator();
            }
            if (_xamlExists)
            {
                yield return CreateAppBarToggleButton(new Uri("ms-appx:///Assets/Icons/Xaml.png"), "Xaml Code", OnXamlCodeButton);
            }
            if (_codeExists)
            {
                yield return CreateAppBarToggleButton(new Uri("ms-appx:///Assets/Icons/CSharp.png"), "Source Code", OnSourceCodeButton);
            }
        }

        private void OnHelpButton(object sender, RoutedEventArgs e)
        {
            var button = sender as AppBarToggleButton;
            if (button.IsChecked.Value)
            {
                this.ReleaseToggleButtons(button);
                OnHelp();
            }
            else
            {
                AppShell.Current.Shell.HideTopPane();
            }
        }
        protected virtual async void OnHelp()
        {
            var control = new DocumentControl();
            AppShell.Current.Shell.ShowTopPane(control);
            await control.ShowHelp(new Uri($"ms-appx:///Pages/{SampleName}/Docs/{SampleName}Help.html"));
        }

        private void OnSettingsButton(object sender, RoutedEventArgs e)
        {
            var button = sender as AppBarToggleButton;
            if (button.IsChecked.Value)
            {
                this.ReleaseToggleButtons(button);
                OnSettings();
            }
            else
            {
                AppShell.Current.Shell.HideRightPane();
            }
        }
        protected abstract void OnSettings();

        private void OnSourceCodeButton(object sender, RoutedEventArgs e)
        {
            var button = sender as AppBarToggleButton;
            if (button.IsChecked.Value)
            {
                _restoreContent = false;
                this.ReleaseToggleButtons(button);
                OnSourceCode();
                _restoreContent = true;
            }
            else
            {
                if (_restoreContent)
                {
                    this.Content = _content;
                    this.Content.FadeIn();
                }
            }
        }
        protected virtual async void OnSourceCode()
        {
            await this.Content.FadeOutAsync(100);

            var control = new DocumentControl { Opacity = 0.0 };
            await control.ShowCSharp(new Uri($"ms-appx:///Pages/{SampleName}/Docs/{SampleName}CSharp.cs"));

            this.Content = control;
            control.FadeIn(100);
        }

        private void OnXamlCodeButton(object sender, RoutedEventArgs e)
        {
            var button = sender as AppBarToggleButton;
            if (button.IsChecked.Value)
            {
                _restoreContent = false;
                this.ReleaseToggleButtons(button);
                OnXamlCode();
                _restoreContent = true;
            }
            else
            {
                if (_restoreContent)
                {
                    this.Content = _content;
                    this.Content.FadeIn();
                }
            }
        }
        protected virtual async void OnXamlCode()
        {
            await this.Content.FadeOutAsync(100);

            var control = new DocumentControl { Opacity = 0.0 };
            await control.ShowXaml(new Uri($"ms-appx:///Pages/{SampleName}/Docs/{SampleName}Xaml.xml"));

            this.Content = control;
            control.FadeIn(100);
        }

        #region AppBarButton Helpers
        protected ICommandBarElement CreateAppBarButton(Symbol symbol, string label, RoutedEventHandler eventHandler)
        {
            var command = new AppBarButton { Icon = new SymbolIcon(symbol), Label = label };
            ToolTipService.SetToolTip(command, label);
            command.Click += eventHandler;
            return command;
        }

        protected ICommandBarElement CreateAppBarToggleButton(Symbol symbol, string label, RoutedEventHandler eventHandler)
        {
            return CreateAppBarToggleButton(new SymbolIcon(symbol), label, eventHandler);
        }
        protected ICommandBarElement CreateAppBarToggleButton(string glyph, string label, RoutedEventHandler eventHandler)
        {
            return CreateAppBarToggleButton(new FontIcon() { Glyph = glyph, FontFamily = new FontFamily("Segoe MDL2 Assets") }, label, eventHandler);
        }
        protected ICommandBarElement CreateAppBarToggleButton(Uri UriSource, string label, RoutedEventHandler eventHandler)
        {
            return CreateAppBarToggleButton(new BitmapIcon { UriSource = UriSource }, label, eventHandler);
        }
        protected ICommandBarElement CreateAppBarToggleButton(IconElement icon, string label, RoutedEventHandler eventHandler)
        {
            var command = new AppBarToggleButton { Icon = icon, Label = label };
            ToolTipService.SetToolTip(command, label);
            command.Checked += eventHandler;
            command.Unchecked += eventHandler;
            return command;
        }

        private void ReleaseToggleButtons(AppBarToggleButton skip)
        {
            var items = PrimaryCommands.Where(r => r != skip && r.GetType() == typeof(AppBarToggleButton)).Cast<AppBarToggleButton>();
            foreach (var item in items)
            {
                item.IsChecked = false;
            }
        }
        #endregion

        #region DocumentFileExists
        private async Task<bool> DocumentFileExists(string fileName)
        {
            var InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var docsFolder = await InstallationFolder.GetFolderAsync($"Pages\\{SampleName}\\Docs");
            return await docsFolder.TryGetItemAsync(fileName) != null;
        }
        #endregion
    }
}
