using System;

using Windows.Foundation;

namespace AppStudio.Uwp
{
    public static class SizeExtensions
    {
        public static Size ByFactorUniform(this Size size, double factor)
        {
            return ByUniform(size, 1024 * factor, 1024);
        }
        public static Size ByUniform(this Size size, double width, double height)
        {
            return ToUniform(new Size(width, height), size);
        }
        public static Size ToUniform(this Size size, Size availableSize)
        {
            return ToUniform(size, availableSize.Width, availableSize.Height);
        }
        public static Size ToUniform(this Size size, double availableWidth = double.PositiveInfinity, double availableHeight = double.PositiveInfinity)
        {
            if (double.IsInfinity(availableWidth) && double.IsInfinity(availableHeight))
            {
                return size;
            }

            double w = availableWidth;
            double h = size.Height * (w / size.Width);
            if (h <= availableHeight)
            {
                return new Size(w, h);
            }
            h = availableHeight;
            w = size.Width * (h / size.Height);

            return new Size(w, h);
        }

        public static Size ByFactorUniformToFill(this Size size, double factor)
        {
            return ByUniformToFill(size, 1024 * factor, 1024);
        }
        public static Size ByUniformToFill(this Size size, double width, double height)
        {
            return ToUniformToFill(new Size(width, height), size);
        }
        public static Size ToUniformToFill(this Size size, Size availableSize)
        {
            return ToUniformToFill(size, availableSize.Width, availableSize.Height);
        }
        public static Size ToUniformToFill(this Size size, double availableWidth = double.PositiveInfinity, double availableHeight = double.PositiveInfinity)
        {
            if (double.IsInfinity(availableWidth) && double.IsInfinity(availableHeight))
            {
                return size;
            }

            if (double.IsInfinity(availableWidth))
            {
                double h = availableHeight;
                double w = size.Width * (h / size.Height);
                return new Size(w, h);
            }

            if (double.IsInfinity(availableHeight))
            {
                double w = availableWidth;
                double h = size.Height * (w / size.Width);
                return new Size(w, h);
            }

            double ww = size.Width / availableWidth;
            double hh = size.Height / availableHeight;

            if (ww > hh)
            {
                double h = availableHeight;
                double w = size.Width * (h / size.Height);
                return new Size(w, h);
            }
            else
            {
                double w = availableWidth;
                double h = size.Height * (w / size.Width);
                return new Size(w, h);
            }
        }
    }
}
