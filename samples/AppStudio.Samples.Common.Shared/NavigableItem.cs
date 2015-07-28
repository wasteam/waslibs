using AppStudio.Common.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStudio.Samples.Common
{
    public class NavigableItem : INavigable
    {
        private string _pageName;
        public string PageName
        {
            get { return _pageName; }
            set { _pageName = value; }
        }
        private NavigationInfo _navigationInfo;
        public NavigationInfo NavigationInfo
        {
            get { return _navigationInfo; }
            set { this._navigationInfo = value; }
        }

        public NavigableItem(string pageName, NavigationInfo navigationInfo)
        {
            this.PageName = pageName;
            this.NavigationInfo = navigationInfo;
        }
    }
}
