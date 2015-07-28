using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Common.Commands
{
    public static class SectionHeaderClickCommand
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command", 
            typeof(ICommand),
            typeof(SectionHeaderClickCommand), 
            new PropertyMetadata(null, OnCommandPropertyChanged));

        public static void SetCommand(DependencyObject dependencyObject, ICommand value)
        {
            if (dependencyObject != null)
            {
                dependencyObject.SetValue(CommandProperty, value);
            }
        }

        public static ICommand GetCommand(DependencyObject dependencyObject)
        {
            return dependencyObject == null ? null : (ICommand)dependencyObject.GetValue(CommandProperty);
        }

        private static void OnCommandPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var control = dependencyObject as Hub;
            if (control != null)
            {
                control.SectionHeaderClick += OnSectionHeaderClick;
            }
        }

        private static void OnSectionHeaderClick(object sender, HubSectionHeaderClickEventArgs args)
        {
            var control = sender as Hub;
            var command = GetCommand(control);

            if (command != null && command.CanExecute(args.Section.DataContext))
            {
                command.Execute(args.Section.DataContext);
            }
        }
    }
}
