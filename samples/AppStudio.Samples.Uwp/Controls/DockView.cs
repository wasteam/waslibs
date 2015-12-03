using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Samples.Uwp.Controls
{
    public sealed class DockView : Control
    {
        private Button _button = null;
        private SplitView _splitView = null;

        public DockView()
        {
            this.DefaultStyleKey = typeof(DockView);
        }

        #region Pane
        public UIElement Pane
        {
            get { return (UIElement)GetValue(PaneProperty); }
            set { SetValue(PaneProperty, value); }
        }

        public static readonly DependencyProperty PaneProperty = DependencyProperty.Register("Pane", typeof(UIElement), typeof(DockView), new PropertyMetadata(null));
        #endregion

        #region Content
        public UIElement Content
        {
            get { return (UIElement)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(UIElement), typeof(DockView), new PropertyMetadata(null));
        #endregion

        protected override void OnApplyTemplate()
        {
            _splitView = base.GetTemplateChild("splitView") as SplitView;
            _button = base.GetTemplateChild("button") as Button;
            _button.Click += OnClick;

            base.OnApplyTemplate();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            _splitView.IsPaneOpen = !_splitView.IsPaneOpen;
        }
    }
}
