using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp
{
    public static class GridExtensions
    {
        public static UIElement GetChild(this Grid grid, int column, int row)
        {
            if (grid == null)
            {
                throw new ArgumentNullException("grid");
            }

            if (grid.Children.Count > 0)
            {
                return grid.Children
                        .OfType<FrameworkElement>()
                        .FirstOrDefault(r => Grid.GetColumn(r) == column && Grid.GetRow(r) == row); 
            }
            return null;
        }

        public static T GetChild<T>(this Grid grid, int column, int row) where T : UIElement
        {
            return grid.GetChild(column, row) as T;
        }
    }
}
