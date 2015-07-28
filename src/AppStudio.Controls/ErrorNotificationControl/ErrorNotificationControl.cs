using System.Windows.Input;
using AppStudio.Common.Commands;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace AppStudio.Controls
{
    public class ErrorNotificationControl : Control
    {
        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(ErrorNotificationControl), new PropertyMetadata(null));

        public static readonly DependencyProperty ErrorColorProperty =
            DependencyProperty.Register("ErrorColor", typeof(Brush), typeof(ErrorNotificationControl), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static readonly DependencyProperty ErrorTextProperty =
            DependencyProperty.Register("ErrorText", typeof(string), typeof(ErrorNotificationControl), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ErrorVisibilityProperty =
            DependencyProperty.Register("ErrorVisibility", typeof(Visibility), typeof(ErrorNotificationControl), new PropertyMetadata(Visibility.Collapsed));

        public ErrorNotificationControl()
        {
            DefaultStyleKey = typeof(ErrorNotificationControl);
            CloseCommand = new RelayCommand(() => { SetValue(ErrorVisibilityProperty, Visibility.Collapsed); });
        }

        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }

        public Brush ErrorColor
        {
            get { return (Brush)GetValue(ErrorColorProperty); }
            set { SetValue(ErrorColorProperty, value); }
        }

        public string ErrorText
        {
            get { return (string)GetValue(ErrorTextProperty); }
            set { SetValue(ErrorTextProperty, value); }
        }

        public Visibility ErrorVisibility
        {
            get { return (Visibility)GetValue(ErrorVisibilityProperty); }
            set { SetValue(ErrorVisibilityProperty, value); }
        }
    }
}
