using System;
using System.Linq;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    partial class ShellControl
    {
        private NavigationItem _currentItem = null;
        private int _selectedIndex = 0;

        public void SelectItem(string id)
        {
            if (_isInitialized)
            {
                if (id != null)
                {
                    int index = 0;
                    var selected = _lview.Items.Cast<NavigationItem>().Select(r => new { Index = index++, Item = r }).Where(r => r.Item.ID != null && r.Item.ID.Equals(id, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (selected != null)
                    {
                        _currentItem = selected.Item;
                        _selectedIndex = selected.Index;
                        _lview.SelectedIndex = selected.Index;
                    }
                }
                else
                {
                    _currentItem = null;
                    _selectedIndex = -1;
                    _lview.SelectedIndex = -1;
                }
            }
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            var navItem = e.ClickedItem as NavigationItem;
            if (navItem != null)
            {
                if (navItem.OnClick != null)
                {
                    InvokeClick(navItem);
                }
                else if (navItem.Control != null)
                {
                    InvokeContent(navItem);
                }
                else if (navItem.SubItems != null)
                {
                    InvokeSubItems(navItem);
                }
            }
        }

        private void InvokeClick(NavigationItem navItem)
        {
            _splitView.IsPaneOpen = false;
            _currentItem = navItem;
            navItem.OnClick(navItem);
        }

        private async void InvokeContent(NavigationItem navItem)
        {
            if (_splitView.IsPaneOpen)
            {
                if (navItem != _currentItem)
                {
                    _lview.Width = _splitView.CompactPaneLength;
                    await System.Threading.Tasks.Task.Delay(50);

                    _currentItem = navItem;
                    _content.Children.Clear();
                    _content.Children.Add(navItem.Control);
                    _container.Visibility = Visibility.Visible;
                    _content.Visibility = Visibility.Visible;
                    _lviewSub.Visibility = Visibility.Collapsed;
                }
                else
                {
                    _splitView.IsPaneOpen = false;
                    _currentItem = null;
                }
            }
            else
            {
                _currentItem = navItem;
                _content.Children.Clear();
                _content.Children.Add(navItem.Control);
                _container.Visibility = Visibility.Visible;
                _content.Visibility = Visibility.Visible;
                _lviewSub.Visibility = Visibility.Collapsed;
                OpenLeftPane();
            }
        }

        private async void InvokeSubItems(NavigationItem navItem)
        {
            _lviewSub.Background = navItem.Background;
            if (_splitView.IsPaneOpen)
            {
                if (navItem != _currentItem)
                {
                    _lview.Width = _splitView.CompactPaneLength;
                    await System.Threading.Tasks.Task.Delay(50);

                    _currentItem = navItem;
                    _content.Children.Clear();
                    _container.Visibility = Visibility.Visible;
                    _content.Visibility = Visibility.Collapsed;
                    _lviewSub.Visibility = Visibility.Visible;
                    _lviewSub.ItemsSource = navItem.SubItems;
                }
                else
                {
                    _splitView.IsPaneOpen = false;
                    _currentItem = null;
                }
            }
            else
            {
                _currentItem = navItem;
                _content.Children.Clear();
                _container.Visibility = Visibility.Visible;
                _content.Visibility = Visibility.Collapsed;
                _lviewSub.Visibility = Visibility.Visible;
                _lviewSub.ItemsSource = navItem.SubItems;
                OpenLeftPane();
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_splitView.IsPaneOpen)
            {
                var navItem = _lview.SelectedItem as NavigationItem;
                if (navItem != null)
                {
                    if (navItem.ClearSelection)
                    {
                        _lview.SelectedItem = null;
                        _currentItem = null;
                    }
                }
            }
        }
    }
}
