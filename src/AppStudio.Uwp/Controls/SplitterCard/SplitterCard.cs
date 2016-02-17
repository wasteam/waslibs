using System;
using System.Globalization;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    public sealed partial class SplitterCard : Control
    {
        private const double MarginFactor = 3.0;
        private const double FontSizeFactor = 1.5;
        private const double DefaultWidth = 350;
        private const double DefaultHeight = 350;
        private const double DefaultTextFontSize = 50.0;
        private static Thickness DefaultPadding = new Thickness(12.0);
        private static Thickness DefaultMargin = new Thickness(0);
        private static Thickness DefaultLine2Margin = ScaleMargin(ScaleFont(DefaultTextFontSize));

        #region Properties
        public static readonly DependencyProperty Line1TextProperty =
            DependencyProperty.Register("Line1Text", typeof(string), typeof(SplitterCard), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty Line2TextProperty =
            DependencyProperty.Register("Line2Text", typeof(string), typeof(SplitterCard), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SplitterCard), new PropertyMetadata(string.Empty, (d, e) => { ((SplitterCard)d).UpdateText(e.NewValue); }));
        public static readonly DependencyProperty TextFontSizeProperty =
            DependencyProperty.Register("TextFontSize", typeof(double), typeof(SplitterCard), new PropertyMetadata(DefaultTextFontSize, (d, e) => { ((SplitterCard)d).UpdateTextFontSizeProperty(e.NewValue); }));
        public static readonly DependencyProperty TextSplitterProperty =
            DependencyProperty.Register("TextSplitter", typeof(char), typeof(SplitterCard), new PropertyMetadata(' '));
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(SplitterCard), new PropertyMetadata(TextAlignment.Center));
        private static readonly DependencyProperty Line1FontWeightProperty =
            DependencyProperty.Register("Line1FontWeight", typeof(FontWeight), typeof(SplitterCard), new PropertyMetadata(FontWeights.Bold));
        private static readonly DependencyProperty Line2FontWeightProperty =
            DependencyProperty.Register("Line2FontWeight", typeof(FontWeight), typeof(SplitterCard), new PropertyMetadata(FontWeights.Light));
        private static readonly DependencyProperty Line1FontSizeProperty =
            DependencyProperty.Register("Line1FontSize", typeof(double), typeof(SplitterCard), new PropertyMetadata(ScaleFont(DefaultTextFontSize)));
        private static readonly DependencyProperty Line2FontSizeProperty =
            DependencyProperty.Register("Line2FontSize", typeof(double), typeof(SplitterCard), new PropertyMetadata(DefaultTextFontSize));
        private static readonly DependencyProperty Line2MarginProperty =
            DependencyProperty.Register("Line2Margin", typeof(Thickness), typeof(SplitterCard), new PropertyMetadata(DefaultLine2Margin));

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
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public double TextFontSize
        {
            get { return (double)GetValue(TextFontSizeProperty); }
            set { SetValue(TextFontSizeProperty, value); }
        }
        public char TextSplitter
        {
            get { return (char)GetValue(TextSplitterProperty); }
            set { SetValue(TextSplitterProperty, value); }
        }
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }
        internal FontWeight Line1FontWeight
        {
            get { return (FontWeight)GetValue(Line1FontWeightProperty); }
            set { SetValue(Line1FontWeightProperty, value); }
        }
        internal FontWeight Line2FontWeight
        {
            get { return (FontWeight)GetValue(Line2FontWeightProperty); }
            set { SetValue(Line2FontWeightProperty, value); }
        }
        internal double Line1FontSize
        {
            get { return (double)GetValue(Line1FontSizeProperty); }
            set { SetValue(Line1FontSizeProperty, value); }
        }
        internal double Line2FontSize
        {
            get { return (double)GetValue(Line2FontSizeProperty); }
            set { SetValue(Line2FontSizeProperty, value); }
        }
        internal Thickness Line2Margin
        {
            get { return (Thickness)GetValue(Line2MarginProperty); }
            set { SetValue(Line2MarginProperty, value); }
        }
        #endregion
        public SplitterCard()
        {
            this.VerticalAlignment = VerticalAlignment.Center;
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.HorizontalContentAlignment = HorizontalAlignment.Center;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.Padding = DefaultPadding;
            this.Margin = DefaultMargin;
            this.Width = DefaultWidth;
            this.Height = DefaultHeight;
            this.DefaultStyleKey = typeof(SplitterCard);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
        private void UpdateTextFontSizeProperty(object newValue)
        {
            if (newValue != null && newValue.GetType() == typeof(double))
            {
                double newTextFontSize = (double)newValue;
                if (newTextFontSize != Line2FontSize)
                {
                    Line1FontSize = ScaleFont(newTextFontSize);
                    Line2FontSize = newTextFontSize;
                    this.Line2Margin = ScaleMargin(Line1FontSize);
                }                
            }
        }
        private static double ScaleFont(double newTextFontSize)
        {
            return Math.Truncate(newTextFontSize + (newTextFontSize / FontSizeFactor));
        }
        private static Thickness ScaleMargin(double newMargin)
        {
            return new Thickness(0, Math.Truncate(newMargin / MarginFactor), 0, 0);
        }
        private void UpdateText(object newValue)
        {
            if (newValue != null && !string.IsNullOrEmpty(newValue.ToString()))
            {
                string newText = (string)newValue;
                string[] splitTexts = newText.Split(TextSplitter);
                if (splitTexts != null && splitTexts.Length > 0)
                {
                    Line1Text = splitTexts[0];
                }
                if (splitTexts != null && splitTexts.Length > 1)
                {
                    Line2Text = splitTexts[1];
                }
            }
        }
    }
}
