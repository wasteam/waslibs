using Windows.UI.Xaml.Media;
using Windows.Foundation;

namespace AppStudio.Uwp.Controls
{
    partial class Virtualbox
    {
        private void ArrangeContent()
        {
            if (_content == null)
            {
                return;
            }

            if (this.ActualWidth == 0 || this.ActualHeight == 0)
            {
                return;
            }

            var containerSize = new Size(this.ActualWidth, this.ActualHeight);
            var virtualSize = GetVirtualSize();

            _content.Width = virtualSize.Width;
            _content.Height = virtualSize.Height;

            var stretchSize = GetStretchSize(containerSize, virtualSize);

            switch (AlignmentX)
            {
                case AlignmentX.Left:
                    _transform.TranslateX = 0.0;
                    break;
                case AlignmentX.Center:
                    _transform.TranslateX = (this.ActualWidth - stretchSize.Width) / 2.0;
                    break;
                case AlignmentX.Right:
                    _transform.TranslateX = this.ActualWidth - stretchSize.Width;
                    break;
                default:
                    break;
            }

            switch (AlignmentY)
            {
                case AlignmentY.Top:
                    _transform.TranslateY = 0.0;
                    break;
                case AlignmentY.Center:
                    _transform.TranslateY = (this.ActualHeight - stretchSize.Height) / 2.0;
                    break;
                case AlignmentY.Bottom:
                    _transform.TranslateY = this.ActualHeight - stretchSize.Height;
                    break;
                default:
                    break;
            }

            _transform.ScaleX = stretchSize.Width / virtualSize.Width;
            _transform.ScaleY = stretchSize.Height / virtualSize.Height;

            _clip.Rect = new Rect(new Point(), containerSize);

            this.InvalidateMeasure();
        }

        private Size GetVirtualSize()
        {
            if (this.VirtualWidth > 0 && this.VirtualHeight > 0)
            {
                return new Size(this.VirtualWidth, this.VirtualHeight);
            }

            if (this.VirtualWidth > 0)
            {
                return new Size(this.VirtualWidth, this.VirtualWidth);
            }
            if (this.VirtualHeight > 0)
            {
                return new Size(this.VirtualHeight, this.VirtualHeight);
            }

            return new Size(512.0, 512.0);
        }

        private Size GetStretchSize(Size containerSize, Size virtualSize)
        {
            switch (this.Stretch)
            {
                case Stretch.Fill:
                    return new Size(this.ActualWidth, this.ActualHeight);
                case Stretch.Uniform:
                    return virtualSize.ToUniform(containerSize);
                case Stretch.UniformToFill:
                    return virtualSize.ToUniformToFill(containerSize);
                default:
                    return virtualSize;
            }
        }
    }
}
