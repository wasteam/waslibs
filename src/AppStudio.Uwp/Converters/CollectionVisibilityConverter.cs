using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AppStudio.Uwp.Converters
{
    public class CollectionVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Visibility result = Visibility.Collapsed;
            if (value != null)
            {
                IEnumerable<object> collection = value as IEnumerable<object>;
                if (collection != null)
                {
                    if (collection.Count() > 0)
                    {
                        result = Visibility.Visible;
                    }
                }
            }
            if (parameter != null)
            {
                if (result == Visibility.Visible)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
