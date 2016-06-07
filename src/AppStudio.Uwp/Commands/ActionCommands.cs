// ***********************************************************************
// <copyright file="ActionCommands.cs" company="Microsoft">
//     Copyright (c) 2015 Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AppStudio.Uwp.Commands
{
    using System;
    using System.Windows.Input;
    using AppStudio.Uwp.Navigation;
    using Windows.ApplicationModel.DataTransfer;
    using Windows.ApplicationModel.Appointments;
    using Windows.UI.Xaml;
    using Windows.System;
    /// <summary>
    /// This class defines commands used to implement the actions.
    /// </summary>
    public sealed class ActionCommands
    {
        /// <summary>
        /// Gets the command used to send an email.
        /// </summary>
        public static ICommand Mailto
        {
            get
            {
                return new RelayCommand<string>(async mail =>
                {
                    if (!string.IsNullOrEmpty(mail))
                    {
                        await Launcher.LaunchUriAsync(new Uri(string.Format("mailto:{0}", mail)));
                    }
                }, (mail => !string.IsNullOrEmpty(mail)));
            }
        }

        /// <summary>
        /// Gets the command used to call to a telephone.
        /// </summary>
        public static ICommand CallToPhone
        {
            get
            {
                return new RelayCommand<string>(async phone =>
                {
                    if (!string.IsNullOrEmpty(phone))
                    {
                        await Launcher.LaunchUriAsync(new Uri(string.Format("tel:{0}", phone)));
                    }
                }, (phone => !string.IsNullOrEmpty(phone)));
            }
        }

        /// <summary>
        /// Gets the command used to navigate to an Url.
        /// </summary>
        public static ICommand NavigateToUrl
        {
            get
            {
                return new RelayCommand<string>(async url =>
                {
                    if (!string.IsNullOrEmpty(url))
                    {
                        await Launcher.LaunchUriAsync(new Uri(url));
                    }
                }, (url => !string.IsNullOrEmpty(url)));
            }
        }

        public static ICommand MapsPosition
        {
            get
            {
                return new RelayCommand<string>(async coordinates =>
                {
                    if (!string.IsNullOrEmpty(coordinates))
                    {
                        await Launcher.LaunchUriAsync(new Uri("bingmaps:" + coordinates, UriKind.Absolute));
                    }
                }, (coordinates => !string.IsNullOrEmpty(coordinates)));
            }
        }

        public static ICommand MapsHowToGet
        {
            get
            {
                return new RelayCommand<string>(async address =>
                {
                    if (!string.IsNullOrEmpty(address))
                    {
                        await Launcher.LaunchUriAsync(new Uri("bingmaps:?rtp=~adr." + address, UriKind.Absolute));
                    }
                }, (address => !string.IsNullOrEmpty(address)));
            }
        }

        public static ICommand Share
        {
            get
            {
                return new RelayCommand(() =>
                {
                    DataTransferManager.ShowShareUI();
                });
            }
        }

        /// <summary>
        /// Gets the command used to add Appointment to Calendar
        /// </summary>
        public static ICommand AddToCalendar
        {
            get
            {
                return new RelayCommand<Appointment>(async appointment =>
                {
                    if (appointment != null)
                    {
                        await AppointmentManager.ShowEditNewAppointmentAsync(appointment);
                    }
                }, (appointment => appointment != null));
            }
        }
    }
}
