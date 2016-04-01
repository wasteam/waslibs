using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppStudio.Uwp.Html
{
    //TODO: CHANGE NAME?
    public sealed class HtmlDocument : HtmlFragment
    {
        public HtmlDocument()
        {
            Name = "doc";
        }

        public static async Task<HtmlDocument> LoadAsync(string document)
        {
            return await Task.Run<HtmlDocument>(() =>
            {
                return Load(document);
            });
        }

        private static HtmlDocument Load(string document)
        {
            var root = new HtmlDocument();

            document = Clean(document);
            var reader = TagReader.Create(document);

            AddFragments(reader, root);

            return root;
        }

        private static void AddFragments(TagReader reader, HtmlFragment parentFragment)
        {
            while (reader.Read())
            {
                parentFragment.TryToAddText(HtmlText.Create(reader.Document, reader.PreviousTag, reader.CurrentTag));

                if (reader.CurrentTag.TagType == TagType.Close)
                {
                    return;
                }

                var node = parentFragment.AddNode(reader.CurrentTag);

                if (reader.CurrentTag.TagType == TagType.Open)
                {
                    AddFragments(reader, node);
                }
            }

        }

        private static string Clean(string doc)
        {
            //TODO: REMOVE ROOT, HEAD & BODY
            //TODO: REMOVE SPACES WHEN IDENTED
            doc = Regex.Replace(doc, @"^\r\n|\r\n\s+|\n|\n\s+|\r$", string.Empty, RegexOptions.Multiline);
            //doc = Regex.Replace(doc, @">\s+", ">");

            return doc;
        }
    }
}
