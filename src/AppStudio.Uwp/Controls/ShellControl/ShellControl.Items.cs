using System.Linq;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;

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

        #region SecondaryCommands
        public IEnumerable<ICommandBarElement> SecondaryCommands
        {
            get { return (IEnumerable<ICommandBarElement>)GetValue(SecondaryCommandsProperty); }
            set { SetValue(SecondaryCommandsProperty, value); }
        }

        private static void SecondaryCommandsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ShellControl;
            control.ArrangeCommands();
        }

        public static readonly DependencyProperty SecondaryCommandsProperty = DependencyProperty.Register("SecondaryCommands", typeof(IEnumerable<ICommandBarElement>), typeof(ShellControl), new PropertyMetadata(null, SecondaryCommandsChanged));
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

        #region HideCommandBar
        public bool HideCommandBar
        {
            get { return (bool)GetValue(HideCommandBarProperty); }
            set { SetValue(HideCommandBarProperty, value); }
        }

        private static void HideCommandBarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ShellControl;
            control.ArrangeCommands();
        }

        public static readonly DependencyProperty HideCommandBarProperty = DependencyProperty.Register("HideCommandBar", typeof(bool), typeof(ShellControl), new PropertyMetadata(false, HideCommandBarChanged));
        #endregion

        private void ArrangeCommands()
        {
            if (_isInitialized)
            {
                _commandBarT.PrimaryCommands.Clear();
                _commandBarB.PrimaryCommands.Clear();
                _commandBarT.SecondaryCommands.Clear();
                _commandBarB.SecondaryCommands.Clear();
                if (!this.HideCommandBar)
                {
                    _panes.Margin = new Thickness(0, 48, 0, 0);
                    if (this.CommandBarAlignment == CommandBarAlignment.Top)
                    {
                        if (this.PrimaryCommands != null)
                        {
                            foreach (var item in this.PrimaryCommands)
                            {
                                SetForeground(item);
                                _commandBarT.PrimaryCommands.Add(item);
                            }
                        }
                        if (this.SecondaryCommands != null)
                        {
                            foreach (var item in this.SecondaryCommands)
                            {
                                SetForeground(item);
                                _commandBarT.SecondaryCommands.Add(item);
                            }
                        }
                        _commandBarT.Margin = new Thickness(48, 0, 0, 0);
                        _commandBarT.Visibility = Visibility.Visible;
                        _commandBarB.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        if (this.PrimaryCommands != null)
                        {
                            foreach (var item in this.PrimaryCommands)
                            {
                                SetForeground(item);
                                _commandBarB.PrimaryCommands.Add(item);
                            }
                        }
                        if (this.SecondaryCommands != null)
                        {
                            foreach (var item in this.SecondaryCommands)
                            {
                                SetForeground(item);
                                _commandBarB.SecondaryCommands.Add(item);
                            }
                        }
                        _commandBarT.Margin = new Thickness(48, 0, -48, 0);
                        _commandBarT.Visibility = Visibility.Visible;
                        _commandBarB.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    _panes.Margin = new Thickness(0);
                    _commandBarT.Visibility = Visibility.Collapsed;
                    _commandBarB.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void SetForeground(ICommandBarElement item)
        {
            var control = item as Control;
            control.Foreground = this.CommandBarForeground;
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
