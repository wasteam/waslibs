using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace AppStudio.Controls
{
    [ContentProperty(Name = "ContentTemplate")]
    public class SectionListItem : Control
    {
        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register("ContentTemplate", typeof(DataTemplate), typeof(SectionListItem), new PropertyMetadata(null));

        public static readonly DependencyProperty ContentVisibilityProperty =
            DependencyProperty.Register("ContentVisibility", typeof(Visibility), typeof(SectionListItem), new PropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty ErrorColorProperty =
            DependencyProperty.Register("ErrorColor", typeof(Brush), typeof(SectionListItem), new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public static readonly DependencyProperty ErrorTextProperty =
            DependencyProperty.Register("ErrorText", typeof(string), typeof(SectionListItem), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ErrorVisibilityProperty =
            DependencyProperty.Register("ErrorVisibility", typeof(Visibility), typeof(SectionListItem), new PropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty HeaderLinkClickCommandProperty =
            DependencyProperty.Register("HeaderLinkClickCommand", typeof(ICommand), typeof(SectionListItem), new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderLinkStyleProperty =
            DependencyProperty.Register("HeaderLinkStyle", typeof(Style), typeof(SectionListItem), new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderLinkTextProperty =
            DependencyProperty.Register("HeaderLinkText", typeof(string), typeof(SectionListItem), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty HeaderLinkVisibilityProperty =
            DependencyProperty.Register("HeaderLinkVisibility", typeof(Visibility), typeof(SectionListItem), new PropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty LoadingTemplateProperty =
            DependencyProperty.Register("LoadingTemplate", typeof(DataTemplate), typeof(SectionListItem), new PropertyMetadata(null));

        public static readonly DependencyProperty LoadingTemplateStaticHeightProperty =
            DependencyProperty.Register("LoadingTemplateStaticHeight", typeof(double), typeof(SectionListItem), new PropertyMetadata(double.NaN));

        public static readonly DependencyProperty LoadingVisibilityProperty =
            DependencyProperty.Register("LoadingVisibility", typeof(Visibility), typeof(SectionListItem), new PropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty LoadingContainerVisibilityProperty =
            DependencyProperty.Register("LoadingContainerVisibility", typeof(Visibility), typeof(SectionListItem), new PropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(SectionListItem), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty TitleStyleProperty =
            DependencyProperty.Register("TitleStyle", typeof(Style), typeof(SectionListItem), new PropertyMetadata(null));

        public static readonly DependencyProperty TitleTemplateProperty =
            DependencyProperty.Register("TitleTemplate", typeof(DataTemplate), typeof(SectionListItem), new PropertyMetadata(null));

        public static readonly DependencyProperty TitleTemplateVisibilityProperty =
            DependencyProperty.Register("TitleTemplateVisibility", typeof(Visibility), typeof(SectionListItem), new PropertyMetadata(Visibility.Collapsed));

        public static readonly DependencyProperty TitleTextBlockVisibilityProperty =
                    DependencyProperty.Register("TitleTextBlockVisibility", typeof(Visibility), typeof(SectionListItem), new PropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty TitleVisibilityProperty =
                    DependencyProperty.Register("TitleVisibility", typeof(Visibility), typeof(SectionListItem), new PropertyMetadata(Visibility.Visible));



        public SectionListItem()
        {
            DefaultStyleKey = typeof(SectionListItem);
        }

        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }

        public Visibility ContentVisibility
        {
            get { return (Visibility)GetValue(ContentVisibilityProperty); }
            set { SetValue(ContentVisibilityProperty, value); }
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

        public ICommand HeaderLinkClickCommand
        {
            get { return (ICommand)GetValue(HeaderLinkClickCommandProperty); }
            set { SetValue(HeaderLinkClickCommandProperty, value); }
        }

        public string HeaderLinkText
        {
            get { return (string)GetValue(HeaderLinkTextProperty); }
            set { SetValue(HeaderLinkTextProperty, value); }
        }

        public Visibility HeaderLinkVisibility
        {
            get { return (Visibility)GetValue(HeaderLinkVisibilityProperty); }
            set { SetValue(HeaderLinkVisibilityProperty, value); }
        }

        public Visibility LoadingVisibility
        {
            get { return (Visibility)GetValue(LoadingVisibilityProperty); }
            set { SetValue(LoadingVisibilityProperty, value); }
        }

        public bool ManageLoading
        {
            get
            {
                var v = (Visibility)GetValue(LoadingContainerVisibilityProperty);
                return v == Visibility.Visible;
            }
            set
            {
                if (value == false)
                {
                    SetValue(LoadingContainerVisibilityProperty, Visibility.Collapsed);
                }
            }
        }

        public Visibility TitleVisibility
        {
            get { return (Visibility)GetValue(TitleVisibilityProperty); }
            set { SetValue(TitleVisibilityProperty, value); }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        internal Style HeaderLinkStyle
        {
            get { return (Style)GetValue(HeaderLinkStyleProperty); }
            set { SetValue(HeaderLinkStyleProperty, value); }
        }

        internal DataTemplate LoadingTemplate
        {
            get { return (DataTemplate)GetValue(LoadingTemplateProperty); }
            set { SetValue(LoadingTemplateProperty, value); }
        }

        internal double LoadingTemplateStaticHeight
        {
            get { return (double)GetValue(LoadingTemplateStaticHeightProperty); }
            set { SetValue(LoadingTemplateStaticHeightProperty, value); }
        }

        internal Style TitleStyle
        {
            get { return (Style)GetValue(TitleStyleProperty); }
            set { SetValue(TitleStyleProperty, value); }
        }

        internal DataTemplate TitleTemplate
        {
            get { return (DataTemplate)GetValue(TitleTemplateProperty); }
            set
            {
                SetValue(TitleTemplateProperty, value);
                SetValue(TitleTemplateVisibilityProperty, Visibility.Visible);
                SetValue(TitleTextBlockVisibilityProperty, Visibility.Collapsed);
            }
        }
    }
}