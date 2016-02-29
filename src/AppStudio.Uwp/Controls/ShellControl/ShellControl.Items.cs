using System.Linq;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    #region CommandBarAlignment
    public enum CommandBarAlignment
    {
        Top,
        Bottom
    }
    #endregion

    partial class ShellControl
    {
        #region NavigationItems
        public IEnumerable<NavigationItem> NavigationItems
        {
            get { return (IEnumerable<NavigationItem>)GetValue(NavigationItemsProperty); }
            set { SetValue(NavigationItemsProperty, value); }
        }

        public static readonly DependencyProperty NavigationItemsProperty = DependencyProperty.Register("NavigationItems", typeof(IEnumerable<NavigationItem>), typeof(ShellControl), new PropertyMetadata(null));
        #endregion

        #region PrimaryCommands
        public IEnumerable<ICommandBarElement> PrimaryCommands
        {
            get { return (IEnumerable<ICommandBarElement>)GetValue(PrimaryCommandsProperty); }
            set { SetValue(PrimaryCommandsProperty, value); }
        }

        private static void PrimaryCommandsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ShellControl;
            control.ArrangeCommands();
        }

        public static readonly DependencyProperty PrimaryCommandsProperty = DependencyProperty.Register("PrimaryCommands", typeof(IEnumerable<ICommandBarElement>), typeof(ShellControl), new PropertyMetadata(null, PrimaryCommandsChanged));
        #endregion

        #region CommandBarAlignment
        public CommandBarAlignment CommandBarAlignment
        {
            get { return (CommandBarAlignment)GetValue(CommandBarAlignmentProperty); }
            set { SetValue(CommandBarAlignmentProperty, value); }
        }

        private static void CommandBarAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ShellControl;
            control.ArrangeCommands();
        }

        public static readonly DependencyProperty CommandBarAlignmentProperty = DependencyProperty.Register("CommandBarAlignment", typeof(CommandBarAlignment), typeof(ShellControl), new PropertyMetadata(CommandBarAlignment.Top, CommandBarAlignmentChanged));
        #endregion

        private void ArrangeCommands()
        {
            if (_isInitialized)
            {
                _commandBarT.PrimaryCommands.Clear();
                _commandBarB.PrimaryCommands.Clear();
                if (this.CommandBarAlignment == CommandBarAlignment.Top)
                {
                    if (this.PrimaryCommands != null)
                    {
                        foreach (var item in this.PrimaryCommands)
                        {
                            _commandBarT.PrimaryCommands.Add(item);
                        }
                    }
                    _commandBarT.Margin = new Thickness(48, 0, 0, 0);
                    _commandBarB.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (this.PrimaryCommands != null)
                    {
                        foreach (var item in this.PrimaryCommands)
                        {
                            _commandBarB.PrimaryCommands.Add(item);
                        }
                    }
                    _commandBarT.Margin = new Thickness(48, 0, -48, 0);
                    _commandBarB.Visibility = Visibility.Visible;
                }
            }
        }

        private void ReleaseCommands()
        {
            if (this.PrimaryCommands != null)
            {
                foreach (var item in this.PrimaryCommands.Where(r => r.GetType() == typeof(AppBarToggleButton)).Cast<AppBarToggleButton>())
                {
                    item.IsChecked = false;
                }
            }
        }
    }
}
