using System;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    public partial class ShellControl : ContentControl
    {
        private SplitView _splitView = null;
        private Button _exitFS = null;
        private Button _toggle = null;
        private Panel _headerContainer = null;
        private ContentControl _commandBarContainer = null;
        private ContentControl _paneHeaderContainer = null;
        private ListView _lview = null;
        private ListView _lviewSub = null;
        private Panel _container = null;
        private Panel _content = null;

        private ContentControl _topPane = null;
        private ContentControl _rightPane = null;

        private bool _isInitialized = false;

        public ShellControl()
        {
            Current = this;
            this.DefaultStyleKey = typeof(ShellControl);
        }

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
            _splitView = base.GetTemplateChild("splitView") as SplitView;
            _toggle = base.GetTemplateChild("toggle") as Button;
            _exitFS = base.GetTemplateChild("exitFS") as Button;
            _headerContainer = base.GetTemplateChild("headerContainer") as Panel;
            _commandBarContainer = base.GetTemplateChild("commandBarContainer") as ContentControl;
            _paneHeaderContainer = base.GetTemplateChild("paneHeaderContainer") as ContentControl;
            _lview = base.GetTemplateChild("lview") as ListView;
            _lviewSub = base.GetTemplateChild("lviewSub") as ListView;
            _container = base.GetTemplateChild("container") as Panel;
            _content = base.GetTemplateChild("content") as Panel;

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

            this.SelectFirstNavigationItem();

            SetDisplayMode(this.DisplayMode);
            SetCommandBar(_commandBar);
            SetPaneHeader(_paneHeader);

            base.OnApplyTemplate();
        }

        private void OnToggleClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_splitView.IsPaneOpen)
            {
                CloseLeftPane();
            }
            else
            {
                OpenLeftPane();
            }
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

        private async void OnPaneClosed(SplitView sender, object args)
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
            // TODOX: 
            //_currentItem = null;
            _content.Children.Clear();
            _container.Visibility = Visibility.Collapsed;

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                await Task.Delay(100);
                _lview.SelectedIndex = _selectedIndex;
            });
        }
    }
}
