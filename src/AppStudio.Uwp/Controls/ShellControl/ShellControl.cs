using System.Threading.Tasks;

using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    public sealed partial class ShellControl : ContentControl
    {
        private Button _toggle = null;
        private Button _exitFS = null;
        private SplitView _splitView = null;
        private CommandBar _commandBarT = null;
        private CommandBar _commandBarB = null;
        private Panel _paneContent = null;
        private ListView _lview = null;
        private ListView _lviewSub = null;
        private Panel _container = null;
        private Panel _content = null;

        private Panel _panes = null;
        private ContentPresenter _presenter = null;
        private ContentControl _topPane = null;
        private ContentControl _rightPane = null;

        private bool _isInitialized = false;

        public ShellControl()
        {
            this.DefaultStyleKey = typeof(ShellControl);
        }

        #region FullScreen
        public async void EnterFullScreen()
        {
            await Task.Delay(100);
            ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
            _splitView.AnimateDoubleProperty("CompactPaneLength", 48, 0, 250);
            _commandBarT.ClosedDisplayMode = AppBarClosedDisplayMode.Hidden;
            _commandBarB.ClosedDisplayMode = AppBarClosedDisplayMode.Hidden;
            _toggle.Visibility = Visibility.Collapsed;
            _exitFS.Visibility = Visibility.Visible;
            _panes.Margin = new Thickness(0);
        }

        public void ExitFullScreen()
        {
            ApplicationView.GetForCurrentView().ExitFullScreenMode();
            _splitView.AnimateDoubleProperty("CompactPaneLength", 0, 48, 250);
            _lview.AnimateDoubleProperty("Width", 0, 48, 250);
            _commandBarT.ClosedDisplayMode = AppBarClosedDisplayMode.Compact;
            _commandBarB.ClosedDisplayMode = AppBarClosedDisplayMode.Compact;
            _toggle.Visibility = Visibility.Visible;
            _exitFS.Visibility = Visibility.Collapsed;
            _panes.Margin = new Thickness(0, 48, 0, 0);
        }
        #endregion

        #region ClearSelection
        public void ClearSelection()
        {
            _currentItem = null;
            _content.Children.Clear();
            _container.Visibility = Visibility.Collapsed;
            _lview.SelectedItem = null;
            _lviewSub.SelectedItem = null;
        }
        #endregion

        protected override void OnApplyTemplate()
        {
            _toggle = base.GetTemplateChild("toggle") as Button;
            _exitFS = base.GetTemplateChild("exitFS") as Button;
            _splitView = base.GetTemplateChild("splitView") as SplitView;
            _commandBarT = base.GetTemplateChild("commandBarT") as CommandBar;
            _commandBarB = base.GetTemplateChild("commandBarB") as CommandBar;
            _paneContent = base.GetTemplateChild("paneContent") as Panel;
            _lview = base.GetTemplateChild("lview") as ListView;
            _lviewSub = base.GetTemplateChild("lviewSub") as ListView;
            _container = base.GetTemplateChild("container") as Panel;
            _content = base.GetTemplateChild("content") as Panel;
            _panes = base.GetTemplateChild("panes") as Panel;
            _presenter = base.GetTemplateChild("presenter") as ContentPresenter;
            _topPane = base.GetTemplateChild("topPane") as ContentControl;
            _rightPane = base.GetTemplateChild("rightPane") as ContentControl;

            _lview.ItemContainerStyleSelector = new NavigationStyleSelector(_lview.ItemContainerStyle, this.SeparatorStyle);
            _lview.ItemContainerStyle = null;
            _lviewSub.ItemContainerStyleSelector = new NavigationStyleSelector(_lview.ItemContainerStyle, this.SeparatorStyle);
            _lviewSub.ItemContainerStyle = null;

            _toggle.Click += OnToggleClick;
            _exitFS.Click += OnExitFSClick;
            _splitView.PaneClosed += OnPaneClosed;
            _lview.ItemClick += OnItemClick;
            _lviewSub.ItemClick += OnItemClick;
            _lview.SelectionChanged += OnSelectionChanged;

            _isInitialized = true;

            this.ArrangeCommands();

            this.SizeChanged += OnSizeChanged;

            base.OnApplyTemplate();
        }

        private void OnToggleClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _splitView.IsPaneOpen = !_splitView.IsPaneOpen;
            if (_splitView.IsPaneOpen)
            {
                _lview.Width = _splitView.OpenPaneLength;
                this.ReleaseCommands();
            }
        }

        private void OnExitFSClick(object sender, RoutedEventArgs e)
        {
            this.ExitFullScreen();
        }

        private void OnPaneClosed(SplitView sender, object args)
        {
            _lview.Width = _splitView.CompactPaneLength;
            var navItem = _lview.SelectedItem as NavigationItem;
            if (navItem != null)
            {
                if (navItem.ClearSelection)
                {
                    _lview.SelectedItem = null;
                }
                _lviewSub.SelectedItem = null;
            }
            _currentItem = null;
            _content.Children.Clear();
            _container.Visibility = Visibility.Collapsed;
            this.DisplayMode = this.ActualWidth > 640 ? SplitViewDisplayMode.CompactOverlay : SplitViewDisplayMode.Overlay;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _splitView.OpenPaneLength = e.NewSize.Width > 640 ? 360 : 320;
            this.DisplayMode = e.NewSize.Width > 640 ? SplitViewDisplayMode.CompactOverlay : SplitViewDisplayMode.Overlay;
            this.CommandBarAlignment = e.NewSize.Width > 640 ? CommandBarAlignment.Top : CommandBarAlignment.Bottom;
        }
    }
}
