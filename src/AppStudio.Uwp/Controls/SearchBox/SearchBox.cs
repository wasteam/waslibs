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
        private const double _animationDurationMilliseconds = 500;
        private TextBox _textBox;

        public SearchBox()
        {
            base.HorizontalAlignment = HorizontalAlignment.Center;
            this.Height = 68;
            this.DefaultStyleKey = typeof(SearchBox);
        }

        protected override void OnApplyTemplate()
        {
            Grid _searchButtonGrid;
            _textBox = base.GetTemplateChild("textBox") as TextBox;
            _searchButtonGrid = base.GetTemplateChild("searchButtonGrid") as Grid;
            _textBox.LostFocus += OnLostFocus;
            _textBox.KeyUp += TextKeyUp;
            _searchButtonGrid.Tapped += OnTapped;
            _searchButtonGrid.PointerEntered += OnPointerEntered;
            _searchButtonGrid.PointerExited += OnPointerExited;
            _searchButtonGrid.PointerPressed += OnPointerPressed;
            _searchButtonGrid.PointerReleased += OnPointerEntered;            
            UpdateSearchTextGridVisibility();
            base.OnApplyTemplate();
        }


        #region FrameworkElementEvents
        private void TextKeyUp(object sender, KeyRoutedEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt != null)
            {
                Text = txt.Text;
                UpdatePlaceholderTextVisibility(Text);
                if (e.Key == Windows.System.VirtualKey.Enter)
                {
                    ExecuteCommand(Text);
                }
                else if (e.Key == Windows.System.VirtualKey.Escape)
                {
                    Reset();
                }
            }
        }
        private void OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (IsTextVisible)
            {
                if (!ExecuteCommand(Text))
                {
                    HideSearchText();
                }
            }
            else
            {
                ShowSearchText();
                _textBox.Focus(FocusState.Keyboard);
            }
        }
        public void OnLostFocus(object sender, RoutedEventArgs e)
        {

            TextBox txt = sender as TextBox;
            if (txt != null)
            {
                Text = txt.Text;
                UpdatePlaceholderTextVisibility(Text);
            }
            //HideSearchText();
        }
        public void OnPointerEntered(object sender, PointerRoutedEventArgs e) => ShadowOpacity = 0.2;
        public void OnPointerExited(object sender, PointerRoutedEventArgs e) => ShadowOpacity = 0.0;
        public void OnPointerPressed(object sender, PointerRoutedEventArgs e) => ShadowOpacity = 0.6;
        #endregion


        #region Methods
        public void Reset()
        {
            Text = string.Empty;
            HideSearchText();
        }
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
        private async void HideSearchText()
        {
            if (DisplayMode == DisplayModeValue.Expand)
            {
                var oldValue = SearchWidth;
                await this.AnimateDoublePropertyAsync("SearchWidth", SearchWidth, 0.0, _animationDurationMilliseconds);
                SearchTextGridVisibility = Visibility.Collapsed;
                IsTextVisible = false;
                SearchWidth = oldValue;
            }
            if (DisplayMode == DisplayModeValue.FadeIn)
            {
                await this.AnimateDoublePropertyAsync("SearchTextGridOpacity", 1.0, 0.0, _animationDurationMilliseconds);
                SearchTextGridVisibility = Visibility.Collapsed;
                IsTextVisible = false;
                SearchTextGridOpacity = 1.0;
            }
        }

        private async Task ShowSearchText()
        {
            if (DisplayMode == DisplayModeValue.Expand)
            {
                SearchTextGridVisibility = Visibility.Visible;
                await this.AnimateDoublePropertyAsync("SearchWidth", 0.0, SearchWidth, _animationDurationMilliseconds);
                IsTextVisible = true;
            }
            if (DisplayMode == DisplayModeValue.FadeIn)
            {
                SearchTextGridVisibility = Visibility.Visible;
                await this.AnimateDoublePropertyAsync("SearchTextGridOpacity", 0.0, 1.0, _animationDurationMilliseconds);
                IsTextVisible = true;
            }
        }
        private void UpdatePlaceholderTextVisibility(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                PlaceholderTextVisibility = Visibility.Visible;
            }
            else
            {
                PlaceholderTextVisibility = Visibility.Collapsed;
            }
        }
        private void UpdateSearchTextGridVisibility()
        {
            if (DisplayMode == DisplayModeValue.Visible)
            {
                SearchTextGridVisibility = Visibility.Visible;
                IsTextVisible = true;
            }
            else
            {
                SearchTextGridVisibility = Visibility.Collapsed;
                IsTextVisible = false;
            }
        }
        #endregion
    }
}
