using System.Windows.Input;
using Windows.UI.Xaml.Navigation;

using AppStudio.Uwp.Commands;
using AppStudio.Uwp.Actions;
using System.Collections.Generic;
using Windows.UI.Xaml;
using System;
using System.Collections.ObjectModel;


namespace AppStudio.Uwp.Samples
{
    public sealed partial class ActionsCommandBarPage : Page
    {
        public ActionsCommandBarPage()
        {
            ActionCommands = new List<ActionInfo>();
            ActionCommands.Add(new ActionInfo() { ActionType = ActionType.Primary, Text = "Primary C#", Style = ActionKnownStyles.Mail });
            ActionCommands.Add(new ActionInfo() { ActionType = ActionType.Secondary, Text = "C#", Style = ActionKnownStyles.Phone });
            this.InitializeComponent();
            this.DataContext = this;
        }

        #region ActionCommands
        public List<ActionInfo> ActionCommands
        {
            get { return (List<ActionInfo>)GetValue(ActionCommandsProperty); }
            set { SetValue(ActionCommandsProperty, value); }
        }
        public static readonly DependencyProperty ActionCommandsProperty = DependencyProperty.Register("ActionCommands", typeof(List<ActionInfo>), typeof(ActionsCommandBarPage), new PropertyMetadata(null));
        #endregion
    }
}
