using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.Uwp.EventArguments
{
    public class SourceEventArgs : EventArgs
    {
        public string Source { get; }
        public SourceEventArgs(string source)
        {
            Source = source;
        }
    }
}
