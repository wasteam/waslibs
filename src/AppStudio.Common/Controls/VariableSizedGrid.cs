// ***********************************************************************
// <copyright file="VariableSizedGrid.cs" company="Microsoft">
//     Copyright (c) 2015 Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AppStudio.Common.Controls
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    
    /// <summary>
    /// This class implement a GridView with dynamic columns and rows.
    /// </summary>
    public class VariableSizedGrid : GridView
    {
        /// <summary>
        /// The position of item at array.
        /// </summary>
       private int positionOfItemAtArray = 0;

       /// <summary>
       /// Called when the items have changed.
       /// </summary>
       /// <param name="e">The element.</param>
        protected override void OnItemsChanged(object e)
        {          
            base.OnItemsChanged(e);
        }

        /// <summary>
        /// Prepares the specified element to display the specified item.
        /// </summary>
        /// <param name="element">The element that's used to display the specified item.</param>
        /// <param name="item">The item to display.</param>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            if (element != null)
            {
                int spanProperty = this.CalculateSpanProperty();

                element.SetValue(Windows.UI.Xaml.Controls.VariableSizedWrapGrid.ColumnSpanProperty, spanProperty);
                element.SetValue(Windows.UI.Xaml.Controls.VariableSizedWrapGrid.RowSpanProperty, spanProperty);

                this.positionOfItemAtArray++;
                base.PrepareContainerForItemOverride(element, item);
            }
        }

        /// <summary>
        /// Calculates the span property.
        /// </summary>
        /// <returns>Value for span.</returns>
        private int CalculateSpanProperty()
        {
            return this.positionOfItemAtArray % 3 == 0 ? 2 : 1;
        }
    }
}
