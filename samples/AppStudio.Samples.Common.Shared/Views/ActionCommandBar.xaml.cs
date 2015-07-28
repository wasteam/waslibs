using AppStudio.Common.Actions;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AppStudio.Samples.Common
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ActionCommandBar : Page
    {
        private List<ActionInfo> _commandActions;
        public List<ActionInfo> CommandActions
        {
            get
            {
                if (_commandActions == null) { _commandActions = new List<ActionInfo>(); }
                return _commandActions;
            }
        }
        public ActionCommandBar()
        {
            this.InitializeComponent();
            CommandActions.Add(GetActionInfo("actionButton1"));
            CommandActions.Add(GetActionInfo("actionButton2"));
            CommandActions.Add(GetActionInfo("actionButton3"));
        }
        public ActionInfo GetActionInfo(string name) { return new ActionInfo() { ActionType = ActionType.Primary, Name = name, Style = "actionButtonStyle" }; }
    }
}
