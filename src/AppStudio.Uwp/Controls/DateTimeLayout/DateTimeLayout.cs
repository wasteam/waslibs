using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    public sealed partial class DateTimeLayout : Control
    {
        #region Properties
        public static readonly DependencyProperty Line1TextProperty =
            DependencyProperty.Register("Line1Text", typeof(string), typeof(DateTimeLayout), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty Line2TextProperty =
            DependencyProperty.Register("Line2Text", typeof(string), typeof(DateTimeLayout), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty Line3TextProperty =
            DependencyProperty.Register("Line3Text", typeof(string), typeof(DateTimeLayout), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty Line1FontSizeProperty =
            DependencyProperty.Register("Line1FontSize", typeof(double), typeof(DateTimeLayout), new PropertyMetadata(32.0));
        public static readonly DependencyProperty Line2FontSizeProperty =
            DependencyProperty.Register("Line2FontSize", typeof(double), typeof(DateTimeLayout), new PropertyMetadata(19.0));
        public static readonly DependencyProperty Line3FontSizeProperty =
            DependencyProperty.Register("Line3FontSize", typeof(double), typeof(DateTimeLayout), new PropertyMetadata(32.0));
        public static readonly DependencyProperty TextFontSizeProperty =
            DependencyProperty.Register("TextFontSize", typeof(double), typeof(DateTimeLayout), new PropertyMetadata(19.0, (d, e) => { ((DateTimeLayout)d).UpdateTextFontSizeProperty(e.NewValue); }));
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(DateTimeLayout), new PropertyMetadata(TextAlignment.Center));
        public static readonly DependencyProperty DateTimeValueProperty =
            DependencyProperty.Register("DateTimeValue", typeof(DateTime), typeof(DateTimeLayout), new PropertyMetadata(DateTime.Now, (d, e) => { ((DateTimeLayout)d).UpdateDateTimeValue(e.NewValue); }));

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
        public string Line3Text
        {
            get { return (string)GetValue(Line3TextProperty); }
            set { SetValue(Line3TextProperty, value); }
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
        public double Line3FontSize
        {
            get { return (double)GetValue(Line3FontSizeProperty); }
            set { SetValue(Line3FontSizeProperty, value); }
        }
        public double TextFontSize
        {
            get { return (double)GetValue(TextFontSizeProperty); }
            set { SetValue(TextFontSizeProperty, value); }
        }
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }
        public DateTime DateTimeValue
        {
            get { return (DateTime)GetValue(DateTimeValueProperty); }
            set { SetValue(DateTimeValueProperty, value); }
        }
        #endregion
        public DateTimeLayout()
        {            
            this.VerticalAlignment = VerticalAlignment.Center;
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.HorizontalContentAlignment = HorizontalAlignment.Center;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.Padding = new Thickness(12.0);
            this.Width = 150;
            this.Height = 150;
            this.DefaultStyleKey = typeof(DateTimeLayout);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateDateTimeValue(DateTimeValue);
            UpdateTextFontSizeProperty(TextFontSize);
        }
        private void UpdateDateTimeValue(object newValue)
        {
            if (newValue != null && newValue.GetType() == typeof(DateTime))
            {
                DateTime newDateTime = (DateTime)newValue;
                Line1Text = newDateTime.Day.ToString();
                Line2Text = newDateTime.ToString("MMMM");
                Line3Text = newDateTime.ToString("hh:mm");
            }
        }
        private void UpdateTextFontSizeProperty(object newValue)
        {
            if (newValue != null && newValue.GetType() == typeof(double))
            {
                double newTextFontSize = (double)newValue;
                Line1FontSize = newTextFontSize + 13;
                Line2FontSize = newTextFontSize;
                Line3FontSize = newTextFontSize + 13;
            }
        }
    }
}
