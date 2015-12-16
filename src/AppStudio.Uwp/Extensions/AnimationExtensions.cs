using System;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace AppStudio.Uwp
{
    public static class AnimationExtensions
    {
        public static Storyboard AnimateX(this FrameworkElement element, double x, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element.GetTranslateX() == x)
            {
                return null;
            }

            var storyboard = new Storyboard();

            var animation = new DoubleAnimation
            {
                To = x,
                Duration = TimeSpan.FromMilliseconds(duration),
                EasingFunction = easingFunction ?? new SineEase(),
                FillBehavior = FillBehavior.HoldEnd,
                EnableDependentAnimation = false
            };

            Storyboard.SetTarget(animation, element.GetCompositeTransform());
            Storyboard.SetTargetProperty(animation, "TranslateX");

            storyboard.Children.Add(animation);
            storyboard.FillBehavior = FillBehavior.HoldEnd;
            storyboard.Begin();

            return storyboard;
        }

        public static Storyboard AnimateY(this FrameworkElement element, double y, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element.GetTranslateY() == y)
            {
                return null;
            }

            var storyboard = new Storyboard();

            var animation = new DoubleAnimation
            {
                To = y,
                Duration = TimeSpan.FromMilliseconds(duration),
                EasingFunction = easingFunction ?? new SineEase(),
                FillBehavior = FillBehavior.HoldEnd,
                EnableDependentAnimation = false
            };

            Storyboard.SetTarget(animation, element.GetCompositeTransform());
            Storyboard.SetTargetProperty(animation, "TranslateY");

            storyboard.Children.Add(animation);
            storyboard.FillBehavior = FillBehavior.HoldEnd;
            storyboard.Begin();

            return storyboard;
        }

        public static Storyboard AnimateWidth(this FrameworkElement element, double width, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element.Width == width)
            {
                return null;
            }

            var storyboard = new Storyboard();

            var animation = new DoubleAnimation
            {
                To = width,
                Duration = TimeSpan.FromMilliseconds(duration),
                EasingFunction = easingFunction ?? new SineEase(),
                FillBehavior = FillBehavior.HoldEnd,
                EnableDependentAnimation = false
            };

            Storyboard.SetTarget(animation, element);
            Storyboard.SetTargetProperty(animation, "Width");

            storyboard.Children.Add(animation);
            storyboard.FillBehavior = FillBehavior.HoldEnd;
            storyboard.Begin();

            return storyboard;
        }

        public static Storyboard AnimateHeight(this FrameworkElement element, double height, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element.Height == height)
            {
                return null;
            }

            var storyboard = new Storyboard();

            var animation = new DoubleAnimation
            {
                To = height,
                Duration = TimeSpan.FromMilliseconds(duration),
                EasingFunction = easingFunction ?? new SineEase(),
                FillBehavior = FillBehavior.HoldEnd,
                EnableDependentAnimation = false
            };

            Storyboard.SetTarget(animation, element);
            Storyboard.SetTargetProperty(animation, "Height");

            storyboard.Children.Add(animation);
            storyboard.FillBehavior = FillBehavior.HoldEnd;
            storyboard.Begin();

            return storyboard;
        }

        public static Storyboard FadeIn(this FrameworkElement element, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            return AnimateOpacity(element, 1.0, duration, easingFunction);
        }
        public static Storyboard FadeOut(this FrameworkElement element, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            return AnimateOpacity(element, 0.0, duration, easingFunction);
        }
        public static Storyboard AnimateOpacity(this FrameworkElement element, double to, double duration = 250, EasingFunctionBase easingFunction = null)
        {
            if (element.Opacity == to)
            {
                return null;
            }

            var storyboard = new Storyboard();

            var animation = new DoubleAnimation
            {
                To = to,
                Duration = TimeSpan.FromMilliseconds(duration),
                EasingFunction = easingFunction ?? new SineEase(),
                FillBehavior = FillBehavior.HoldEnd,
                EnableDependentAnimation = false
            };

            Storyboard.SetTarget(animation, element);
            Storyboard.SetTargetProperty(animation, "Opacity");

            storyboard.Children.Add(animation);
            storyboard.FillBehavior = FillBehavior.HoldEnd;
            storyboard.Begin();

            return storyboard;
        }
    }
}
