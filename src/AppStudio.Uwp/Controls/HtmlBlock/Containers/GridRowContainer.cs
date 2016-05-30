using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls.Html.Containers
{
    class GridRowContainer : DocumentContainer<GridRow>
    {
        private int _columnIndex = 0;

        public GridRowContainer(GridRow ctrl) : base(ctrl)
        {
        }

        public override bool CanContain(DependencyObject ctrl)
        {
            return ctrl is GridColumn;
        }

        protected override void Add(DependencyObject ctrl)
        {
            if (Control.Container != null && _columnIndex >= Control.Container.ColumnDefinitions.Count)
            {
                Control.Container.ColumnDefinitions.Add(new ColumnDefinition());
            }

            var column = ctrl as GridColumn;

            column.Index = _columnIndex++;
            column.Row = Control;
        }
    }
}
