using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.Uwp.Navigation
{
    public sealed class NavigationInitializationException : Exception
    {
        public NavigationInitializationException() : base("NavigationService wasn't initialized. Call 'Initialize' method before use it.")
        {
        }
    }
}
