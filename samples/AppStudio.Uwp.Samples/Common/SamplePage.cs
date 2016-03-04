using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;
using Windows.Storage;

using AppStudio.Uwp.Samples.Extensions;

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

        protected bool ShowSettings { get; set; }
        protected bool ShowXaml { get; set; }
        protected bool ShowCode { get; set; }
        protected bool ShowJson { get; set; }

        #region PrimaryCommands
        public IEnumerable<ICommandBarElement> PrimaryCommands
        {
            get { return (IEnumerable<ICommandBarElement>)GetValue(PrimaryCommandsProperty); }
            set { SetValue(PrimaryCommandsProperty, value); }
        }

        public static readonly DependencyProperty PrimaryCommandsProperty = DependencyProperty.Register("PrimaryCommands", typeof(IEnumerable<ICommandBarElement>), typeof(SamplePage), new PropertyMetadata(null));
        #endregion

        #region SecondaryCommands
        public IEnumerable<ICommandBarElement> SecondaryCommands
        {
            get { return (IEnumerable<ICommandBarElement>)GetValue(SecondaryCommandsProperty); }
            set { SetValue(SecondaryCommandsProperty, value); }
        }

        public static readonly DependencyProperty SecondaryCommandsProperty = DependencyProperty.Register("SecondaryCommands", typeof(IEnumerable<ICommandBarElement>), typeof(SamplePage), new PropertyMetadata(null));
        #endregion

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.ShowSettings = IsTypePresent($"AppStudio.Uwp.Samples.{SampleName}Settings");

            this.ShowXaml = await ContentFileExists($"Pages\\{SampleName}\\Docs", $"{SampleName}Xaml.xml");
            this.ShowCode = await ContentFileExists($"Pages\\{SampleName}\\Docs", $"{SampleName}CSharp.cs");
            this.ShowJson = await ContentFileExists($"Pages\\{SampleName}\\Docs", $"{SampleName}Json.json");

            this.PrimaryCommands = CreatePrimaryCommands().ToArray();
            this.SecondaryCommands = CreateSecondaryCommands().ToArray();

            base.OnNavigatedTo(e);
        }

        protected virtual IEnumerable<ICommandBarElement> CreatePrimaryCommands()
        {
            yield return CreateAppBarToggleButton(Symbol.Help, this.GetResourceString("AppBarButtonHelp"), OnHelpButton);

            if (ShowSettings)
            {
                yield return CreateAppBarToggleButton(Symbol.Setting, this.GetResourceString("AppBarButtonSettings"), OnSettingsButton);
            }

            if (ShowXaml || ShowCode || ShowJson)
            {
                yield return new AppBarSeparator();
                if (ShowXaml)
                {
                    yield return CreateAppBarToggleButton(new Uri("ms-appx:///Assets/Icons/Xaml.png"), this.GetResourceString("AppBarButtonXamlCode"), OnXamlCodeButton);
                }
                if (ShowCode)
                {
                    yield return CreateAppBarToggleButton(new Uri("ms-appx:///Assets/Icons/CSharp.png"), this.GetResourceString("AppBarButtonSourceCode"), OnSourceCodeButton);
                }
                if (ShowJson)
                {
                    yield return CreateAppBarToggleButton(new Uri("ms-appx:///Assets/Icons/Json.png"), this.GetResourceString("AppBarButtonJsonData"), OnJsonButton);
                }
            }
        }

        protected virtual IEnumerable<ICommandBarElement> CreateSecondaryCommands()
        {
            yield break;
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
            string typeName = $"AppStudio.Uwp.Samples.{SampleName}Help";
            if (IsTypePresent(typeName))
            {
                AppShell.Current.Shell.ShowTopPane(Activator.CreateInstance(Type.GetType(typeName)) as Control);
            }
            else
            {
                var control = new PrettifyControl();
                AppShell.Current.Shell.ShowTopPane(control);
                control.HtmlSource = await ReadContent(new Uri($"ms-appx:///Pages/{SampleName}/Docs/{SampleName}Help.html"));
            }
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
        protected virtual void OnSettings() { }

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

            var control = new PrettifyControl();
            control.CSharpSource = await ReadContent(new Uri($"ms-appx:///Pages/{SampleName}/Docs/{SampleName}CSharp.cs"));
            this.Content = control;
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

            var control = new PrettifyControl();
            control.XamlSource = await ReadContent(new Uri($"ms-appx:///Pages/{SampleName}/Docs/{SampleName}Xaml.xml"));
            this.Content = control;
        }

        private void OnJsonButton(object sender, RoutedEventArgs e)
        {
            var button = sender as AppBarToggleButton;
            if (button.IsChecked.Value)
            {
                _restoreContent = false;
                this.ReleaseToggleButtons(button);
                OnJson();
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
        protected virtual async void OnJson()
        {
            await this.Content.FadeOutAsync(100);

            var control = new PrettifyControl();
            control.JsonSource = await ReadContent(new Uri($"ms-appx:///Pages/{SampleName}/Docs/{SampleName}Json.json"));
            this.Content = control;
        }

        #region AppBarButton Helpers
        protected ICommandBarElement CreateAppBarButton(Symbol symbol, string label, RoutedEventHandler eventHandler)
        {
            return CreateAppBarButton(new SymbolIcon(symbol), label, eventHandler);
        }
        protected ICommandBarElement CreateAppBarButton(string glyph, string label, RoutedEventHandler eventHandler)
        {
            return CreateAppBarButton(new FontIcon() { Glyph = glyph, FontFamily = new FontFamily("Segoe MDL2 Assets") }, label, eventHandler);
        }
        protected ICommandBarElement CreateAppBarButton(Uri uriSource, string label, RoutedEventHandler eventHandler)
        {
            return CreateAppBarButton(new BitmapIcon { UriSource = uriSource }, label, eventHandler);
        }
        protected ICommandBarElement CreateAppBarButton(IconElement icon, string label, RoutedEventHandler eventHandler)
        {
            var command = CreateAppBarButton(label, eventHandler) as AppBarButton;
            command.Icon = icon;
            return command;
        }
        protected ICommandBarElement CreateAppBarButton(string label, RoutedEventHandler eventHandler)
        {
            var command = new AppBarButton { Label = label };
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
        protected ICommandBarElement CreateAppBarToggleButton(Uri uriSource, string label, RoutedEventHandler eventHandler)
        {
            return CreateAppBarToggleButton(new BitmapIcon { UriSource = uriSource }, label, eventHandler);
        }
        protected ICommandBarElement CreateAppBarToggleButton(IconElement icon, string label, RoutedEventHandler eventHandler)
        {
            var command = CreateAppBarToggleButton(label, eventHandler) as AppBarToggleButton;
            command.Icon = icon;
            return command;
        }
        protected ICommandBarElement CreateAppBarToggleButton(string label, RoutedEventHandler eventHandler)
        {
            var command = new AppBarToggleButton { Label = label };
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

        #region ContentFileExists
        private async Task<bool> ContentFileExists(string path, string fileName)
        {
            var InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var docsFolder = await InstallationFolder.GetFolderAsync(path);
            return await docsFolder.TryGetItemAsync(fileName) != null;
        }
        #endregion

        #region ReadContent
        private async Task<string> ReadContent(Uri uri)
        {
            var docFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
            return await docFile.ReadTextAsync();
        }
        #endregion

        #region IsTypePresent
        private bool IsTypePresent(string typeName)
        {
            return Type.GetType(typeName, false) != null;
        }
        #endregion        
    }
}
