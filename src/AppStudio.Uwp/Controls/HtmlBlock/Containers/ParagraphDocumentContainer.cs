using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;

namespace AppStudio.Uwp.Controls.Html.Containers
{
    class ParagraphDocumentContainer : DocumentContainer<Paragraph>
    {
        public ParagraphDocumentContainer(Paragraph ctrl) : base(ctrl)
        {
        }

        public override void Add(DependencyObject ctrl)
        {
            var inline = ctrl as Inline;
            Control.Inlines.Add(inline);
        }

        public override bool CanAdd(DependencyObject ctrl)
        {
            return ctrl is Inline;
        }
    }
}
