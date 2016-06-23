using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.Foundation;

namespace AppStudio.Uwp
{
    public static class FrameworkElementExtensions
    {
        public static void SetVisibility(this FrameworkElement elem, bool visibility)
        {
            if (elem == null)
            {
                throw new ArgumentNullException("elem");
            }
            elem.Visibility = visibility ? Visibility.Visible : Visibility.Collapsed;
        }

        public static double GetTranslateX(this FrameworkElement elem)
        {
            return elem.GetCompositeTransform().TranslateX;
        }
        public static double GetTranslateY(this FrameworkElement elem)
        {
            return elem.GetCompositeTransform().TranslateY;
        }

        public static void TranslateX(this FrameworkElement elem, double x)
        {
            elem.GetCompositeTransform().TranslateX = x;
        }
        public static void TranslateY(this FrameworkElement elem, double y)
        {
            elem.GetCompositeTransform().TranslateY = y;
        }

        public static void TranslateDeltaX(this FrameworkElement elem, double x)
        {
            elem.GetCompositeTransform().TranslateX += x;
        }
        public static void TranslateDeltaY(this FrameworkElement elem, double y)
        {
            elem.GetCompositeTransform().TranslateY += y;
        }

        public static double GetScaleX(this FrameworkElement elem)
        {
            return elem.GetCompositeTransform().ScaleX;
        }
        public static double GetScaleY(this FrameworkElement elem)
        {
            return elem.GetCompositeTransform().ScaleY;
        }

        public static void ScaleX(this FrameworkElement elem, double x)
        {
            elem.GetCompositeTransform().ScaleX = x;
        }
        public static void ScaleY(this FrameworkElement elem, double y)
        {
            elem.GetCompositeTransform().ScaleY = y;
        }

        public static void ScaleDeltaX(this FrameworkElement elem, double x)
        {
            elem.GetCompositeTransform().ScaleX += x;
        }
        public static void ScaleDeltaY(this FrameworkElement elem, double y)
        {
            elem.GetCompositeTransform().ScaleY += y;
        }

        public static CompositeTransform GetCompositeTransform(this FrameworkElement elem)
        {
            if (elem == null)
            {
                throw new ArgumentNullException("elem");
            }

            var trans = elem.RenderTransform as CompositeTransform;
            if (trans == null)
            {
                trans = new CompositeTransform();
                elem.RenderTransform = trans;
            }
            return trans;
        }

        public static Size GetSize(this FrameworkElement elem)
        {
            if (elem == null)
            {
                throw new ArgumentNullException("elem");
            }

            return new Size(elem.ActualWidth, elem.ActualHeight);
        }

        public static void Size(this FrameworkElement elem, Size s)
        {
            if (elem == null)
            {
                throw new ArgumentNullException("elem");
            }

            elem.Width = s.Width;
            elem.Height = s.Height;
        }
    }
}
