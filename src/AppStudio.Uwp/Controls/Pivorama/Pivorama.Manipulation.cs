using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace AppStudio.Uwp.Controls
{
    partial class Pivorama
    {
        private void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            double deltaX = -e.Delta.Translation.X;

            if (e.IsInertial)
            {
                e.Complete();
            }
            else
            {
                if (Math.Abs(e.Cumulative.Translation.X) >= this.ItemWidth)
                {
                    e.Complete();
                }
                else
                {
                    Position += deltaX;
                }
            }
        }

        private void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            double offset = this.ItemWidth - Position % this.ItemWidth;

            if (e.IsInertial)
            {
                if (Math.Sign(e.Cumulative.Translation.X) < 0)
                {
                    AnimateNext();
                }
                else
                {
                    AnimatePrev();
                }
            }
            else
            {
                if (offset < this.ItemWidth / 2.0)
                {
                    AnimateNext();
                }
                else
                {
                    AnimatePrev();
                }
            }
        }


        private async void AnimateNext(double duration = 500)
        {
            double delta = this.ItemWidth - Position % this.ItemWidth;
            double position = (Position + delta);
            _headerItems.AnimateX(-position);
            // TODO: 
            //AnimateTabsRight();
            await _container.AnimateXAsync(-position);
            this.ArrangeTabs();
            this.ArrangeItems();
        }

        private async void AnimatePrev(double duration = 500)
        {
            double delta = -Position % this.ItemWidth;
            double position = (Position + delta);
            _headerItems.AnimateX(-position);
            // TODO: 
            //AnimateTabsLeft();
            await _container.AnimateXAsync(-position);
            this.ArrangeTabs();
            this.ArrangeItems();
        }
    }
}
