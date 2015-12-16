using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    public sealed partial class Indicator : Control
    {
        private StackPanel _stack = null;

        private bool _isInitialized = false;

        public Indicator()
        {
            this.DefaultStyleKey = typeof(Indicator);
        }

        #region ItemContainerStyle
        public Style ItemContainerStyle
        {
            get { return (Style)GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }

        public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(Indicator), new PropertyMetadata(null));
        #endregion

        #region ItemTemplate
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(Indicator), new PropertyMetadata(null));
        #endregion

        #region SelectedIndex
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        private static void SelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Indicator;
            control.SelectedIndexChanged((int)e.NewValue);
        }

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(Indicator), new PropertyMetadata(-1, SelectedIndexChanged));
        #endregion

        protected override void OnApplyTemplate()
        {
            _stack = base.GetTemplateChild("stack") as StackPanel;

            _isInitialized = true;

            ItemsSourceChanged(this.ItemsSource as IEnumerable);
            SelectedIndexChanged(this.SelectedIndex);

            base.OnApplyTemplate();
        }

        private void OnSelectedItemChanged(object sender, EventArgs e)
        {
            if (_isInitialized)
            {
                var item = sender as IndicatorItem;
                if (item.IsSelected)
                {
                    UnselectItems(_stack.Children.Cast<IndicatorItem>().Where(r => r != item));
                    this.SelectedIndex = _stack.Children.IndexOf(item);
                }
            }
        }

        private void SelectedIndexChanged(int newValue)
        {
            if (_isInitialized)
            {
                UnselectItems(_stack.Children.Cast<IndicatorItem>());
                if (newValue > -1 && newValue < _stack.Children.Count)
                {
                    var control = _stack.Children[newValue] as IndicatorItem;
                    control.IsSelected = true;
                }
            }
        }

        #region UnselectItems
        private void UnselectItems(IEnumerable<IndicatorItem> items)
        {
            foreach (var control in items.Where(r => r.IsSelected == true))
            {
                control.IsSelected = false;
            }
        }
        #endregion
    }
}
