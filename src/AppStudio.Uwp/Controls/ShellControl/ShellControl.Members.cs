using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    partial class ShellControl
    {
        #region Header
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(ShellControl), new PropertyMetadata(null));
        #endregion

        #region HeaderTemplate
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(ShellControl), new PropertyMetadata(null));
        #endregion

        #region NavigationItemTemplate
        public DataTemplate NavigationItemTemplate
        {
            get { return (DataTemplate)GetValue(NavigationItemTemplateProperty); }
            set { SetValue(NavigationItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty NavigationItemTemplateProperty = DependencyProperty.Register("NavigationItemTemplate", typeof(DataTemplate), typeof(ShellControl), new PropertyMetadata(null));
        #endregion

        #region NavigationSubItemTemplate
        public DataTemplate NavigationSubItemTemplate
        {
            get { return (DataTemplate)GetValue(NavigationSubItemTemplateProperty); }
            set { SetValue(NavigationSubItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty NavigationSubItemTemplateProperty = DependencyProperty.Register("NavigationSubItemTemplate", typeof(DataTemplate), typeof(ShellControl), new PropertyMetadata(null));
        #endregion

        #region SeparatorStyle
        public Style SeparatorStyle
        {
            get { return (Style)GetValue(SeparatorStyleProperty); }
            set { SetValue(SeparatorStyleProperty, value); }
        }

        public static readonly DependencyProperty SeparatorStyleProperty = DependencyProperty.Register("SeparatorStyle", typeof(Style), typeof(ShellControl), new PropertyMetadata(null));
        #endregion

        #region CommandBarBackground
        public Brush CommandBarBackground
        {
            get { return (Brush)GetValue(CommandBarBackgroundProperty); }
            set { SetValue(CommandBarBackgroundProperty, value); }
        }

        public static readonly DependencyProperty CommandBarBackgroundProperty = DependencyProperty.Register("CommandBarBackground", typeof(Brush), typeof(ShellControl), new PropertyMetadata(null));
        #endregion

        #region CommandBarForeground
        public Brush CommandBarForeground
        {
            get { return (Brush)GetValue(CommandBarForegroundProperty); }
            set { SetValue(CommandBarForegroundProperty, value); }
        }

        public static readonly DependencyProperty CommandBarForegroundProperty = DependencyProperty.Register("CommandBarForeground", typeof(Brush), typeof(ShellControl), new PropertyMetadata(null));
        #endregion

        #region HamburgerBackground
        public Brush HamburgerBackground
        {
            get { return (Brush)GetValue(HamburgerBackgroundProperty); }
            set { SetValue(HamburgerBackgroundProperty, value); }
        }

        public static readonly DependencyProperty HamburgerBackgroundProperty = DependencyProperty.Register("HamburgerBackground", typeof(Brush), typeof(ShellControl), new PropertyMetadata(null));
        #endregion

        #region HamburgerForeground
        public Brush HamburgerForeground
        {
            get { return (Brush)GetValue(HamburgerForegroundProperty); }
            set { SetValue(HamburgerForegroundProperty, value); }
        }

        public static readonly DependencyProperty HamburgerForegroundProperty = DependencyProperty.Register("HamburgerForeground", typeof(Brush), typeof(ShellControl), new PropertyMetadata(null));
        #endregion

        #region HamburgerBorderBrush
        public Brush HamburgerBorderBrush
        {
            get { return (Brush)GetValue(HamburgerBorderBrushProperty); }
            set { SetValue(HamburgerBorderBrushProperty, value); }
        }

        public static readonly DependencyProperty HamburgerBorderBrushProperty = DependencyProperty.Register("HamburgerBorderBrush", typeof(Brush), typeof(ShellControl), new PropertyMetadata(null));
        #endregion

        #region NavigationBackground
        public Brush NavigationBackground
        {
            get { return (Brush)GetValue(NavigationBackgroundProperty); }
            set { SetValue(NavigationBackgroundProperty, value); }
        }

        public static readonly DependencyProperty NavigationBackgroundProperty = DependencyProperty.Register("NavigationBackground", typeof(Brush), typeof(ShellControl), new PropertyMetadata(null));
        #endregion

        #region NavigationForeground
        public Brush NavigationForeground
        {
            get { return (Brush)GetValue(NavigationForegroundProperty); }
            set { SetValue(NavigationForegroundProperty, value); }
        }

        public static readonly DependencyProperty NavigationForegroundProperty = DependencyProperty.Register("NavigationForeground", typeof(Brush), typeof(ShellControl), new PropertyMetadata(null));
        #endregion

        #region DisplayMode
        public SplitViewDisplayMode DisplayMode
        {
            get { return (SplitViewDisplayMode)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }

        public static readonly DependencyProperty DisplayModeProperty = DependencyProperty.Register("DisplayMode", typeof(SplitViewDisplayMode), typeof(ShellControl), new PropertyMetadata(SplitViewDisplayMode.CompactOverlay));
        #endregion

        #region TopPaneHeight
        public double TopPaneHeight
        {
            get { return (double)GetValue(TopPaneHeightProperty); }
            set { SetValue(TopPaneHeightProperty, value); }
        }

        private static void TopPaneHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ShellControl;
            control.SetTopPaneHeight();
        }

        private void SetTopPaneHeight()
        {
            if (_topPane.GetTranslateX() < 0)
            {
                _topPane.TranslateY(-this.TopPaneHeight);
            }
        }

        public static readonly DependencyProperty TopPaneHeightProperty = DependencyProperty.Register("TopPaneHeight", typeof(double), typeof(ShellControl), new PropertyMetadata(360.0, TopPaneHeightChanged));
        #endregion

        #region RightPaneWidth
        public double RightPaneWidth
        {
            get { return (double)GetValue(RightPaneWidthProperty); }
            set { SetValue(RightPaneWidthProperty, value); }
        }

        private static void RightPaneWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ShellControl;
            control.SetRightPaneWidth();
        }

        private void SetRightPaneWidth()
        {
            if (_rightPane.GetTranslateX() > 0)
            {
                _rightPane.TranslateX(this.RightPaneWidth);
            }
        }

        public static readonly DependencyProperty RightPaneWidthProperty = DependencyProperty.Register("RightPaneWidth", typeof(double), typeof(ShellControl), new PropertyMetadata(360.0, RightPaneWidthChanged));
        #endregion

        public void OpenLeftPane()
        {
            _splitView.OpenPaneLength = this.ActualWidth > 640 ? 360 : 320;
            _splitView.IsPaneOpen = true;
        }

        public void CloseLeftPane()
        {
            _splitView.IsPaneOpen = false;
        }

        public void ShowTopPane(FrameworkElement content)
        {
            _splitView.IsPaneOpen = false;
            if (_isInitialized && _topPane.Visibility == Visibility.Collapsed)
            {
                _topPane.TranslateY(-this.TopPaneHeight);
                _topPane.Content = content;
                _topPane.Visibility = Visibility.Visible;
                _topPane.AnimateY(0, 350);
            }
        }
        public async void HideTopPane()
        {
            if (_isInitialized && _topPane.Visibility == Visibility.Visible)
            {
                await _topPane.AnimateYAsync(-_topPane.ActualHeight, 350);
                _topPane.Content = null;
                _topPane.Visibility = Visibility.Collapsed;
            }
        }

        public void ShowRightPane(FrameworkElement content)
        {
            _splitView.IsPaneOpen = false;
            if (_isInitialized && _rightPane.Visibility == Visibility.Collapsed)
            {
                _rightPane.TranslateX(this.RightPaneWidth);
                _rightPane.Content = content;
                _rightPane.Visibility = Visibility.Visible;
                _rightPane.AnimateX(0, 350);
            }
        }
        public async void HideRightPane()
        {
            if (_isInitialized && _rightPane.Visibility == Visibility.Visible)
            {
                await _rightPane.AnimateXAsync(_rightPane.ActualWidth, 350);
                _rightPane.Content = null;
                _rightPane.Visibility = Visibility.Collapsed;
            }
        }
    }
}
