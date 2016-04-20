using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppStudio.Uwp.Html;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls.Html.Writers
{
    class TableWriter : HtmlWriter
    {
        public override string[] TargetTags
        {
            get { return new string[] { "table" }; }
        }

        public override DependencyObject GetControl(HtmlFragment fragment)
        {
            return new Grid();
        }

        public override void ApplyStyles(DocumentStyle style, DependencyObject ctrl, HtmlFragment fragment)
        {
            if (style?.Table != null)
            {
                var grid = ctrl as Grid;
                grid.HorizontalAlignment = style.Table.HorizontalAlignment;

                foreach (var columnDefinition in grid.ColumnDefinitions)
                {
                    columnDefinition.Width = style.Table.ColumnWidth;
                }

                if (!double.IsNaN(style.Table.Margin.Top))
                {
                    grid.Margin = style.Table.Margin;
                }
                if (!double.IsNaN(style.Table.Padding.Top))
                {
                    grid.Padding = style.Table.Padding;
                }
            }
        }
    }
}
