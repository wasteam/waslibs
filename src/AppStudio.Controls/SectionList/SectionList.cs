using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace AppStudio.Controls
{
    [ContentProperty(Name = "Sections")]
    public class SectionList : Control
    {
        public static readonly DependencyProperty ErrorColorProperty =
            DependencyProperty.Register("ErrorColor", typeof(Brush), typeof(SectionList), new PropertyMetadata(null));

        public static readonly DependencyProperty ErrorTextProperty =
            DependencyProperty.Register("ErrorText", typeof(string), typeof(SectionList), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty HeaderLinkStyleProperty =
            DependencyProperty.Register("HeaderLinkStyle", typeof(Style), typeof(SectionList), new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderLinkTextProperty =
            DependencyProperty.Register("HeaderLinkText", typeof(string), typeof(SectionList), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty LoadingTemplateProperty =
            DependencyProperty.Register("LoadingTemplate", typeof(DataTemplate), typeof(SectionList), new PropertyMetadata(null));

        public static readonly DependencyProperty LoadingTemplateStaticHeightProperty =
            DependencyProperty.Register("LoadingTemplateStaticHeight", typeof(double), typeof(SectionList), new PropertyMetadata(double.NaN));

        public static readonly DependencyProperty SectionsProperty =
            DependencyProperty.Register("Sections", typeof(IList<SectionListItem>), typeof(SectionList), new PropertyMetadata(new List<SectionListItem>()));

        public static readonly DependencyProperty TitleStyleProperty =
            DependencyProperty.Register("TitleStyle", typeof(Style), typeof(SectionList), new PropertyMetadata(null));

        public static readonly DependencyProperty TitleTemplateProperty =
            DependencyProperty.Register("TitleTemplate", typeof(DataTemplate), typeof(SectionList), new PropertyMetadata(null));

        public SectionList()
        {
            Sections = new List<SectionListItem>();
            DefaultStyleKey = typeof(SectionList);
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

        public Style HeaderLinkStyle
        {
            get { return (Style)GetValue(HeaderLinkStyleProperty); }
            set { SetValue(HeaderLinkStyleProperty, value); }
        }

        public string HeaderLinkText
        {
            get { return (string)GetValue(HeaderLinkTextProperty); }
            set { SetValue(HeaderLinkTextProperty, value); }
        }

        public DataTemplate LoadingTemplate
        {
            get { return (DataTemplate)GetValue(LoadingTemplateProperty); }
            set { SetValue(LoadingTemplateProperty, value); }
        }

        public double LoadingTemplateStaticHeight
        {
            get { return (double)GetValue(LoadingTemplateStaticHeightProperty); }
            set { SetValue(LoadingTemplateStaticHeightProperty, value); }
        }

        public IList<SectionListItem> Sections
        {
            get { return (IList<SectionListItem>)GetValue(SectionsProperty); }
            set { SetValue(SectionsProperty, value); }
        }

        public Style TitleStyle
        {
            get { return (Style)GetValue(TitleStyleProperty); }
            set { SetValue(TitleStyleProperty, value); }
        }

        public DataTemplate TitleTemplate
        {
            get { return (DataTemplate)GetValue(TitleTemplateProperty); }
            set { SetValue(TitleTemplateProperty, value); }
        }
        protected override void OnApplyTemplate()
        {
            if (Sections != null && Sections.Count > 0)
            {
                foreach (var section in Sections)
                {
                    if (TitleTemplate != null)
                    {
                        section.TitleTemplate = TitleTemplate;                        
                    }
                    if (TitleStyle != null)
                    {
                        section.TitleStyle = TitleStyle;
                    }
                    if (HeaderLinkStyle != null)
                    {
                        section.HeaderLinkStyle = HeaderLinkStyle;
                    }
                    if (!string.IsNullOrEmpty(HeaderLinkText))
                    {
                        section.HeaderLinkText = HeaderLinkText;
                    }
                    if (LoadingTemplate != null)
                    {
                        section.LoadingTemplate = LoadingTemplate;
                        section.LoadingTemplateStaticHeight = LoadingTemplateStaticHeight;
                    }
                    else
                    {
                        section.LoadingTemplate = DefaultLoadingTemplate();
                    }
                    if (!string.IsNullOrEmpty(ErrorText))
                    {
                        section.ErrorText = ErrorText;
                    }
                    if (ErrorColor != null)
                    {
                        section.ErrorColor = ErrorColor;
                    }
                }
            }
            base.OnApplyTemplate();
        }
        private DataTemplate DefaultLoadingTemplate()
        {
            DataTemplate retVal = null;
            String markup = String.Empty;

            markup = "<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">";
            //markup += "<Grid>";
            //markup += "<TextBlock Text=\"{Binding RelativeSource={RelativeSource Mode=TemplatedParent}, Path=Content, Mode=OneWay}\" />";
            //markup += "</Grid>";
            markup += "<ProgressBar IsIndeterminate=\"True\"/>";
            markup += "</DataTemplate>";

            retVal = (DataTemplate)XamlReader.Load(markup);

            return retVal;
        }
    }
}
