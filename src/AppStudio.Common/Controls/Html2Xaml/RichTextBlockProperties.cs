// ***********************************************************************
// <copyright file="RichTextBlockProperties.cs" company="Microsoft">
//     Copyright (c) 2015 Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AppStudio.Common.Controls.Html2Xaml
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Windows.ApplicationModel.Resources;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Documents;
    using Windows.UI.Xaml.Markup;

    /// <summary>
    /// Usage: 
    /// 1) In a XAML file, declare the above namespace, e.g.:
    ///    xmlns:h2xaml="using:Html2Xaml"
    ///     
    /// 2) In RichTextBlock controls, set or databind the Html property, e.g.:
    /// <code>
    ///    <RichTextBlock h2xaml:Properties.Html="{Binding ...}"/>
    ///    or
    ///    <RichTextBlock>
    ///     <h2xaml:Properties.Html>
    ///         <![CDATA[
    ///             <p>This is a list:</p>
    ///             <ul>
    ///                 <li>Item 1</li>
    ///                 <li>Item 2</li>
    ///                 <li>Item 3</li>
    ///             </ul>
    ///         ]]>
    ///     </h2xaml:Properties.Html>
    /// </RichTextBlock>
    /// </code>
    /// </summary>
    public class Properties : DependencyObject
    {
        /// <summary>
        /// Declares the HTML attached property.
        /// </summary>
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
            "Html", 
            typeof(string), 
            typeof(Properties), 
            new PropertyMetadata(null, HtmlChanged));

        /// <summary>
        /// Sets the HTML.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        public static void SetHtml(DependencyObject obj, string value)
        {
            if (obj != null)
            {
                obj.SetValue(HtmlProperty, value);
            }
        }

        /// <summary>
        /// Gets the HTML.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The HTML data.</returns>
        public static string GetHtml(DependencyObject obj)
        {
            return obj == null ? null : (string)obj.GetValue(HtmlProperty);
        }

        /// <summary>
        /// Handles the HTML has changed, converts it and attach to the attached RichTextBlock.
        /// </summary>
        /// <param name="d">The RichTextBlock.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static async void HtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RichTextBlock richText = d as RichTextBlock;
            string html = e.NewValue as string;

            if (richText != null && !string.IsNullOrEmpty(html))
            {
                try
                {
                    if (!ContainsHtmlTags(html))
                    {
                        html = html.Replace("\r\n", "<br/>");
                        html = html.Replace("\n\r", "<br/>");
                        html = html.Replace("\n", "<br/>");
                    }

                    string xaml = await Html2XamlProcessor.ConvertToXaml(html);
                    ChangeRichTextBlockContents(richText, xaml);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    try
                    {
                        ChangeRichTextBlockContents(richText, GetErrorXaml(ex, html));
                    }
                    catch
                    {
                        Debug.WriteLine(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Changes the rich text block contents.
        /// </summary>
        /// <param name="richText">The rich text.</param>
        /// <param name="xamlContents">The XAML contents.</param>
        private static void ChangeRichTextBlockContents(RichTextBlock richText, string xamlContents)
        {
            richText.Blocks.Clear();
            RichTextBlock newRichText = (RichTextBlock)XamlReader.Load(xamlContents);
            for (int i = newRichText.Blocks.Count - 1; i >= 0; i--)
            {
                Block b = newRichText.Blocks[i];
                newRichText.Blocks.RemoveAt(i);
                richText.Blocks.Insert(0, b);
            }
        }

        /// <summary>
        /// Gets the error XAML.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="html">The HTML.</param>
        /// <returns>A XAML with error string.</returns>
        private static string GetErrorXaml(Exception ex, string html)
        {
            string localizedError = GetStringForResource("Html2XamlError/Text");
            string localizedSourceHtml = GetStringForResource("Html2XamlSourceHtml/Text");

            if (string.IsNullOrEmpty(localizedError))
            {
                localizedError = "An exception occurred while converting HTML to XAML: {0}";
            }
            
            localizedError = string.Format(localizedError, ex.Message);

            if (string.IsNullOrEmpty(localizedSourceHtml))
            {
                localizedSourceHtml = "Source HTML:";
            }

            return string.Format(
                    @"<RichTextBlock xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">"
                    + @"<Paragraph>{0}</Paragraph><Paragraph /><Paragraph>{1}</Paragraph><Paragraph>{2}</Paragraph></RichTextBlock>",
                    localizedError,
                    localizedSourceHtml,
                    EncodeXml(html));
        }

        /// <summary>
        /// Encodes the XML.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns>An properly encoded XML.</returns>
        private static string EncodeXml(string xml)
        {
            return xml.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
        }

        /// <summary>
        /// Gets the string for resource.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The data for the resource.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static string GetStringForResource(string key)
        {
            try
            {
                ResourceLoader rl = ResourceLoader.GetForViewIndependentUse();
                return rl.GetString(key);
            }
            catch
            {
                return string.Empty;
            }
        }

        private static bool ContainsHtmlTag(string text, string tag)
        {
            var pattern = @"<\s*" + tag + @"\s*\/?>";
            return Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase);
        }

        private static bool ContainsHtmlTags(string text, string tags)
        {
            var ba = tags.Split('|').Select(x => new { tag = x, hastag = ContainsHtmlTag(text, x) }).Where(x => x.hastag);

            return ba.Count() > 0;
        }

        private static bool ContainsHtmlTags(string text)
        {
            return ContainsHtmlTags(
                    text, 
                    "a|abbr|acronym|address|area|b|base|bdo|big|blockquote|body|br|button|caption|cite|code|col|colgroup|dd|del|dfn|div|dl|DOCTYPE|dt|em|fieldset|form|h1|h2|h3|h4|h5|h6|head|html|hr|i|img|input|ins|kbd|label|legend|li|link|map|meta|noscript|object|ol|optgroup|option|p|param|pre|q|samp|script|select|small|span|strong|style|sub|sup|table|tbody|td|textarea|tfoot|th|thead|title|tr|tt|ul|var");
        }
    }
}
