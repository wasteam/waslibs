using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    public class NavigationStyleSelector : StyleSelector
    {
        private Style _itemStyle;
        private Style _separatorStyle;

        public NavigationStyleSelector(Style itemContainerStyle, Style separatorStyle)
        {
            _itemStyle = itemContainerStyle;
            _separatorStyle = separatorStyle;
        }

        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            var menuItem = item as NavigationItem;
            if (menuItem.IsSeparator)
            {
                return _separatorStyle;
            }
            return _itemStyle;
        }
    }
}
