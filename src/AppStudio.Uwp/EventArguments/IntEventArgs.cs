using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.Uwp.EventArguments
{
    public class IntEventArgs : EventArgs
    {
        public int Value { get; private set; }
        public IntEventArgs(int value)
        {
            this.Value = value;
        }
    }
}
