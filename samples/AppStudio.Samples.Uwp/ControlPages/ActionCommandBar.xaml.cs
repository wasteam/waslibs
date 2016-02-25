using AppStudio.Uwp.Actions;
using AppStudio.Uwp.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Appointments;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AppStudio.Samples.Uwp.ControlPages
{
    public sealed partial class ActionCommandBar : BaseControlPage
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
            CommandActions.Add(new ActionInfo() { ActionType = ActionType.Primary, Name = "actionButton1", Style = "AppBarAddToCalendar", Command = ActionCommands.AddToCalendar, CommandParameter = new Appointment() { StartTime = DateTime.Now, Subject = "App Studio Team meeting", Duration = TimeSpan.FromMinutes(45) } });
            CommandActions.Add(new ActionInfo() { ActionType = ActionType.Primary, Name = "actionButton2", Style = "AppBarAudio" });
            CommandActions.Add(new ActionInfo() { ActionType = ActionType.Primary, Name = "actionButton3", Style = "AppBarCalculator" });
            CommandActions.Add(new ActionInfo() { ActionType = ActionType.Secondary, Name = "actionButton4", Style = "CustomAppBarButtonStyle" });
            CommandActions.Add(new ActionInfo() { ActionType = ActionType.Secondary, Name = "actionButton5", Style = "CustomAppBarButtonStyle" });            
        }        
    }
}
