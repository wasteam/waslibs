using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppStudio.Uwp.Html
{
    internal sealed class TagReader
    {
        private static readonly string[] AutoclosedTags = new string[] { "br" };

        private const string RegexPatternTag = @"</?(?<tag>\w+)\s*?(?<attr>[^>]*)*?/?>";
        private const string RegexPatternAttributes = "(?<attrName>[^\"'=]*)=?(\"|')?(?<attrValue>[^\"']*)(\"|')?";

        private Regex _regexTag;
        private Regex _regexAttributes;
        private Match _match;

        public string Document { get; }
        public HtmlTag CurrentTag { get; private set; }
        public HtmlTag PreviousTag { get; private set; }

        private TagReader(string document)
        {
            Document = document;
            _regexTag = new Regex(RegexPatternTag, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            _regexAttributes = new Regex(RegexPatternAttributes, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public static TagReader Create(string document)
        {
            return new TagReader(document);
        }

        public bool Read()
        {
            if (_match == null)
            {
                _match = _regexTag.Match(Document);
            }
            else
            {
                _match = _match.NextMatch();
            }
            if (!_match.Success)
            {
                return false;
            }
            try
            {
                var tag = new HtmlTag
                {
                    Name = _match.Groups["tag"].Value,
                    TagType = GetTagType(_match.Value, _match.Groups["tag"].Value),
                    StartIndex = _match.Index,
                    Length = _match.Length
                };

                var attributes = _match.Groups["attr"];
                if (attributes.Success)
                {
                    var attrMatches = _regexAttributes.Matches(attributes.Value);
                    for (int i = 0; i < attrMatches.Count; i++)
                    {
                        var attrMatch = attrMatches[i];
                        if (attrMatch.Success && !string.IsNullOrWhiteSpace(attrMatch.Value))
                        {
                            tag.Attributes.Add(FormatAttribute(attrMatch.Groups["attrName"].Value).ToLowerInvariant(), FormatAttribute(attrMatch.Groups["attrValue"].Value));
                        }
                    }
                }

                PreviousTag = CurrentTag;
                CurrentTag = tag;
            }
            catch (Exception ex)
            {
                throw new HtmlException($"Error reading tag {_match.Groups["tag"].Value} at index {_match.Index}", ex);
            }

            return true;
        }

        private static TagType GetTagType(string tag, string tagName)
        {
            if (tag.StartsWith("</"))
            {
                return TagType.Close;
            }
            else if (tag.EndsWith("/>") || AutoclosedTags.Contains(tagName.ToLowerInvariant()))
            {
                return TagType.AutoClose;
            }
            else
            {
                return TagType.Open;
            }
        }

        private static string FormatAttribute(string attrValue)
        {
            if (!string.IsNullOrWhiteSpace(attrValue))
            {
                return attrValue
                            .Trim()
                            .ToLowerInvariant();
            }
            return string.Empty;
        }
    }
}
