using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AppStudio.Uwp
{
    public static class XamlExtensions
    {
        public static T FindChildOfType<T>(this DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);                
                T childType = child as T;
                if (childType == null)
                {
                    // The child is not of the request child type. Recursively drill down the tree.
                    foundChild = child.FindChildOfType<T>();
                    if (foundChild != null) break;
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }
            return foundChild;
        }

        public static void ScrollToTop(this DependencyObject parent)
        {
            ScrollViewer scrollViewer = parent.FindChildOfType<ScrollViewer>();
            if (scrollViewer != null)
            {
                scrollViewer.ChangeView(0, 0, scrollViewer.ZoomFactor, true);
            }
        }

        public static object Resource(this string self)
        {
            if (Application.Current.Resources.ContainsKey(self))
            {
                return Application.Current.Resources[self];
            }
            else
            {
                return null;
            }
        }
    }
}
