using AppStudio.Uwp.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace AppStudio.Uwp.Controls.Html.Writers
{
    abstract class HtmlWriter
    {
        public abstract string[] TargetTags { get; }

        public abstract DependencyObject GetControl(HtmlFragment fragment);

        public virtual bool Match(HtmlFragment fragment)
        {
            return fragment != null && !string.IsNullOrEmpty(fragment.Name) && TargetTags.Any(t => t.Equals(fragment.Name, StringComparison.CurrentCultureIgnoreCase));
        }

        public static IEnumerable<HtmlWriter> GetWriters()
        {
            yield return Singleton<ContainerWriter>.Instance;
            yield return Singleton<SpanWriter>.Instance;
            yield return Singleton<TextWriter>.Instance;
            yield return Singleton<HeaderWriter>.Instance;
            yield return Singleton<StrongWriter>.Instance;
            yield return Singleton<ImageWriter>.Instance;
            yield return Singleton<ListItemWriter>.Instance;
            yield return Singleton<CodeWriter>.Instance;
            yield return Singleton<BlockQuoteWriter>.Instance;
            yield return Singleton<AnchorWriter>.Instance;
            yield return Singleton<BrWriter>.Instance;
        }
    }
}
