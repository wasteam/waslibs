using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    partial class ShellControl
    {
        private NavigationItem _currentItem = null;

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
            if (navItem != _currentItem)
            {
                _currentItem = navItem;
                navItem.OnClick(navItem);
            }
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
                _splitView.IsPaneOpen = true;
            }
        }

        private async void InvokeSubItems(NavigationItem navItem)
        {
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
                _splitView.IsPaneOpen = true;
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
