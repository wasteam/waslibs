using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AppStudio.Uwp.Controls
{
    public sealed class PropertySet : Control
    {
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

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(PropertySet), new PropertyMetadata(null));
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


        #region ComboItems
        public IEnumerable<KeyValuePair<string, object>> ComboItems
        {
            get { return (IEnumerable<KeyValuePair<string, object>>)GetValue(ComboItemsProperty); }
            set { SetValue(ComboItemsProperty, value); }
        }

        public static readonly DependencyProperty ComboItemsProperty = DependencyProperty.Register("ComboItems", typeof(IEnumerable<KeyValuePair<string, object>>), typeof(PropertySet), new PropertyMetadata(null));
        #endregion


        protected override void OnApplyTemplate()
        {
            _combo = base.GetTemplateChild("combo") as ComboBox;
            _combo.DataContext = this;
            _slider = base.GetTemplateChild("slider") as Slider;
            _slider.DataContext = this;
            _toggle = base.GetTemplateChild("toggle") as ToggleSwitch;
            _toggle.DataContext = this;
            _textBox = base.GetTemplateChild("textBox") as TextBox;
            _textBox.DataContext = this;

            _isInitialized = true;

            ExploreProperty();

            base.OnApplyTemplate();
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
                        _combo.Visibility = Visibility.Visible;
                        this.ComboItems = GetBrushItems(type);
                    }
                    else if (type.GetTypeInfo().IsEnum)
                    {
                        _combo.Visibility = Visibility.Visible;
                        this.ComboItems = GetEnumItems(type);
                    }
                    else if (type == typeof(int) || type == typeof(double))
                    {
                        _slider.Visibility = Visibility.Visible;
                    }
                    else if (type == typeof(bool))
                    {
                        _toggle.Visibility = Visibility.Visible;
                    }
                    else if (type == typeof(string))
                    {
                        _textBox.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private IEnumerable<KeyValuePair<string, object>> GetBrushItems(Type type)
        {
            return typeof(Colors).GetRuntimeProperties().Select(r => new KeyValuePair<string, object>(Name = r.Name, new SolidColorBrush((Color)r.GetValue(null))));
        }

        private IEnumerable<KeyValuePair<string, object>> GetEnumItems(Type type)
        {
            foreach (var name in Enum.GetNames(type))
            {
                yield return new KeyValuePair<string, object>(name, Enum.Parse(type, name));
            }
        }
    }
}
