using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.Uwp.Html
{
    public sealed class HtmlText : HtmlFragment
    {
        public string Content { get; set; }

        public HtmlText()
        {
            Name = "text";
        }

        public HtmlText(string doc, int startIndex, int endIndex) : this()
        {
            if (!string.IsNullOrEmpty(doc) && endIndex - startIndex > 0)
            {
                Content = WebUtility.HtmlDecode(doc.Substring(startIndex, endIndex - startIndex));
            }
        }

        public override string ToString()
        {
            return Content;
        }

        internal static HtmlText Create(string doc, HtmlTag startTag, HtmlTag endTag)
        {
            var startIndex = 0;

            if (startTag != null)
            {
                startIndex = startTag.StartIndex + startTag.Length;
            }

            var endIndex = doc.Length;

            if (endTag != null)
            {
                endIndex = endTag.StartIndex;
            }

            var text = new HtmlText(doc, startIndex, endIndex);
            if (text != null && !string.IsNullOrEmpty(text.Content))
            {
                return text;
            }

            return null;
        }
    }
}
