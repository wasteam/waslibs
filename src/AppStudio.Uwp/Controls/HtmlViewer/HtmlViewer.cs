using System.Threading.Tasks;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.ApplicationModel;

namespace AppStudio.Uwp.Controls
{
    public sealed partial class HtmlViewer : Control
    {
        private WebView _webView = null;

        private ContentPresenter _header = null;
        private ContentPresenter _footer = null;
        private ContentPresenter _asideLeft = null;
        private ContentPresenter _asideRight = null;

        private RectangleGeometry _clip = null;

        private Grid _glass = null;

        private bool _isHtmlLoaded = false;

        public HtmlViewer()
        {
            this.DefaultStyleKey = typeof(HtmlViewer);
            this.Background = new SolidColorBrush(Colors.Transparent);
            if (!DesignMode.DesignModeEnabled)
            {
                this.Opacity = 0.001;
            }
            this.Loaded += OnLoaded;
            this.SizeChanged += OnSizeChanged;
            this.Unloaded += OnUnloaded;
        }

        protected override void OnApplyTemplate()
        {
            if (!DesignMode.DesignModeEnabled)
            {
                var content = base.GetTemplateChild("webViewContainer") as Grid;
                _webView = new WebView(WebViewExecutionMode.SameThread) { DefaultBackgroundColor = Colors.Transparent };
                _webView.Settings.IsJavaScriptEnabled = false;
                _webView.Settings.IsIndexedDBEnabled = false;
                content.Children.Add(_webView);
            }

            _header = base.GetTemplateChild("header") as ContentPresenter;
            _footer = base.GetTemplateChild("footer") as ContentPresenter;
            _asideLeft = base.GetTemplateChild("asideleft") as ContentPresenter;
            _asideRight = base.GetTemplateChild("asideright") as ContentPresenter;

            _clip = base.GetTemplateChild("clip") as RectangleGeometry;

            InitializeGlass();

            base.OnApplyTemplate();
        }

        private void InitializeGlass()
        {
            _glass = base.GetTemplateChild("glass") as Grid;
            _glass.ManipulationStarting += OnGlassManipulationStarting;
        }

        private long _tokenFontSize;
        private long _tokenForeground;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _tokenFontSize = this.RegisterPropertyChangedCallback(FontSizeProperty, async (s, d) => { await SetFontSize(); });
            _tokenForeground = this.RegisterPropertyChangedCallback(ForegroundProperty, async (s, d) => { await SetForeground(); });

            if (!DesignMode.DesignModeEnabled)
            {
                _webView.NavigationStarting += OnNavigationStarting;
                _webView.NavigationCompleted += OnNavigationCompleted;
                _webView.ScriptNotify += OnScriptNotify;

                _header.SizeChanged += OnComplementSizeChanged;
                _footer.SizeChanged += OnComplementSizeChanged;
                _asideLeft.SizeChanged += OnComplementSizeChanged;
                _asideRight.SizeChanged += OnComplementSizeChanged;

                _header.PointerWheelChanged += OnPointerWheelChanged;
                _header.ManipulationStarted += OnComplementManipulationStarted;
                _header.ManipulationDelta += OnComplementManipulationDelta;

                _footer.PointerWheelChanged += OnPointerWheelChanged;
                _footer.ManipulationStarted += OnComplementManipulationStarted;
                _footer.ManipulationDelta += OnComplementManipulationDelta;

                _asideLeft.PointerWheelChanged += OnPointerWheelChanged;
                _asideLeft.ManipulationStarted += OnComplementManipulationStarted;
                _asideLeft.ManipulationDelta += OnComplementManipulationDelta;

                _asideRight.PointerWheelChanged += OnPointerWheelChanged;
                _asideRight.ManipulationStarted += OnComplementManipulationStarted;
                _asideRight.ManipulationDelta += OnComplementManipulationDelta;

                if (this.Source != null)
                {
                    Navigate(this.Source);
                }
                else if (this.Html != null)
                {
                    NavigateToString(this.Html);
                }
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.UnregisterPropertyChangedCallback(FontSizeProperty, _tokenFontSize);
            this.UnregisterPropertyChangedCallback(ForegroundProperty, _tokenForeground);

            _header.SizeChanged -= OnComplementSizeChanged;
            _footer.SizeChanged -= OnComplementSizeChanged;
            _asideLeft.SizeChanged -= OnComplementSizeChanged;
            _asideRight.SizeChanged -= OnComplementSizeChanged;

            _header.PointerWheelChanged -= OnPointerWheelChanged;
            _header.ManipulationStarted -= OnComplementManipulationStarted;
            _header.ManipulationDelta -= OnComplementManipulationDelta;

            _footer.PointerWheelChanged -= OnPointerWheelChanged;
            _footer.ManipulationStarted -= OnComplementManipulationStarted;
            _footer.ManipulationDelta -= OnComplementManipulationDelta;

            _asideLeft.PointerWheelChanged -= OnPointerWheelChanged;
            _asideLeft.ManipulationStarted -= OnComplementManipulationStarted;
            _asideLeft.ManipulationDelta -= OnComplementManipulationDelta;

            _asideRight.PointerWheelChanged -= OnPointerWheelChanged;
            _asideRight.ManipulationStarted -= OnComplementManipulationStarted;
            _asideRight.ManipulationDelta -= OnComplementManipulationDelta;

            if (!DesignMode.DesignModeEnabled)
            {
                _webView.NavigationStarting -= OnNavigationStarting;
                _webView.NavigationCompleted -= OnNavigationCompleted;
                _webView.ScriptNotify -= OnScriptNotify;

                _webView.NavigateToString("");
            }
        }

        private async Task SetFontSize()
        {
            if (_isHtmlLoaded)
            {
                await _webView.InvokeScriptAsync("setFontSize", this.FontSize + "px");
            }
        }

        private async Task SetForeground()
        {
            if (_isHtmlLoaded)
            {
                var solidBrush = this.Foreground as SolidColorBrush;
                if (solidBrush != null)
                {
                    await _webView.InvokeScriptAsync("setHtmlColor", "#" + solidBrush.Color.ToString().Substring(3));
                }
            }
        }
    }
}
