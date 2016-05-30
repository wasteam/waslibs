using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Input;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Data;

namespace AppStudio.Uwp.Controls
{
    public sealed class PropertySet : Control
    {
        private ComboBox _colors = null;
        private ComboBox _combo = null;
        private Slider _slider = null;
        private ToggleSwitch _toggle = null;
        private TextBox _textBox = null;

        private bool _isInitialized = false;

        public PropertySet()
        {
            this.DefaultStyleKey = typeof(PropertySet);
        }

        #region Source
        public object Source
        {
            get { return (object)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(object), typeof(PropertySet), new PropertyMetadata(null, ExploreProperty));
        #endregion

        #region Property
        public string Property
        {
            get { return (string)GetValue(PropertyProperty); }
            set { SetValue(PropertyProperty, value); }
        }

        public static readonly DependencyProperty PropertyProperty = DependencyProperty.Register("Property", typeof(string), typeof(PropertySet), new PropertyMetadata(null, ExploreProperty));
        #endregion

        #region Value
        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PropertySet;
            control.SetValue(e.NewValue);
        }

        private void SetValue(object value)
        {
            if (_isInitialized)
            {
                if (value != null)
                {
                    if (this.Source != null && this.Property != null)
                    {
                        var element = this.Source as FrameworkElement;
                        var prop = element.GetType().GetProperty(this.Property);
                        var type = prop.PropertyType;

                        if (type.GetTypeInfo().IsEnum)
                        {
                            _combo.SelectedIndex = GetIndexOf(type, value.ToString());
                        }
                    }
                }
            }
        }

        private static int GetIndexOf(Type type, string value)
        {
            return Enum.GetNames(type).ToList().IndexOf(value);
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(PropertySet), new PropertyMetadata(null, ValueChanged));
        #endregion

        #region Label
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(string), typeof(PropertySet), new PropertyMetadata(string.Empty));
        #endregion

        #region Minimun
        public double Minimun
        {
            get { return (double)GetValue(MinimunProperty); }
            set { SetValue(MinimunProperty, value); }
        }

        public static readonly DependencyProperty MinimunProperty = DependencyProperty.Register("Minimun", typeof(double), typeof(PropertySet), new PropertyMetadata(1.0));
        #endregion

        #region Maximun
        public double Maximun
        {
            get { return (double)GetValue(MaximunProperty); }
            set { SetValue(MaximunProperty, value); }
        }

        public static readonly DependencyProperty MaximunProperty = DependencyProperty.Register("Maximun", typeof(double), typeof(PropertySet), new PropertyMetadata(10.0));
        #endregion

        #region SmallChange
        public double SmallChange
        {
            get { return (double)GetValue(SmallChangeProperty); }
            set { SetValue(SmallChangeProperty, value); }
        }

        public static readonly DependencyProperty SmallChangeProperty = DependencyProperty.Register("SmallChange", typeof(double), typeof(PropertySet), new PropertyMetadata(1.0));
        #endregion

        #region ToggleOnContent
        public object ToggleOnContent
        {
            get { return (object)GetValue(ToggleOnContentProperty); }
            set { SetValue(ToggleOnContentProperty, value); }
        }

        public static readonly DependencyProperty ToggleOnContentProperty = DependencyProperty.Register("ToggleOnContent", typeof(string), typeof(PropertySet), new PropertyMetadata(null));
        #endregion

        #region ToggleOffContent
        public object ToggleOffContent
        {
            get { return (object)GetValue(ToggleOffContentProperty); }
            set { SetValue(ToggleOffContentProperty, value); }
        }
        public static readonly DependencyProperty ToggleOffContentProperty = DependencyProperty.Register("ToggleOffContent", typeof(object), typeof(PropertySet), new PropertyMetadata(null));
        #endregion

        #region ComboItems
        public IEnumerable<KeyValuePair<string, object>> ComboItems
        {
            get { return (IEnumerable<KeyValuePair<string, object>>)GetValue(ComboItemsProperty); }
            set { SetValue(ComboItemsProperty, value); }
        }

        public static readonly DependencyProperty ComboItemsProperty = DependencyProperty.Register("ComboItems", typeof(IEnumerable<KeyValuePair<string, object>>), typeof(PropertySet), new PropertyMetadata(null));
        #endregion

        #region ComboSelectionChangedCommand
        public ICommand ComboSelectionChangedCommand
        {
            get { return (ICommand)GetValue(ComboSelectionChangedCommandProperty); }
            set { SetValue(ComboSelectionChangedCommandProperty, value); }
        }

        public static readonly DependencyProperty ComboSelectionChangedCommandProperty = DependencyProperty.Register("ComboSelectionChangedCommand", typeof(ICommand), typeof(PropertySet), new PropertyMetadata(null));
        #endregion

        protected override void OnApplyTemplate()
        {
            _colors = base.GetTemplateChild("colors") as ComboBox;
            _combo = base.GetTemplateChild("combo") as ComboBox;
            _slider = base.GetTemplateChild("slider") as Slider;
            _toggle = base.GetTemplateChild("toggle") as ToggleSwitch;
            _textBox = base.GetTemplateChild("textBox") as TextBox;

            _isInitialized = true;

            ExploreProperty();
            this.SetValue(this.Value);

            if (string.IsNullOrEmpty(Label))
            {
                Label = Property;
            }
            _combo.SelectionChanged += ComboSelectionChanged;

            base.OnApplyTemplate();
        }

        private void ComboSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (ComboSelectionChangedCommand != null && ComboSelectionChangedCommand.CanExecute(cb.SelectedItem))
            {
                ComboSelectionChangedCommand.Execute(cb.SelectedItem);
            }
        }

        private static void ExploreProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PropertySet;
            control.ExploreProperty();
        }

        private void ExploreProperty()
        {
            if (_isInitialized)
            {
                if (this.Source != null && this.Property != null)
                {
                    var element = this.Source as FrameworkElement;
                    var prop = element.GetType().GetProperty(this.Property);
                    var type = prop.PropertyType;

                    if (type.IsAssignableFrom(typeof(Brush)))
                    {
                        _colors.Visibility = Visibility.Visible;
                        this.ComboItems = GetBrushItems();
                        _colors.DataContext = this;
                        SetBinding(ComboBox.SelectedValueProperty, _colors);
                        _colors.SelectedIndex = GetColorIndex(this.Value as SolidColorBrush);
                    }
                    else if (type.GetTypeInfo().IsEnum)
                    {
                        _combo.Visibility = Visibility.Visible;
                        this.ComboItems = GetEnumItems(type);
                        _combo.DataContext = this;
                        SetBinding(ComboBox.SelectedValueProperty, _combo);
                    }
                    else if (type == typeof(int) || type == typeof(double))
                    {
                        _slider.Visibility = Visibility.Visible;
                        _slider.DataContext = this;
                        SetBinding(Slider.ValueProperty, _slider);
                    }
                    else if (type == typeof(bool))
                    {
                        if (ToggleOnContent != null)
                        {
                            _toggle.OnContent = ToggleOnContent;
                        }
                        if (ToggleOffContent != null)
                        {
                            _toggle.OffContent = ToggleOffContent;
                        }
                        _toggle.Visibility = Visibility.Visible;
                        _toggle.DataContext = this;
                        SetBinding(ToggleSwitch.IsOnProperty, _toggle);
                    }
                    else if (type == typeof(string))
                    {
                        _textBox.Visibility = Visibility.Visible;
                        _textBox.DataContext = this;
                        SetBinding(TextBox.TextProperty, _textBox);
                    }
                }
            }
        }

        private IEnumerable<KeyValuePair<string, object>> GetEnumItems(Type type)
        {
            foreach (var name in Enum.GetNames(type))
            {
                yield return new KeyValuePair<string, object>(name, Enum.Parse(type, name));
            }
        }

        private IEnumerable<KeyValuePair<string, object>> GetBrushItems()
        {
            return typeof(Colors).GetRuntimeProperties().Select(r => new KeyValuePair<string, object>(Name = r.Name, new SolidColorBrush((Color)r.GetValue(null))));
        }

        private static int GetColorIndex(SolidColorBrush brush)
        {
            if (brush != null)
            {
                return new List<Color>(typeof(Colors).GetRuntimeProperties().Select(r => r.GetValue(null)).Cast<Color>()).IndexOf(brush.Color);
            }
            return -1;
        }

        private void SetBinding(DependencyProperty dp, FrameworkElement element)
        {
            var binding = new Binding
            {
                Source = this,
                Path = new PropertyPath("Value"),
                Mode = BindingMode.TwoWay
            };
            element.SetBinding(dp, binding);
        }
    }
}
