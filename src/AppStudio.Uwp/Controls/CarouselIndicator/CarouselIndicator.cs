using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Controls
{
    public sealed partial class CarouselIndicator : Control
    {
        private ListBox _listBox = null;

        public CarouselIndicator()
        {
            this.DefaultStyleKey = typeof(CarouselIndicator);
        }

        #region Source
        public Carousel Source
        {
            get { return (Carousel)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(Carousel), typeof(CarouselIndicator), new PropertyMetadata(null));
        #endregion

        protected override void OnApplyTemplate()
        {
            _listBox = base.GetTemplateChild("listbox") as ListBox;
            _listBox.DataContext = this;

            base.OnApplyTemplate();
        }
    }
}
