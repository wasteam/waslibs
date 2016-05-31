using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.Uwp.EventArguments
{
    public class BoolEventArgs : EventArgs
    {
        public bool Value { get; private set; }
        public BoolEventArgs(bool value)
        {
            this.Value = value;
        }
    }
}
