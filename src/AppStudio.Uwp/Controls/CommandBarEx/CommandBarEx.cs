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

        private static void PrimaryCommandItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CommandBarEx;
            control.ArrangeCommands();
        }

        public static readonly DependencyProperty PrimaryCommandItemsProperty = DependencyProperty.Register("PrimaryCommandItems", typeof(IEnumerable<ICommandBarElement>), typeof(CommandBarEx), new PropertyMetadata(null, PrimaryCommandItemsChanged));
        #endregion

        private void ArrangeCommands()
        {
            base.PrimaryCommands.Clear();
            foreach (var item in this.PrimaryCommandItems)
            {
                var fe = item as FrameworkElement;
                fe.SetBinding(CommandBar.ForegroundProperty, new Binding { Source = this, Path = new PropertyPath("Foreground") });
                base.PrimaryCommands.Add(item);
            }
        }
    }
}
