using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    public sealed partial class DateTimeLayout : Control
    {
        private static double DefaultWidth = 150;
        private static double DefaultHeight = 150;
        private static Thickness DefaultPadding = new Thickness(12.0);
        private static Thickness DefaultLine2Margin = new Thickness(0);
        private static double DefaultTextFontSize = 30.0;
        private static double TextFontSizeDifferenceDate = 15.0;
        private static double TextFontSizeDifferenceTime = 20.0;
        private static double Line2MarginDifferenceTime = 4;
        private static double Line2MarginDifferenceDate = 2;
        public enum DisplayMode { Date, Time }
        #region Properties
        public static readonly DependencyProperty Line1TextProperty =
            DependencyProperty.Register("Line1Text", typeof(string), typeof(DateTimeLayout), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty Line2TextProperty =
            DependencyProperty.Register("Line2Text", typeof(string), typeof(DateTimeLayout), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty Line1FontSizeProperty =
            DependencyProperty.Register("Line1FontSize", typeof(double), typeof(DateTimeLayout), new PropertyMetadata(DefaultTextFontSize + TextFontSizeDifferenceDate));
        public static readonly DependencyProperty Line2FontSizeProperty =
            DependencyProperty.Register("Line2FontSize", typeof(double), typeof(DateTimeLayout), new PropertyMetadata(DefaultTextFontSize));
        public static readonly DependencyProperty TextFontSizeProperty =
            DependencyProperty.Register("TextFontSize", typeof(double), typeof(DateTimeLayout), new PropertyMetadata(DefaultTextFontSize, (d, e) => { ((DateTimeLayout)d).UpdateTextFontSizeProperty(e.NewValue); }));
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(DateTimeLayout), new PropertyMetadata(TextAlignment.Center));
        public static readonly DependencyProperty Line2MarginProperty =
    DependencyProperty.Register("Line2Margin", typeof(Thickness), typeof(DateTimeLayout), new PropertyMetadata(DefaultLine2Margin));
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(DateTime), typeof(DateTimeLayout), new PropertyMetadata(DateTime.Now, (d, e) => { ((DateTimeLayout)d).UpdateContent(e.NewValue); }));
        public static readonly DependencyProperty DisplayModeValueProperty =
            DependencyProperty.Register("DisplayModeValue", typeof(DisplayMode), typeof(DateTimeLayout), new PropertyMetadata(DisplayMode.Date, (d, e) => { ((DateTimeLayout)d).UpdateDisplayMode(e.NewValue); }));

        public string Line1Text
        {
            get { return (string)GetValue(Line1TextProperty); }
            set { SetValue(Line1TextProperty, value); }
        }
        public string Line2Text
        {
            get { return (string)GetValue(Line2TextProperty); }
            set { SetValue(Line2TextProperty, value); }
        }
        public double Line1FontSize
        {
            get { return (double)GetValue(Line1FontSizeProperty); }
            set { SetValue(Line1FontSizeProperty, value); }
        }
        public double Line2FontSize
        {
            get { return (double)GetValue(Line2FontSizeProperty); }
            set { SetValue(Line2FontSizeProperty, value); }
        }
        public double TextFontSize
        {
            get { return (double)GetValue(TextFontSizeProperty); }
            set { SetValue(TextFontSizeProperty, value); System.Diagnostics.Debug.WriteLine("TextFontSize: " + value); }
        }
        public Thickness Line2Margin
        {
            get { return (Thickness)GetValue(Line2MarginProperty); }
            set { SetValue(Line2MarginProperty, value); }
        }
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }
        public DateTime Content
        {
            get { return (DateTime)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        public DisplayMode DisplayModeValue
        {
            get { return (DisplayMode)GetValue(DisplayModeValueProperty); }
            set { SetValue(DisplayModeValueProperty, value); }
        }
        #endregion
        public DateTimeLayout()
        {
            this.VerticalAlignment = VerticalAlignment.Center;
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.HorizontalContentAlignment = HorizontalAlignment.Center;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.Padding = DefaultPadding;
            this.Width = DefaultWidth;
            this.Height = DefaultHeight;
            this.DefaultStyleKey = typeof(DateTimeLayout);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateDisplayMode(DisplayModeValue);
            UpdateContent(Content);
            UpdateTextFontSizeProperty(TextFontSize);
        }

        private void UpdateDisplayMode(object newValue)
        {
            if (newValue != null && newValue.GetType() == typeof(DisplayMode))
            {
                UpdateContent(Content);
                UpdateTextFontSizeProperty(TextFontSize);
            }
        }

        private void UpdateContent(object newValue)
        {
            if (newValue != null && newValue.GetType() == typeof(DateTime))
            {
                DateTime newDateTime = (DateTime)newValue;
                if (DisplayModeValue == DisplayMode.Date)
                {
                    Line1Text = DateTimeNumberToString(newDateTime.Day);
                    Line2Text = newDateTime.ToString("MMM", CultureInfo.CurrentCulture).ToUpper();
                }
                else if (DisplayModeValue == DisplayMode.Time)
                {
                    Line1Text = DateTimeNumberToString(newDateTime.Hour);
                    Line2Text = DateTimeNumberToString(newDateTime.Minute);                    
                }
            }
        }

        private void UpdateTextFontSizeProperty(object newValue)
        {
            if (newValue != null && newValue.GetType() == typeof(double))
            {
                double newTextFontSize = (double)newValue;
                if (DisplayModeValue == DisplayMode.Date)
                {
                    Line1FontSize = newTextFontSize + TextFontSizeDifferenceDate;
                    Line2FontSize = newTextFontSize;
                    Line2Margin = new Thickness(0, newTextFontSize / Line2MarginDifferenceDate, 0, 0);
                }
                else if (DisplayModeValue == DisplayMode.Time)
                {
                    Line1FontSize = newTextFontSize + TextFontSizeDifferenceTime;
                    Line2FontSize = newTextFontSize + TextFontSizeDifferenceTime;
                    Line2Margin = new Thickness(0, newTextFontSize / Line2MarginDifferenceTime, 0, 0);
                }
            }
        }
        private string DateTimeNumberToString(double newValue)
        {
            if (newValue < 10)
            {
                return string.Format("0{0}", newValue);
            }
            else
            {
                return newValue.ToString();
            }
        }
    }
}
