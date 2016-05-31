using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.Uwp.Html
{
    public class HtmlException : Exception
    {
        public HtmlException()
        {
        }

        public HtmlException(string message) : base(message)
        {
        }

        public HtmlException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
