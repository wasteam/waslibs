using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml.Input;

namespace AppStudio.Uwp.Controls
{
    public sealed partial class HtmlViewer : Control
    {
        private Grid _frame = null;
        private WebView _webView = null;
        private Grid _glass = null;

        private Grid _container = null;
        private ContentPresenter _header = null;
        private ContentPresenter _footer = null;
        private ContentPresenter _asideLeft = null;
        private ContentPresenter _asideRight = null;

        private RectangleGeometry _clip = null;

        private ProgressRing _progress = null;

        const double MARGIN_RIGHT = 1.0;

        public HtmlViewer()
        {
            this.DefaultStyleKey = typeof(HtmlViewer);
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        protected override void OnApplyTemplate()
        {
            _frame = base.GetTemplateChild("frame") as Grid;
            _webView = base.GetTemplateChild("webView") as WebView;
            _glass = base.GetTemplateChild("glass") as Grid;

            _container = base.GetTemplateChild("container") as Grid;
            _header = base.GetTemplateChild("header") as ContentPresenter;
            _footer = base.GetTemplateChild("footer") as ContentPresenter;
            _asideLeft = base.GetTemplateChild("asideLeft") as ContentPresenter;
            _asideRight = base.GetTemplateChild("asideRight") as ContentPresenter;

            _clip = base.GetTemplateChild("clip") as RectangleGeometry;

            _progress = base.GetTemplateChild("progress") as ProgressRing;

            _webView.NavigationStarting += OnNavigationStarting;
            _webView.NavigationCompleted += OnNavigationCompleted;
            _webView.ScriptNotify += OnScriptNotify;

            _glass.ManipulationStarting += OnGlassManipulationStarting;
            _glass.ManipulationStarted += OnGlassManipulationStarted;
            _glass.PointerPressed += OnGlassPointerPressed;

            _header.PointerWheelChanged += OnPointerWheelChanged;
            _footer.PointerWheelChanged += OnPointerWheelChanged;
            _asideLeft.PointerWheelChanged += OnPointerWheelChanged;
            _asideRight.PointerWheelChanged += OnPointerWheelChanged;

            _header.ManipulationStarted += OnAdornManipulationStarted;
            _footer.ManipulationStarted += OnAdornManipulationStarted;
            _asideLeft.ManipulationStarted += OnAdornManipulationStarted;
            _asideRight.ManipulationStarted += OnAdornManipulationStarted;

            _header.ManipulationDelta += OnAdornManipulationDelta;
            _footer.ManipulationDelta += OnAdornManipulationDelta;
            _asideLeft.ManipulationDelta += OnAdornManipulationDelta;
            _asideRight.ManipulationDelta += OnAdornManipulationDelta;

            _header.ManipulationCompleted += OnAdornManipulationCompleted;
            _footer.ManipulationCompleted += OnAdornManipulationCompleted;
            _asideLeft.ManipulationCompleted += OnAdornManipulationCompleted;
            _asideRight.ManipulationCompleted += OnAdornManipulationCompleted;

            _header.SizeChanged += AdornSizeChanged;
            _footer.SizeChanged += AdornSizeChanged;
            _asideLeft.SizeChanged += AdornSizeChanged;
            _asideRight.SizeChanged += AdornSizeChanged;

            this.SizeChanged += OnSizeChanged;

            base.OnApplyTemplate();
        }

        private long _tokenFontSize;
        private long _tokenForeground;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.Html != null)
            {
                NavigateToString(this.Html);
            }
            else if (this.Source != null)
            {
                NavigateToSource(this.Source);
            }
            else
            {
                NavigateToString("<div></div>");
            }

            _tokenFontSize = this.RegisterPropertyChangedCallback(FontSizeProperty, async (s, d) => { await SetFontSize(); });
            _tokenForeground = this.RegisterPropertyChangedCallback(ForegroundProperty, async (s, d) => { await SetForeground(); });
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.UnregisterPropertyChangedCallback(FontSizeProperty, _tokenFontSize);
            this.UnregisterPropertyChangedCallback(ForegroundProperty, _tokenForeground);

            if (_webView != null && _isHtmlLoaded)
            {
                _webView.NavigateToString(String.Empty);
            }
        }

        private async void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            await OnControlResize();
            _clip.Rect = new Rect(0, 0, _webView.ActualWidth - 12.0, _webView.ActualHeight);
        }

        private async void AdornSizeChanged(object sender, SizeChangedEventArgs e)
        {
            await OnAdornResize();
        }
    }
}
