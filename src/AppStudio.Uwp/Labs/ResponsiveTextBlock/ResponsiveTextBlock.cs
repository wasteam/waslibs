using System;

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Labs
{
    public sealed class ResponsiveTextBlock : Control
    {
        private TextBlock _textBlock = null;

        public ResponsiveTextBlock()
        {
            this.DefaultStyleKey = typeof(ResponsiveTextBlock);
        }

        #region Text
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ResponsiveTextBlock), new PropertyMetadata(null));
        #endregion

        #region TextWrapping
        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        public static readonly DependencyProperty TextWrappingProperty = DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(ResponsiveTextBlock), new PropertyMetadata(TextWrapping.NoWrap));
        #endregion

        #region TextTrimming
        public TextTrimming TextTrimming
        {
            get { return (TextTrimming)GetValue(TextTrimmingProperty); }
            set { SetValue(TextTrimmingProperty, value); }
        }

        public static readonly DependencyProperty TextTrimmingProperty = DependencyProperty.Register("TextTrimming", typeof(TextTrimming), typeof(ResponsiveTextBlock), new PropertyMetadata(TextTrimming.None));
        #endregion

        #region TextAlignment
        public TextAlignment TextAlignment
        {
            get { return (TextAlignment)GetValue(TextAlignmentProperty); }
            set { SetValue(TextAlignmentProperty, value); }
        }

        public static readonly DependencyProperty TextAlignmentProperty = DependencyProperty.Register("TextAlignment", typeof(TextAlignment), typeof(ResponsiveTextBlock), new PropertyMetadata(TextAlignment.Left));
        #endregion

        #region MinFontSize
        public double MinFontSize
        {
            get { return (double)GetValue(MinFontSizeProperty); }
            set { SetValue(MinFontSizeProperty, value); }
        }

        public static readonly DependencyProperty MinFontSizeProperty = DependencyProperty.Register("MinFontSize", typeof(double), typeof(ResponsiveTextBlock), new PropertyMetadata(14.0));
        #endregion

        #region MaxFontSize
        public double MaxFontSize
        {
            get { return (double)GetValue(MaxFontSizeProperty); }
            set { SetValue(MaxFontSizeProperty, value); }
        }

        public static readonly DependencyProperty MaxFontSizeProperty = DependencyProperty.Register("MaxFontSize", typeof(double), typeof(ResponsiveTextBlock), new PropertyMetadata(28.0));
        #endregion

        #region IsSingleLine
        public bool IsSingleLine
        {
            get { return (bool)GetValue(IsSingleLineProperty); }
            set { SetValue(IsSingleLineProperty, value); }
        }

        public static readonly DependencyProperty IsSingleLineProperty = DependencyProperty.Register("IsSingleLine", typeof(bool), typeof(ResponsiveTextBlock), new PropertyMetadata(false));
        #endregion

        protected override void OnApplyTemplate()
        {
            _textBlock = base.GetTemplateChild("textBlock") as TextBlock;

            base.OnApplyTemplate();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            double fontSize = this.MinFontSize;

            if (this.IsSingleLine)
            {
                while (fontSize < this.MaxFontSize)
                {
                    var requiredSize = GetTextSize(Double.PositiveInfinity, fontSize + 1.0, TextWrapping.NoWrap);
                    if (requiredSize.Width > availableSize.Width || requiredSize.Height > availableSize.Height)
                    {
                        break;
                    }
                    fontSize++;
                }
            }
            else
            {
                while (fontSize < this.MaxFontSize)
                {
                    var requiredSize = GetTextSize(availableSize.Width, fontSize + 1.0, this.TextWrapping);
                    if (requiredSize.Height > availableSize.Height)
                    {
                        break;
                    }
                    fontSize++;
                }
            }

            _textBlock.FontSize = fontSize;
            base.MeasureOverride(availableSize);
            return _textBlock.DesiredSize;
        }

        #region GetTextSize
        private Size GetTextSize(double availableWidth, double fontSize, TextWrapping textWrapping)
        {
            var block = new TextBlock
            {
                Text = $"{this.Text}",
                TextAlignment = this.TextAlignment,
                FontSize = fontSize,
                TextWrapping = textWrapping
            };
            block.Measure(new Size(availableWidth, Double.PositiveInfinity));
            return block.DesiredSize;
        }
        #endregion
    }
}
