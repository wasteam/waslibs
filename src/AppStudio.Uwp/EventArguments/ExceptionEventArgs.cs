using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.Uwp.EventArguments
{
    public  class ExceptionEventArgs : EventArgs
    {
        public Exception Exception{ get; private set; }
        public ExceptionEventArgs(Exception ex)
        {
            this.Exception = ex;
        }
    }
}
