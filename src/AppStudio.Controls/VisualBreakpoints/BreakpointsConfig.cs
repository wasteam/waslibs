using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.Controls
{
    internal class BreakpointsConfig
    {
        public string _import { get; set; }
        public BreakpointItemConfig[] breakpoints { get; set; }
    }

    internal class BreakpointItemConfig
    {
        public string maxwidth { get; set; }
        public dynamic properties { get; set; }
    }
}
