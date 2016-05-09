using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;

using Newtonsoft.Json;

namespace AppStudio.Uwp.Samples
{
    public sealed class PrettifyControl : Control
    {
        private ProgressRing _progress = null;
        private WebView _webView = null;

        private bool _isInitialized = false;

        public PrettifyControl()
        {
            this.DefaultStyleKey = typeof(PrettifyControl);
        }

        #region HtmlSource
        public string HtmlSource
        {
            get { return (string)GetValue(HtmlSourceProperty); }
            set { SetValue(HtmlSourceProperty, value); }
        }

        private static void HtmlSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PrettifyControl;
            control.SetHtmlSource(e.NewValue as string);
        }

        private void SetHtmlSource(string str)
        {
            if (_isInitialized && str != null)
            {
                _webView.NavigateToString(str);
            }
        }

        public static readonly DependencyProperty HtmlSourceProperty = DependencyProperty.Register("HtmlSource", typeof(string), typeof(PrettifyControl), new PropertyMetadata(null, HtmlSourceChanged));
        #endregion

        #region CSharpSource
        public string CSharpSource
        {
            get { return (string)GetValue(CSharpSourceProperty); }
            set { SetValue(CSharpSourceProperty, value); }
        }

        private static void CSharpSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PrettifyControl;
            control.SetCSharpSource(e.NewValue as string);
        }

        private async void SetCSharpSource(string str)
        {
            if (_isInitialized && str != null)
            {
                await ShowDocument(str, "CSharp.html");
            }
        }

        public static readonly DependencyProperty CSharpSourceProperty = DependencyProperty.Register("CSharpSource", typeof(string), typeof(PrettifyControl), new PropertyMetadata(null, CSharpSourceChanged));
        #endregion

        #region XamlSource
        public string XamlSource
        {
            get { return (string)GetValue(XamlSourceProperty); }
            set { SetValue(XamlSourceProperty, value); }
        }

        private static void XamlSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PrettifyControl;
            control.SetXamlSource(e.NewValue as string);
        }

        private async void SetXamlSource(string str)
        {
            if (_isInitialized && str != null)
            {
                await ShowDocument(str, "Xaml.html");
            }
        }

        public static readonly DependencyProperty XamlSourceProperty = DependencyProperty.Register("XamlSource", typeof(string), typeof(PrettifyControl), new PropertyMetadata(null, XamlSourceChanged));
        #endregion

        #region XmlSource
        public string XmlSource
        {
            get { return (string)GetValue(XmlSourceProperty); }
            set { SetValue(XmlSourceProperty, value); }
        }

        private static void XmlSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PrettifyControl;
            control.SetXmlSource(e.NewValue as string);
        }

        private async void SetXmlSource(string str)
        {
            if (_isInitialized && str != null)
            {
                await ShowDocument(IndentXml(str), "Xml.html");
            }
        }

        public static readonly DependencyProperty XmlSourceProperty = DependencyProperty.Register("XmlSource", typeof(string), typeof(PrettifyControl), new PropertyMetadata(null, XmlSourceChanged));
        #endregion

        #region JsonSource
        public string JsonSource
        {
            get { return (string)GetValue(JsonSourceProperty); }
            set { SetValue(JsonSourceProperty, value); }
        }

        private static void JsonSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as PrettifyControl;
            control.SetJsonSource(e.NewValue as string);
        }

        private async void SetJsonSource(string str)
        {
            if (_isInitialized && str != null)
            {
                await ShowDocument(IndentJson(str), "Json.html");
            }
        }

        public static readonly DependencyProperty JsonSourceProperty = DependencyProperty.Register("JsonSource", typeof(string), typeof(PrettifyControl), new PropertyMetadata(null, JsonSourceChanged));
        #endregion

        protected override void OnApplyTemplate()
        {
            _progress = base.GetTemplateChild("progress") as ProgressRing;
            _webView = base.GetTemplateChild("webView") as WebView;

            _webView.LoadCompleted += OnLoadCompleted;

            _isInitialized = true;

            SetHtmlSource(this.HtmlSource);
            SetCSharpSource(this.CSharpSource);
            SetXamlSource(this.XamlSource);
            SetXmlSource(this.XmlSource);
            SetJsonSource(this.JsonSource);

            base.OnApplyTemplate();
        }

        private async Task ShowDocument(string docText, string pattern)
        {
            if (_webView != null)
            {
                await HideWebView();

                var patternFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/Html/{pattern}"));
                var patternText = await patternFile.ReadTextAsync();

                docText = docText.Replace("<", "&lt;").Replace(">", "&gt;");

                string html = patternText.Replace("[CODE]", docText);

                _webView.NavigateToString(html);
            }
        }

        private void OnLoadCompleted(object sender, NavigationEventArgs e)
        {
            ShowWebView();
        }

        #region Indent Document
        private static string IndentXml(string xml)
        {
            try
            {
                var xdoc = XDocument.Parse(xml);

                var sb = new StringBuilder();
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\r\n",
                    NewLineHandling = NewLineHandling.Replace
                };

                using (XmlWriter writer = XmlWriter.Create(sb, settings))
                {
                    xdoc.Save(writer);
                }

                return sb.ToString();
            }
            catch
            {
                return xml;
            }
        }

        private static string IndentJson(string json)
        {
            try
            {
                var parsedJson = JsonConvert.DeserializeObject(json);
                string indented = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
                return indented;
            }
            catch (Exception)
            {
                return json;
            }
        }
        #endregion

        #region Show/Hide WebView
        private void ShowWebView()
        {
            _webView.Opacity = 0.0;
            _progress.IsActive = false;
            _webView.Visibility = Visibility.Visible;
            _webView.FadeIn();
        }

        private async Task HideWebView()
        {
            _webView.Opacity = 0.0;
            _progress.IsActive = true;
            _webView.Visibility = Visibility.Collapsed;
            await Task.Delay(250);
        }
        #endregion
    }
}
