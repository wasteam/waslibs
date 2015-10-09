﻿using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;

namespace AppStudio.Uwp.Actions
{
    public class ActionsCommandBar : CommandBar
    {
        public static readonly DependencyProperty ActionsSourceProperty =
            DependencyProperty.Register("ActionsSource", typeof(List<ActionInfo>), typeof(ActionsCommandBar), new PropertyMetadata(null, ActionsSourcePropertyChanged));
        public static readonly DependencyProperty HideOnLandscapeProperty =
            DependencyProperty.Register("HideOnLandscape", typeof(bool), typeof(ActionsCommandBar), new PropertyMetadata(false));
        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register("IsVisible", typeof(bool), typeof(ActionsCommandBar), new PropertyMetadata(true, OnIsVisiblePropertyChanged));

        private static void OnIsVisiblePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as ActionsCommandBar;
            bool isVisible = (bool)e.NewValue;
            if (isVisible)
            {
                self.Visibility = Visibility.Visible;
            }
            else
            {
                self.Visibility = Visibility.Collapsed;
            }
        }

        public ActionsCommandBar()
        {
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                DisplayInformation.GetForCurrentView().OrientationChanged += ((sender, args) =>
                {
                    UpdateVisibility(sender.CurrentOrientation);
                });
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Setter needed for binding.")]
        public List<ActionInfo> ActionsSource
        {
            get { return (List<ActionInfo>)GetValue(ActionsSourceProperty); }
            set { SetValue(ActionsSourceProperty, value); }
        }

        public bool HideOnLandscape
        {
            get { return (bool)GetValue(HideOnLandscapeProperty); }
            set
            {
                SetValue(HideOnLandscapeProperty, value);
            }
        }
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }        
        private static void ActionsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ActionsCommandBar;

            if (control != null)
            {
                foreach (var action in control.ActionsSource)
                {
                    var label = GetText(action, ActionTextProperties.Label);
                    var automationPropertiesName = GetText(action, ActionTextProperties.AutomationPropertiesName);
                    var button = FindButton(label, control);

                    if (button == null)
                    {
                        button = new AppBarButton();

                        if (action.ActionType == ActionType.Primary)
                        {
                            control.PrimaryCommands.Add(button);
                        }
                        else if (action.ActionType == ActionType.Secondary)
                        {
                            control.SecondaryCommands.Add(button);
                        }
                    }
                    button.Command = action.Command;
                    button.CommandParameter = action.CommandParameter;
                    button.Label = label;
                    AutomationProperties.SetName(button, automationPropertiesName);

                    if (Application.Current.Resources.ContainsKey(action.Style))
                    {
                        button.Style = Application.Current.Resources[action.Style] as Style;
                    }

                }
            }
        }
        private enum ActionTextProperties { Label, AutomationPropertiesName };
        private static string GetText(ActionInfo action, ActionTextProperties property)
        {
            var resourceLoader = new ResourceLoader();
            string text = null;

            if (!string.IsNullOrEmpty(action.Name))
            {
                if (property == ActionTextProperties.Label)
                    text = resourceLoader.GetString(string.Format("{0}/Label", action.Name));
                else if (property == ActionTextProperties.AutomationPropertiesName)
                    text = resourceLoader.GetString(string.Format("{0}/AutomationPropertiesName", action.Name));
            }
            if (string.IsNullOrEmpty(text))
            {
                if (property == ActionTextProperties.Label)
                {
                    text = action.Text;
                }
                else if (property == ActionTextProperties.AutomationPropertiesName)
                {
                    text = GetText(action, ActionTextProperties.Label);
                }
            }
            return text;
        }

        private static AppBarButton FindButton(string label, ActionsCommandBar bar)
        {
            return bar.PrimaryCommands
                            .OfType<AppBarButton>()
                            .Union(bar.SecondaryCommands.OfType<AppBarButton>())
                            .FirstOrDefault(b => b.Label == label);
        }

        private void UpdateVisibility(DisplayOrientations orientation)
        {
            if (HideOnLandscape)
            {
                if (orientation == DisplayOrientations.Landscape ||
                   orientation == DisplayOrientations.LandscapeFlipped)
                {
                    this.Visibility = Visibility.Collapsed;
                }
                else if (orientation == DisplayOrientations.Portrait ||
                        orientation == DisplayOrientations.PortraitFlipped)
                {
                    this.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
