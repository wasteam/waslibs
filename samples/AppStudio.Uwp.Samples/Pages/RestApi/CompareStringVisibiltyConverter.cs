using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AppStudio.Uwp.Samples
{
    public class CompareStringVisibiltyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()) || parameter == null)
            {
                return Visibility.Collapsed;
            }
            else
            {
                string[] parameters = parameter.ToString().Split(';');
                if (parameters.Length > 0)
                {
                    foreach (string param in parameters)
                    {
                        if (param.Equals(value.ToString()))
                        {
                            return Visibility.Visible;
                        }
                    }
                }
                else if (value.ToString().Equals(parameter.ToString()))
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
