using System;
using System.Linq;
using System.Collections.Generic;

using Windows.Foundation;

namespace AppStudio.Uwp.Labs
{
    partial class MosaicPanel
    {
        private class Distribution
        {
            private int _count;
            private double _seedWidth;

            private Rect[] _rects = null;

            public Distribution(int count, double seedWidth = 400.0)
            {
                _count = count;
                _seedWidth = seedWidth;
            }

            private Rect[] Items
            {
                get { return _rects ?? (_rects = GetItems(_seedWidth).Take(_count).ToArray()); }
            }

            private static IEnumerable<Rect> GetItems(double seedWidth)
            {
                int n = 0;
                double x = 0;
                double y = 0;

                while (true)
                {
                    double width = seedWidth;
                    yield return new Rect(x, y, width, width);
                    x += width;

                    width = seedWidth / 2.0;
                    yield return new Rect(x, y, width, width);
                    yield return new Rect(x, y + width, width, width);
                    x += width;

                    width = seedWidth * (2.0 / 3.0);
                    yield return new Rect(x, y, width, width);
                    width = width / 2.0;
                    yield return new Rect(x, y + seedWidth * (2.0 / 3.0), width, width);
                    x += width;
                    yield return new Rect(x, y + seedWidth * (2.0 / 3.0), width, width);
                    x += width;

                    if (n % 2 == 0)
                    {
                        width = seedWidth;
                        yield return new Rect(x, y, width, width);
                        x += width;
                    }

                    width = seedWidth * (2.0 / 3.0);
                    yield return new Rect(x, y + seedWidth * (1.0 / 3.0), width, width);
                    width = seedWidth * (1.0 / 3.0);
                    yield return new Rect(x, y, width, width);
                    x += width;
                    yield return new Rect(x, y, width, width);
                    x += width;

                    n++;
                }
            }

            public IEnumerable<Rect> Distribute(double availableWidth)
            {
                double y = 0;
                foreach (var row in DistributeInternal(availableWidth))
                {
                    double rowWidth = row.Max(r => r.X + r.Width);
                    double factor = availableWidth / rowWidth;

                    var scaledRow = row.Select(r => ScaleRect(r, factor, y));
                    foreach (var item in scaledRow)
                    {
                        yield return item;
                    }
                    y = scaledRow.Max(r => r.Y + r.Height);
                }
            }

            private IEnumerable<IEnumerable<Rect>> DistributeInternal(double availableWidth)
            {
                int skip = 0;
                double y = 0;

                double totalWidth = GetBunchWidth(0, _count);
                availableWidth = totalWidth / Math.Ceiling(totalWidth / availableWidth);

                availableWidth = AdjustAvailableWidth(availableWidth);

                var itemsPerRow = GetItemsPerRow(availableWidth).ToArray();
                for (int n = 0; n < itemsPerRow.Length - 1; n++)
                {
                    int count = itemsPerRow[n];
                    yield return GetBunchRects(y, skip, count);
                    skip += count;
                }
                yield return GetFinalRects(y, itemsPerRow.Last(), availableWidth);
            }

            private double AdjustAvailableWidth(double availableWidth)
            {
                var itemsPerRow = GetItemsPerRow(availableWidth).ToArray();
                int first = itemsPerRow.First();
                int last = itemsPerRow.Last();

                int times = 0;
                while (itemsPerRow.Length > 1 && first > 3 && last < first / 1.5)
                {
                    availableWidth += _seedWidth / 5.0;
                    itemsPerRow = GetItemsPerRow(availableWidth).ToArray();
                    first = itemsPerRow.First();
                    last = itemsPerRow.Last();
                    if (++times > 4)
                    {
                        break;
                    }
                }
                return availableWidth;
            }

            private IEnumerable<int> GetItemsPerRow(double availableWidth)
            {
                int n = 0;
                int skip = 0;

                while (skip < _count)
                {
                    n = GetBunchCount(skip, availableWidth);
                    yield return n;
                    skip += n;
                }
            }

            private int GetBunchCount(int skip, double availableWidth)
            {
                int n = 1;
                double width = GetBunchWidth(skip, n);

                while (width < (availableWidth + _seedWidth / 3.0) && skip + n < _count)
                {
                    n++;
                    width = GetBunchWidth(skip, n);
                }
                n++;
                while (Math.Abs(Math.Floor(width) - Math.Floor(GetBunchWidth(skip, n))) < 10 && skip + n <= _count)
                {
                    n++;
                }

                return n - 1;
            }

            private double GetBunchWidth(int skip, int take)
            {
                Rect r0 = new Rect();
                if (skip > 0)
                {
                    r0 = this.Items.Take(skip).Last();
                }
                return this.Items.Skip(skip).Take(take).Max(r => r.X + r.Width) - (r0.X + r0.Width);
            }

            private IEnumerable<Rect> GetBunchRects(double y, int skip, int take)
            {
                Rect r0 = new Rect();
                if (skip > 0)
                {
                    r0 = this.Items.Take(skip).Last();
                }

                double x0 = r0.X + r0.Width;
                foreach (var rect in this.Items.Skip(skip).Take(take))
                {
                    yield return new Rect(rect.X - x0, rect.Y + y, rect.Width, rect.Height);
                }
            }

            private static Rect ScaleRect(Rect rect, double factor, double offsetY)
            {
                return new Rect(rect.X * factor, rect.Y * factor + offsetY, rect.Width * factor, rect.Height * factor);
            }

            private IEnumerable<Rect> GetFinalRects(double y, int count, double availableWidth)
            {
                int groups = count / 3;
                int remains = count % 3;

                double x = 0;
                foreach (var item in GetThreeItems(y, _seedWidth).Take(groups * 3))
                {
                    x = item.X + item.Width;
                    yield return item;
                }

                for (int n = 0; n < remains; n++)
                {
                    yield return new Rect(x, y, _seedWidth, _seedWidth);
                    x += _seedWidth;
                }
            }

            private IEnumerable<Rect> GetThreeItems(double y, double seedWidth)
            {
                double x = 0;

                while (true)
                {
                    double width = seedWidth;
                    yield return new Rect(x, y, width, width);
                    x += width;

                    width = seedWidth / 2.0;
                    yield return new Rect(x, y, width, width);
                    yield return new Rect(x, y + width, width, width);
                    x += width;

                    width = seedWidth * (2.0 / 3.0);
                    yield return new Rect(x, y, width, width);

                    width = width / 2.0;
                    yield return new Rect(x, y + seedWidth * (2.0 / 3.0), width, width);
                    x += width;
                    yield return new Rect(x, y + seedWidth * (2.0 / 3.0), width, width);
                    x += width;
                }
            }
        }
    }
}
