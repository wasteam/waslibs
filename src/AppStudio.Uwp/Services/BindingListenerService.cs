using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AppStudio.Uwp.Services
{
    public class BindingListenerService
    {
        private static int index;
        private readonly DependencyProperty property;
        private FrameworkElement target;
        public event EventHandler<BindingChangedEventArgs> Changed;

        public BindingListenerService()
        {
            property = DependencyProperty.RegisterAttached(
                "DependencyPropertyListener" + index++,
                typeof(object),
                typeof(BindingListenerService),
                new PropertyMetadata(null, (d, e) => { OnChanged(new BindingChangedEventArgs(e)); }));
        }

        public void OnChanged(BindingChangedEventArgs e)
        {
            Changed?.Invoke(target, e);
        }

        public void Attach(FrameworkElement element, Binding binding)
        {
            if(element == null)
            {
                throw new ArgumentNullException("element");
            }
            if (target != null)
            {
                throw new Exception("Cannot attach an already attached listener");
            }
            target = element;
            target.SetBinding(property, binding);
        }

        public void Detach()
        {
            target.ClearValue(property);
            target = null;
        }
    }

    public class BindingChangedEventArgs : EventArgs
    {
        public BindingChangedEventArgs(DependencyPropertyChangedEventArgs e)
        {
            EventArgs = e;
        }

        public DependencyPropertyChangedEventArgs EventArgs { get; private set; }
    }
}
