using System.Linq;
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

        private static void NavigationItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ShellControl;
            control.SelectFirstNavigationItem();
        }

        private void SelectFirstNavigationItem()
        {
            if (_isInitialized)
            {
                if (this.NavigationItems != null && this.NavigationItems.Count() > 0)
                {
                    _lview.SelectedIndex = 0;
                }
            }
        }

        public static readonly DependencyProperty NavigationItemsProperty = DependencyProperty.Register("NavigationItems", typeof(IEnumerable<NavigationItem>), typeof(ShellControl), new PropertyMetadata(null, NavigationItemsChanged));
        #endregion
    }
}
