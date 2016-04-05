// ***********************************************************************
// <copyright file="EndOfScrollCommand.cs" company="Microsoft">
//     Copyright (c) 2015 Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AppStudio.Uwp.Commands
{
    using System.Windows.Input;

    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Data;

    using AppStudio.Uwp.Services;

    /// <summary>
    /// This class defines an attached property that can be used to attach a Command to a 
    /// ScrollViewer, allowing to bind any ICommand when end of scroll ends.
    /// </summary>
    public static class EndOfScrollCommand
    {
        /// <summary>
        /// Definition for the attached property.
        /// </summary>
        public static DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            "Command",
            typeof(ICommand),
            typeof(EndOfScrollCommand),
            new PropertyMetadata(null, OnCommandPropertyChanged));

        /// <summary>
        /// Sets a command into the attached property.
        /// </summary>
        /// <param name="dependencyObject">The dependency object to assign the command.</param>
        /// <param name="value">The command to attach.</param>
        public static void SetCommand(DependencyObject dependencyObject, ICommand value)
        {
            if (dependencyObject != null)
            {
                dependencyObject.SetValue(CommandProperty, value);
            }
        }

        /// <summary>
        /// Gets the command from the attached property.
        /// </summary>
        /// <param name="dependencyObject">The dependency object to query.</param>
        /// <returns>The ICommand attached.</returns>
        public static ICommand GetCommand(DependencyObject dependencyObject)
        {
            return dependencyObject == null ? null : (ICommand)dependencyObject.GetValue(CommandProperty);
        }

        /// <summary>
        /// Handles the <see cref="E:CommandPropertyChanged" /> event.
        /// </summary>
        /// <param name="dependencyObject">The dependency object changed.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnCommandPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var element = dependencyObject as FrameworkElement;
            if (element != null)
            {
                element.Loaded -= ElementLoaded;
                element.Loaded += ElementLoaded;                                
            }
        }

        private static void ElementUnloaded(object sender, RoutedEventArgs e)
        {            
            FrameworkElement element = sender as FrameworkElement;
            element.Unloaded -= ElementUnloaded;
            ScrollViewer scrollViewer = element.FindChildOfType<ScrollViewer>();
            if (scrollViewer != null)
            {
                scrollViewer = null;
            }
        }

        private static void ElementLoaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            element.Loaded -= ElementLoaded;
            ScrollViewer scrollViewer = element.FindChildOfType<ScrollViewer>();
            if (scrollViewer != null)
            {
                var listener = new BindingListenerService();
                listener.Changed += delegate
                {
                    if (scrollViewer.ScrollableHeight > 0)
                    {
                        bool isAtBottom = scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight;

                        if (isAtBottom)
                        {
                            var command = GetCommand(element);
                            if (command != null)
                            {
                                command.Execute(null);
                            }
                        }
                    }                    
                };
                Binding binding = new Binding() { Source = scrollViewer, Path = new PropertyPath("VerticalOffset") };
                listener.Attach(scrollViewer, binding);
                element.Unloaded += ElementUnloaded;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Error on EndOfScrollCommand: ScrollViewer not found.");
            }
        }
    }
}
