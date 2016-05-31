using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.Uwp.Navigation
{
    public sealed class NavigationInitializationException : Exception
    {
        const string exceptionMessage = "NavigationService wasn't initialized. Call 'Initialize' method before use it.";
        public NavigationInitializationException() : base(exceptionMessage)
        {
        }

        public NavigationInitializationException(string s) : base($"{exceptionMessage} {s}")
        {
        }

        public NavigationInitializationException(string s, Exception ex) : base($"{exceptionMessage} {s}", ex)
        {
        }
    }
}
