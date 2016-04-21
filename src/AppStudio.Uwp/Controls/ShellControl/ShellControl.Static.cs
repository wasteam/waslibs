using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    partial class ShellControl
    {
        private CommandBar _commandBar = null;
        private UIElement _paneHeader = null;

        public static ShellControl Current { get; private set; }

        #region CommandBar
        public static CommandBar GetCommandBar(DependencyObject obj)
        {
            return (CommandBar)obj.GetValue(CommandBarProperty);
        }

        public static void SetCommandBar(DependencyObject obj, CommandBar value)
        {
            obj.SetValue(CommandBarProperty, value);
        }

        private static void CommandBarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (Current != null)
            {
                var commandBar = e.NewValue as CommandBar;
                Current.SetCommandBar(commandBar);
            }
        }

        public void SetCommandBar(CommandBar commandBar)
        {
            _commandBar = commandBar;
            if (_isInitialized)
            {
                _commandBarContainer.Content = _commandBar;
                SetCommandBarVerticalAlignment(this.CommandBarVerticalAlignment);
            }
        }

        public static readonly DependencyProperty CommandBarProperty = DependencyProperty.RegisterAttached("CommandBar", typeof(CommandBar), typeof(ShellControl), new PropertyMetadata(null, CommandBarChanged));
        #endregion

        #region PaneHeader
        public static UIElement GetPaneHeader(DependencyObject obj)
        {
            return (UIElement)obj.GetValue(PaneHeaderProperty);
        }

        public static void SetPaneHeader(DependencyObject obj, UIElement value)
        {
            obj.SetValue(PaneHeaderProperty, value);
        }

        public static readonly DependencyProperty PaneHeaderProperty = DependencyProperty.RegisterAttached("PaneHeader", typeof(UIElement), typeof(ShellControl), new PropertyMetadata(null, PaneHeaderChanged));

        private static void PaneHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (Current != null)
            {
                var paneHeader = e.NewValue as UIElement;
                Current.SetPaneHeader(paneHeader);
            }
        }

        private void SetPaneHeader(UIElement paneHeader)
        {
            _paneHeader = paneHeader;
            if (_isInitialized)
            {
                _paneHeaderContainer.Content = _paneHeader;
            }
        }
        #endregion

        public static void ClearCommandBar()
        {
            Current.SetCommandBar(null);
        }
    }
}
