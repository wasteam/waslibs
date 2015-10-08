using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web;

namespace AppStudio.Uwp.Controls
{
    public class ReadingWebView : UserControl
    {
        public static readonly DependencyProperty DetailContentProperty
            = DependencyProperty.Register("DetailContent", typeof(string), typeof(ReadingWebView), new PropertyMetadata(null, OnHtmlPropertyChanged));

        public static readonly DependencyProperty FlyoutEnabledProperty
            = DependencyProperty.Register("FlyoutEnabled", typeof(bool), typeof(ReadingWebView), new PropertyMetadata(false, OnHtmlPropertyChanged));

        public static readonly DependencyProperty FlyoutTemplateProperty
            = DependencyProperty.Register("FlyoutTemplate", typeof(DataTemplate), typeof(ReadingWebView), new PropertyMetadata(null, OnHtmlPropertyChanged));

        public static readonly DependencyProperty ImageUrlProperty
            = DependencyProperty.Register("ImageUrl", typeof(string), typeof(ReadingWebView), new PropertyMetadata(string.Empty, OnHtmlPropertyChanged));

        public static readonly DependencyProperty SubTitleProperty
            = DependencyProperty.Register("SubTitle", typeof(string), typeof(ReadingWebView), new PropertyMetadata(null, OnHtmlPropertyChanged));

        public static readonly DependencyProperty TitleProperty
             = DependencyProperty.Register("Title", typeof(string), typeof(ReadingWebView), new PropertyMetadata(null, OnHtmlPropertyChanged));

        public static readonly DependencyProperty ContentAlignmentProperty
            = DependencyProperty.Register("ContentAlignment", typeof(HorizontalAlignment), typeof(ReadingWebView), new PropertyMetadata(HorizontalAlignment.Center, OnHtmlPropertyChanged));

        private ContentPresenter titleContentPresenter;
        private WebView innerWebView;
        private ScrollViewer innerScrollViewer;
        private RichTextBlock innerRichTextBlock;
        private TextBlock innerTitleTextBlock;
        private TextBlock innerSubTitleTextBlock;
        private double lastScroll = 0;
        private double lastWidth = 0;

        public ReadingWebView()
        {
            InitializeUserControl(true);
            HorizontalAlignment = HorizontalAlignment.Stretch;
        }

        public event EventHandler<ReadingWebViewNavigationCompletedEventArgs> ReadingWebViewNavigationCompleted;

        public string DetailContent
        {
            get { return (string)GetValue(DetailContentProperty); }
            set { SetValue(DetailContentProperty, value); }
        }
        public bool FlyoutEnabled
        {
            get { return (bool)GetValue(FlyoutEnabledProperty); }
            set { SetValue(FlyoutEnabledProperty, value); }
        }

        public DataTemplate FlyoutTemplate
        {
            get { return (DataTemplate)GetValue(FlyoutTemplateProperty); }
            set { SetValue(FlyoutTemplateProperty, value); }
        }

        public string ImageUrl
        {
            get { return (string)GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }

        public string SubTitle
        {
            get { return (string)GetValue(SubTitleProperty); }
            set { SetValue(SubTitleProperty, value); }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public HorizontalAlignment ContentAlignment
        {
            get { return (HorizontalAlignment)GetValue(ContentAlignmentProperty); }
            set { SetValue(ContentAlignmentProperty, value); }
        }

        public async Task TryApplyFontSizes(int bodyFontSize)
        {
            try
            {
                if (!string.IsNullOrEmpty(DetailContent) && !ContainsHTML(DetailContent))
                {
                    innerRichTextBlock.FontSize = bodyFontSize;
                    innerTitleTextBlock.FontSize = bodyFontSize + 6;
                    innerSubTitleTextBlock.FontSize = bodyFontSize + 2;
                }
                else
                {
                    await innerWebView.InvokeScriptAsync("applyFontSizes", new List<string>() { bodyFontSize + "px" });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private void LoadWebViewContent()
        {
            string html = ComposeHtmlForReading();
            innerWebView.NavigateToString(html);
        }

        private void InitializeUserControl(bool instanceWebView)
        {
            if (instanceWebView)
            {
                // Initialize controls
                Grid grid = InitializeLayout();
                titleContentPresenter = InitializeTitleContainer();
                Grid.SetRow(titleContentPresenter, 0);
                grid.Children.Add(titleContentPresenter);
                innerWebView = InitializeWebView();
                Grid.SetRow(innerWebView, 1);
                grid.Children.Add(innerWebView);
                // Set the grid inside the UserControl's content
                Content = grid;
            }
            else
            {
                innerScrollViewer = InitializeScrollViewer();
                Content = innerScrollViewer;
            }
        }

        private ContentPresenter InitializeTitleContainer()
        {
            var container = new ContentPresenter();
            container.Visibility = Visibility.Collapsed;
            container.Loaded += ((sender, args) =>
            {
                var contentPresenter = sender as ContentPresenter;
                contentPresenter.ContentTemplate = FlyoutTemplate;
            });

            return container;
        }

        private static Grid InitializeLayout()
        {
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            return grid;
        }

        private WebView InitializeWebView()
        {
            var webView = new WebView(WebViewExecutionMode.SameThread) { DefaultBackgroundColor = Colors.Transparent };
            webView.NavigationStarting += NavigationStarting;
            webView.ScriptNotify += ScriptNotify;
            webView.NavigationCompleted += ((sender, e) => ReadingWebViewNavigationCompleted?.Invoke(this, new ReadingWebViewNavigationCompletedEventArgs(e)));
            return webView;
        }
        private ScrollViewer InitializeScrollViewer()
        {
            var scrollViewer = new ScrollViewer();
            var grid = new Grid() { Margin = new Thickness(12, 0, 12, 12) };
            grid.HorizontalAlignment = ContentAlignment;
            if (ContentAlignment != HorizontalAlignment.Stretch)
            {
                grid.MaxWidth = 1000;
            }
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0, GridUnitType.Star) });
            int baseFontSize = 20;
            innerTitleTextBlock = new TextBlock() { Text = Title, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 12), FontSize = baseFontSize + 6, FontWeight = FontWeights.Bold, Foreground = this.Foreground };
            innerSubTitleTextBlock = new TextBlock() { Text = SubTitle, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 12), FontSize = baseFontSize + 2, Foreground = this.Foreground };
            var imageImage = new Image() { Source = new BitmapImage() { UriSource = new Uri(ImageUrl) }, Stretch = Stretch.UniformToFill };
            innerRichTextBlock = new RichTextBlock() { TextWrapping = TextWrapping.Wrap, FontSize = baseFontSize, Foreground = this.Foreground };
            var p = new Paragraph();
            p.Inlines.Add(new Run() { Text = DetailContent });
            innerRichTextBlock.Blocks.Add(p);
            Grid.SetRow(innerTitleTextBlock, 0);
            Grid.SetRow(innerSubTitleTextBlock, 1);
            Grid.SetRow(imageImage, 2);
            Grid.SetRow(innerRichTextBlock, 3);
            grid.Children.Add(innerTitleTextBlock);
            grid.Children.Add(innerSubTitleTextBlock);
            grid.Children.Add(imageImage);
            grid.Children.Add(innerRichTextBlock);
            scrollViewer.Content = grid;
            return scrollViewer;
        }

        private static void OnHtmlPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ReadingWebView control = d as ReadingWebView;
            if (control != null && !string.IsNullOrEmpty(control.DetailContent))
            {
                if (!ContainsHTML(control.DetailContent))
                {
                    control.InitializeUserControl(false);
                }
            }
            control.LoadWebViewContent();
        }

        private static bool ContainsHTML(string input)
        {
            return !Regex.IsMatch(input, @"^(?!.*<[^>]+>).*");
        }

        private string ComposeHtmlForReading()
        {
            // Get colors
            string maxWidth = string.Empty;
            string marginLeft = string.Empty;
            string marginRight = string.Empty;

            switch (ContentAlignment)
            {
                case HorizontalAlignment.Left:
                    maxWidth = "max-width: 1000px;";
                    marginLeft = "0";
                    marginRight = "auto";
                    break;
                case HorizontalAlignment.Center:
                    maxWidth = "max-width: 1000px;";
                    marginLeft = "auto";
                    marginRight = "auto";
                    break;
                case HorizontalAlignment.Right:
                    maxWidth = "max-width: 1000px;";
                    marginLeft = "auto";
                    marginRight = "0";
                    break;
                case HorizontalAlignment.Stretch:
                    marginLeft = "auto";
                    marginRight = "auto";
                    break;
            }

            string flyoutStyle = FlyoutEnabled ? "@media screen and (max-width: 800px)  { .was-title { display: none; } }" : string.Empty;
            string title = string.IsNullOrEmpty(Title) ? string.Empty : $"<h2 class='was-title'>{Title}</h2><br/>";
            string subtitle = string.IsNullOrEmpty(SubTitle) ? string.Empty : $"<h3>{SubTitle}</h3><br/>";
            string htmlContent = string.IsNullOrEmpty(DetailContent) ? string.Empty : DetailContent.Replace("ms-appx://", "ms-appx-web://");

            string imageComponent = GetImageHtmlComponent();

            string html = GetHtmlTemplate();

            html = html.Replace("%COLOR%", GetForegroundColor());
            html = html.Replace("%BACKGROUND%", GetBackgroundColor());
            html = html.Replace("%MAXWIDTH%", maxWidth);
            html = html.Replace("%MARGINLEFT%", marginLeft);
            html = html.Replace("%MARGINRIGHT%", marginRight);
            html = html.Replace("%FLYOUTSTYLE%", flyoutStyle);
            html = html.Replace("%TITLE%", title);
            html = html.Replace("%SUBTITLE%", subtitle);
            html = html.Replace("%IMAGEURL%", imageComponent);
            html = html.Replace("%HTMLCONTENT%", htmlContent);

            return html;
        }

        private string GetImageHtmlComponent()
        {
            string htmlComponent = string.Empty;
            if (!string.IsNullOrWhiteSpace(ImageUrl))
            {
                var currentImageString = ImageUrl;
                var uri = new Uri(currentImageString, UriKind.RelativeOrAbsolute);
                if (uri != null)
                {
                    if (!uri.IsAbsoluteUri)
                    {
                        if (!currentImageString.StartsWith("/"))
                        {
                            currentImageString = $"/{currentImageString}";
                        }
                        currentImageString = $"ms-appx-web://{currentImageString}";
                    }
                    else
                    {
                        if (currentImageString.ToLower().StartsWith("ms-appx://"))
                        {
                            currentImageString = $"ms-appx-web://{currentImageString.Substring(10, currentImageString.Length - 10)}";
                        }
                    }
                    htmlComponent = $"<img src='{currentImageString}'/><br/>";
                }
            }
            return htmlComponent;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "These classes are safe to dispose")]
        private string GetHtmlTemplate()
        {
            var assembly = typeof(ReadingWebView).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream("AppStudio.Uwp.Controls.ReadingWebView.ReadingWebViewContent.htm"))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private string GetBackgroundColor()
        {
            string rgbColor = "#00FFFFFF";
            var background = Background as SolidColorBrush;
            if (background != null && background.Color != null)
            {
                var color = background.Color;
                rgbColor = $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
            }

            return rgbColor;
        }

        private string GetForegroundColor()
        {
            string rgbColor = "#000000";
            var foreground = Foreground as SolidColorBrush;
            if (foreground != null && foreground.Color != null)
            {
                var color = foreground.Color;
                rgbColor = $"#{color.R:X2}{color.G:X2}{color.B:X2}";
            }

            return rgbColor;
        }

        private void ScriptNotify(object sender, NotifyEventArgs e)
        {
            try
            {
                if (e.Value.Contains("scroll"))
                {
                    double newScroll = double.Parse(e.Value.Split(':')[1]);
                    if (lastScroll == 0)
                    {
                        lastScroll = newScroll;
                    }
                    else
                    {
                        double margin = 50;
                        if (newScroll > lastScroll + margin)
                        {
                            UpdateFlyoutVisibility(false);
                            lastScroll = newScroll;
                        }
                        else if (newScroll < lastScroll - margin)
                        {
                            UpdateFlyoutVisibility(true);
                            lastScroll = newScroll;
                        }
                    }
                }
                else if (e.Value.Contains("width"))
                {
                    double newWidth = double.Parse(e.Value.Split(':')[1]);
                    if (lastWidth != newWidth)
                    {
                        lastWidth = newWidth;
                        UpdateFlyoutVisibility(null);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private async void NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (args.Uri != null)
            {
                args.Cancel = true;
                await Windows.System.Launcher.LaunchUriAsync(args.Uri);
            }
        }

        private void UpdateFlyoutVisibility(bool? activate)
        {
            if (activate == null)
            {
                if (lastWidth <= 800)
                {
                    titleContentPresenter.Visibility = Visibility.Visible;
                }
                else
                {
                    titleContentPresenter.Visibility = Visibility.Collapsed;
                }
            }
            else if (activate == true)
            {
                if (lastWidth <= 800)
                {
                    titleContentPresenter.Visibility = Visibility.Visible;
                }
            }
            else
            {
                titleContentPresenter.Visibility = Visibility.Collapsed;
            }
        }
    }

    public class ReadingWebViewNavigationCompletedEventArgs : EventArgs
    {
        public ReadingWebViewNavigationCompletedEventArgs(WebViewNavigationCompletedEventArgs webViewEventArgs)
        {
            IsSuccess = webViewEventArgs.IsSuccess;
            Uri = webViewEventArgs.Uri;
            WebErrorStatus = webViewEventArgs.WebErrorStatus;
        }

        public bool IsSuccess { get; private set; }
        public Uri Uri { get; private set; }
        public WebErrorStatus WebErrorStatus { get; private set; }
    }
}
