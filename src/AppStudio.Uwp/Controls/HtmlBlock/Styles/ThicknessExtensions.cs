using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace AppStudio.Uwp.Controls
{
    static class ThicknessExtensions
    {
        public static Thickness Merge(this Thickness thickness, Thickness thickness2)
        {
            if (!double.IsNaN(thickness2.Top) && thickness != thickness2)
            {
                return thickness2;
            }
            else if(double.IsNaN(thickness.Top))
            {
                return new Thickness();
            }
            else
            {
                return thickness;
            }
        }
    }
}
