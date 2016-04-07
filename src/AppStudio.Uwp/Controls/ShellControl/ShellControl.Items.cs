using System.Collections.Generic;

using Windows.UI.Xaml;

namespace AppStudio.Uwp.Controls
{
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
    }
}
