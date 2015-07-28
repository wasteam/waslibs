using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AppStudio.Common.Converters
{
    public class PageHeaderTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || parameter == null) return Visibility.Collapsed;
            return (value.ToString().Equals(parameter.ToString())) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
