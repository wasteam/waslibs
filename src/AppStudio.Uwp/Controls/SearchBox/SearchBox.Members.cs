using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    partial class SearchBox
    {
        #region Properties
        public enum DisplayModeValue { Visible, Expand, FadeIn };

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(SearchBox), new PropertyMetadata("search"));
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SearchBox), new PropertyMetadata(string.Empty, OnTextPropertyChanged));
        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register("MaxLength", typeof(int), typeof(SearchBox), new PropertyMetadata(int.MaxValue));
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(SearchBox), new PropertyMetadata(TextAlignment.Left));
        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register("SearchCommand", typeof(ICommand), typeof(SearchBox), new PropertyMetadata(null));
        public static readonly DependencyProperty SearchWidthProperty =
            DependencyProperty.Register("SearchWidth", typeof(Double), typeof(SearchBox), new PropertyMetadata(250.0));
        public static readonly DependencyProperty SearchButtonSizeProperty =
            DependencyProperty.Register("SearchButtonSize", typeof(Double), typeof(SearchBox), new PropertyMetadata(20.0));
        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register("DisplayMode", typeof(DisplayModeValue), typeof(SearchBox), new PropertyMetadata(DisplayModeValue.Visible, OnDisplayModePropertyChanged));        
        public static readonly DependencyProperty IsTextVisibleProperty =
            DependencyProperty.Register("IsTextVisible", typeof(bool), typeof(SearchBox), new PropertyMetadata(false));


        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }
        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }
        public Double SearchWidth
        {
            get { return (Double)GetValue(SearchWidthProperty); }
            set { SetValue(SearchWidthProperty, value); }
        }
        public Double SearchButtonSize
        {
            get { return (Double)GetValue(SearchButtonSizeProperty); }
            set { SetValue(SearchButtonSizeProperty, value); }
        }
        public DisplayModeValue DisplayMode
        {
            get { return (DisplayModeValue)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }
        public bool IsTextVisible
        {
            get { return (bool)GetValue(IsTextVisibleProperty); }
            private set { SetValue(IsTextVisibleProperty, value); }
        }
        #endregion
        #region PropertyChangedEvents
        private static void OnDisplayModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SearchBox control = d as SearchBox;
            if (control != null && control.searchTextGrid != null)
            {
                control.UpdateTextVisibility();
            }
        }
        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SearchBox control = d as SearchBox;
            if (control != null)
            {
                control.UpdateHelpVisibility(e.NewValue);
            }
        }
        #endregion
    }
}
