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
        //TODO: COMPILE THIS?
        private const string RegexPattern = "</?(?<tag>\\w+)((\\s+(?<attrName>\\w+)(\\s*=\\s*(?:\"(?<attrValue>.*?)\"|'.*?'|[^'\">\\s]+))?)+\\s*|\\s*)/?>";

        private Regex _regex;
        private Match _match;

        public string Document { get; }
        public HtmlTag CurrentTag { get; private set; }
        public HtmlTag PreviousTag { get; private set; }

        private TagReader(string document)
        {
            Document = document;
            _regex = new Regex(RegexPattern);
        }

        public static TagReader Create(string document)
        {
            return new TagReader(document);
        }

        public bool Read()
        {
            if (_match == null)
            {
                _match = _regex.Match(Document);
            }
            else
            {
                _match = _match.NextMatch();
            }
            if (!_match.Success)
            {
                return false;
            }
            var tag = new HtmlTag
            {
                Name = _match.Groups["tag"].Value,
                TagType = GetTagType(_match.Value),
                StartIndex = _match.Index,
                Length = _match.Length
            };

            //TODO: ASUMMING THE SAME NUMBER OF
            var attrName = _match.Groups["attrName"];
            var attrValue = _match.Groups["attrValue"];

            if (attrName.Success)
            {
                for (int i = 0; i < attrName.Captures.Count; i++)
                {
                    tag.Attributes.Add(attrName.Captures[i].Value.ToLowerInvariant(), attrValue.Captures[i].Value);
                }
            }

            PreviousTag = CurrentTag;
            CurrentTag = tag;

            return true;
        }

        private static TagType GetTagType(string value)
        {
            if (value.StartsWith("</"))
            {
                return TagType.Close;
            }
            else if (value.EndsWith("/>"))
            {
                return TagType.AutoClose;
            }
            else
            {
                return TagType.Open;
            }
        }
    }
}
