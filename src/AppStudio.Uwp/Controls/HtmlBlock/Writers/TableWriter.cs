using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppStudio.Uwp.Html;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI;

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

                BindingOperations.SetBinding(grid, Grid.HorizontalAlignmentProperty, CreateBinding(style.Table, "HorizontalAlignment"));

                foreach (var columnDefinition in grid.ColumnDefinitions)
                {
                    BindingOperations.SetBinding(columnDefinition, ColumnDefinition.WidthProperty, CreateBinding(style.Table, "ColumnWidth"));
                }

                ApplyContainerStyles(grid, style.Table);
            }
        }
    }
}
