using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AppStudio.Uwp.Samples.Pages.RestApi
{
    public class CompareStringVisibiltyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Visibility visibility = Visibility.Collapsed;
            if (value == null || string.IsNullOrEmpty(value.ToString()) || parameter == null)
            {
                visibility = Visibility.Collapsed;
            }
            else if (value.ToString().Equals(parameter.ToString()))
            {
                visibility = Visibility.Visible;
            }

            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
