using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AppStudio.Common.Converters
{
    public class StringToSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (parameter == null)
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    return 0;
                }
                else
                {
                    return GridUnitType.Auto;
                }
            }
            else
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    return GridUnitType.Auto;
                }
                else
                {
                    return 0;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
