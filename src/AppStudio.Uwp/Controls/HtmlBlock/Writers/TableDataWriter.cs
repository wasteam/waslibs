using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppStudio.Uwp.Html;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace AppStudio.Uwp.Controls.Html.Writers
{
    class TableDataWriter : HtmlWriter
    {
        public override string[] TargetTags
        {
            get { return new string[] { "td", "th" }; }
        }

        public override DependencyObject GetControl(HtmlFragment fragment)
        {
            return new GridColumn()
            {
                ColSpan = GetSpan(fragment.AsNode(), "colspan"),
                RowSpan = GetSpan(fragment.AsNode(), "rowspan"),
            };
        }

        public override void ApplyStyles(DocumentStyle style, DependencyObject ctrl, HtmlFragment fragment)
        {
            var column = ctrl as GridColumn;
            var border = column.Row.Container.GetChild<Border>(column.Index, column.Row.Index);
            if (border != null)
            {
                var tableBorder = GetTableBorder(fragment.AsNode());
                if (tableBorder.HasValue)
                {
                    border.BorderThickness = new Thickness(tableBorder.Value);
                    border.BorderBrush = style.Table?.BorderForeground ?? new SolidColorBrush(Colors.Black);
                }
                else if (style.Table != null && !double.IsNaN(style.Table.Border.Top))
                {
                    border.BorderThickness = style.Table.Border;
                    border.BorderBrush = style.Table.BorderForeground ?? new SolidColorBrush(Colors.Black);
                }

                if (style.Table != null)
                {
                    if (!double.IsNaN(style.Td.Margin.Top))
                    {
                        border.Margin = style.Td.Margin;
                    }
                    if (!double.IsNaN(style.Td.Padding.Top))
                    {
                        border.Padding = style.Td.Padding;
                    }
                }
            }
        }

        private static int GetSpan(HtmlNode node, string name)
        {
            if (node != null)
            {
                return node.Attributes.GetValueInt(name);
            }
            return 0;
        }

        private static int? GetTableBorder(HtmlNode node)
        {
            var table = node?.Ascendant("table") as HtmlNode;

            if (table != null && !string.IsNullOrEmpty(table.Attributes.GetValue("border")))
            {
                return table.Attributes.GetValueInt("border");
            }

            return null;
        }
    }
}
