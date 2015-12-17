using System;
using System.Linq;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace AppStudio.Uwp.Controls
{
    public sealed partial class HtmlViewer : Control
    {
        private Grid _frame = null;
        private WebView _webView = null;

        private Grid _container = null;
        private ContentPresenter _header = null;
        private ContentPresenter _footer = null;
        private ContentPresenter _asideLeft = null;
        private ContentPresenter _asideRight = null;

        private RectangleGeometry _clip = null;

        private ProgressBar _progress = null;

        const double MARGIN_RIGHT = 14.0;

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

            _container = base.GetTemplateChild("container") as Grid;
            _header = base.GetTemplateChild("header") as ContentPresenter;
            _footer = base.GetTemplateChild("footer") as ContentPresenter;
            _asideLeft = base.GetTemplateChild("asideLeft") as ContentPresenter;
            _asideRight = base.GetTemplateChild("asideRight") as ContentPresenter;

            _clip = base.GetTemplateChild("clip") as RectangleGeometry;

            _progress = base.GetTemplateChild("progress") as ProgressBar;

            _webView.NavigationStarting += OnNavigationStarting;
            _webView.NavigationCompleted += OnNavigationCompleted;
            _webView.ScriptNotify += OnScriptNotify;

            _header.PointerWheelChanged += OnPointerWheelChanged;
            _footer.PointerWheelChanged += OnPointerWheelChanged;
            _asideLeft.PointerWheelChanged += OnPointerWheelChanged;
            _asideRight.PointerWheelChanged += OnPointerWheelChanged;

            _header.ManipulationDelta += OnManipulationDelta;
            _footer.ManipulationDelta += OnManipulationDelta;
            _asideLeft.ManipulationDelta += OnManipulationDelta;
            _asideRight.ManipulationDelta += OnManipulationDelta;

            if (this.Html != null)
            {
                NavigateToString(this.Html);
            }
            else if (this.Source != null)
            {
                NavigateToSource(this.Source);
            }

            this.SizeChanged += OnSizeChanged;

            base.OnApplyTemplate();
        }

        private long _tokenFontSize;
        private long _tokenForeground;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
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
            _clip.Rect = new Rect(0, 0, this.ActualWidth - 1, this.ActualHeight);
        }
    }
}
