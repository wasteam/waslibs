using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace AppStudio.Uwp.Controls
{
    public sealed partial class SearchBox : Control
    {
        TextBox textBox;
        TextBlock placeholderText;
        Grid shadow;
        Grid searchTextGrid;
        Grid searchButtonGrid;
        Storyboard FadeInStoryboard;
        Storyboard FadeOutStoryboard;
        Storyboard OpenStoryboard;
        Storyboard CloseStoryboard;

        public SearchBox()
        {
            base.HorizontalAlignment = HorizontalAlignment.Center;
            this.Height = 68;
            this.DefaultStyleKey = typeof(SearchBox);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            textBox = base.GetTemplateChild("textBox") as TextBox;
            placeholderText = base.GetTemplateChild("placeholderText") as TextBlock;
            shadow = base.GetTemplateChild("shadow") as Grid;
            searchTextGrid = base.GetTemplateChild("searchTextGrid") as Grid;
            searchButtonGrid = base.GetTemplateChild("searchButtonGrid") as Grid;
            FadeInStoryboard = base.GetTemplateChild("FadeInStoryboard") as Storyboard;
            FadeOutStoryboard = base.GetTemplateChild("FadeOutStoryboard") as Storyboard;
            OpenStoryboard = base.GetTemplateChild("OpenStoryboard") as Storyboard;
            CloseStoryboard = base.GetTemplateChild("CloseStoryboard") as Storyboard;
            textBox.LostFocus += OnLostFocus;
            textBox.KeyUp += TextKeyUp;
            searchButtonGrid.Tapped += OnTapped;
            searchButtonGrid.PointerEntered += OnPointerEntered;
            searchButtonGrid.PointerExited += OnPointerExited;
            searchButtonGrid.PointerPressed += OnPointerPressed;
            searchButtonGrid.PointerReleased += OnPointerEntered;
            UpdateTextVisibility();
        }


        #region FrameworkElementEvents
        private void TextKeyUp(object sender, KeyRoutedEventArgs e)
        {
            UpdateHelpVisibility(textBox.Text);
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                ExecuteCommand(textBox.Text);
            }
            else if (e.Key == Windows.System.VirtualKey.Escape)
            {
                Reset();
            }
        }
        private async void OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (IsTextVisible)
            {
                //Await 100 miliseconds while databinding set Text property
                await Task.Delay(100);
                if (!ExecuteCommand(textBox.Text))
                {
                    HideSearchText();
                }
            }
            else
            {
                ShowSearchText();
                textBox.Focus(FocusState.Keyboard);
            }
        }
        public void OnLostFocus(object sender, RoutedEventArgs e) { } //=> HideSearchText();
        public void OnPointerEntered(object sender, PointerRoutedEventArgs e) => shadow.Opacity = 0.2;
        public void OnPointerExited(object sender, PointerRoutedEventArgs e) => shadow.Opacity = 0.0;
        public void OnPointerPressed(object sender, PointerRoutedEventArgs e) => shadow.Opacity = 0.6;
        #endregion


        #region Methods
        private bool ExecuteCommand(string text)
        {
            if (!string.IsNullOrEmpty(text) && SearchCommand != null)
            {
                if (SearchCommand.CanExecute(text))
                {
                    SearchCommand.Execute(text);
                    return true;
                }
            }
            return false;
        }
        public void Reset()
        {
            Text = string.Empty;
            HideSearchText();
        }
        private async void HideSearchText()
        {
            if (DisplayMode == DisplayModeValue.Expand)
            {
                var width = searchTextGrid.Width;
                CloseStoryboard.Begin();
                await Task.Delay(500);
                searchTextGrid.Visibility = Visibility.Collapsed;
                searchTextGrid.Width = width;
                IsTextVisible = false;
            }
            if (DisplayMode == DisplayModeValue.FadeIn)
            {
                FadeOutStoryboard.Begin();
                await Task.Delay(1000);
                searchTextGrid.Visibility = Visibility.Collapsed;
                searchTextGrid.Opacity = 1;
                IsTextVisible = false;
            }
        }
        private void ShowSearchText()
        {
            if (DisplayMode == DisplayModeValue.Expand)
            {
                searchTextGrid.Visibility = Visibility.Visible;
                OpenStoryboard.Begin();
                IsTextVisible = true;
            }
            if (DisplayMode == DisplayModeValue.FadeIn)
            {
                searchTextGrid.Opacity = 0;
                searchTextGrid.Visibility = Visibility.Visible;
                FadeInStoryboard.Begin();
                IsTextVisible = true;
            }
        }
        private void UpdateHelpVisibility(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                placeholderText.Visibility = Visibility.Visible;
            }
            else
            {
                placeholderText.Visibility = Visibility.Collapsed;
            }
        }
        private void UpdateTextVisibility()
        {
            searchTextGrid.Opacity = 1.0;
            if (DisplayMode == DisplayModeValue.Visible)
            {
                searchTextGrid.Visibility = Visibility.Visible;
                IsTextVisible = true;
            }
            else
            {
                searchTextGrid.Visibility = Visibility.Collapsed;
                IsTextVisible = false;
            }
        }
        #endregion  
    }
}
