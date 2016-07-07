using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    public class CommandBarEx : CommandBar
    {
        #region PrimaryCommandItems
        public IEnumerable<ICommandBarElement> PrimaryCommandItems
        {
            get { return (IEnumerable<ICommandBarElement>)GetValue(PrimaryCommandItemsProperty); }
            set { SetValue(PrimaryCommandItemsProperty, value); }
        }

        public static readonly DependencyProperty PrimaryCommandItemsProperty = DependencyProperty.Register("PrimaryCommandItems", typeof(IEnumerable<ICommandBarElement>), typeof(CommandBarEx), new PropertyMetadata(null, CommandItemsChanged));
        #endregion

        #region SecondaryCommandItems
        public IEnumerable<ICommandBarElement> SecondaryCommandItems
        {
            get { return (IEnumerable<ICommandBarElement>)GetValue(SecondaryCommandItemsProperty); }
            set { SetValue(SecondaryCommandItemsProperty, value); }
        }

        public static readonly DependencyProperty SecondaryCommandItemsProperty = DependencyProperty.Register("SecondaryCommandItems", typeof(IEnumerable<ICommandBarElement>), typeof(CommandBarEx), new PropertyMetadata(null, CommandItemsChanged));
        #endregion

        private static void CommandItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CommandBarEx;
            control.ArrangeCommands();
        }

        private void ArrangeCommands()
        {
            base.PrimaryCommands.Clear();
            base.SecondaryCommands.Clear();

            if (this.PrimaryCommandItems != null)
            {
                foreach (var item in this.PrimaryCommandItems)
                {
                    var fe = item as FrameworkElement;
                    fe.SetBinding(CommandBar.ForegroundProperty, new Binding { Source = this, Path = new PropertyPath("Foreground") });
                    base.PrimaryCommands.Add(item);
                }
            }

            if (this.SecondaryCommandItems != null)
            {
                foreach (var item in this.SecondaryCommandItems)
                {
                    var fe = item as FrameworkElement;
                    fe.SetBinding(CommandBar.ForegroundProperty, new Binding { Source = this, Path = new PropertyPath("Foreground") });
                    base.SecondaryCommands.Add(item);
                }
            }
        }
    }
}
