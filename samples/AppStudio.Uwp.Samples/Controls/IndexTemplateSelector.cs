using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Samples.Controls
{
    public class IndexTemplateSelector : DataTemplateSelector
    {
        public DataTemplate BigItemTemplate { get; set; }
        public DataTemplate MediumItemTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            ControlDataItem controlDataItem = item as ControlDataItem;
            if (controlDataItem != null)
            {
                if (controlDataItem.Index % 3 == 0)
                {
                    return BigItemTemplate;
                }
                else
                {
                    return MediumItemTemplate;
                }
            }
            return base.SelectTemplateCore(item, container);
        }
    }
}
