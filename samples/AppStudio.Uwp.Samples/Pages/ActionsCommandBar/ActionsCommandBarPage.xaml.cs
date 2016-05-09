using System;
using System.Collections.Generic;
using System.Windows.Input;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;

using AppStudio.Uwp.Actions;
using AppStudio.Uwp.Commands;

namespace AppStudio.Uwp.Samples
{
    [SamplePage(Category = "Utilities", Name = "ActionsCommandBar", Order = 20)]
    public sealed partial class ActionsCommandBarPage : SamplePage
    {
        public ActionsCommandBarPage()
        {
            ActionCommands = new List<ActionInfo>();
            ActionCommands.Add(new ActionInfo()
            {
                ActionType = ActionType.Primary,
                Text = "Primary C#",
                Style = ActionKnownStyles.Mail,
                Command = MessageCommand,
                CommandParameter = this.GetResourceString("MessageCommandFromCode")
            });
            ActionCommands.Add(new ActionInfo()
            {
                ActionType = ActionType.Secondary,
                Text = "C#",
                Style = ActionKnownStyles.Phone,
                Command = MessageCommand,
                CommandParameter = this.GetResourceString("MessageCommandFromCode")
            });
            this.InitializeComponent();
            this.DataContext = this;
            commandBar.DataContext = this;
            paneHeader.DataContext = this;
        }

        #region ActionCommands
        public List<ActionInfo> ActionCommands
        {
            get { return (List<ActionInfo>)GetValue(ActionCommandsProperty); }
            set { SetValue(ActionCommandsProperty, value); }
        }

        public static readonly DependencyProperty ActionCommandsProperty =DependencyProperty.Register("ActionCommands", typeof(List<ActionInfo>), typeof(ActionsCommandBarPage), new PropertyMetadata(null));
        #endregion

        #region MessageCommand        
        public static ICommand MessageCommand
        {
            get
            {
                return new RelayCommand<string>(async(message) =>
                {
                    MessageDialog messageDialog = new MessageDialog(message);
                    await messageDialog.ShowAsync();
                });
            }
        }        
        #endregion

        public override string Caption
        {
            get { return "ActionsCommandBar"; }
        }
    }
}
