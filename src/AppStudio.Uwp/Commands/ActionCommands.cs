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
        /// Gets the command used to show an image.
        /// </summary>
        public static ICommand ShowImage
        {
            get
            {
                return new RelayCommand<string>(p =>
                {
                    if (!string.IsNullOrEmpty(p))
                    {
                        NavigationService.NavigateToPage("ImageViewer", p);
                    }
                });
            }
        }

        /// <summary>
        /// Gets the command used to send an email.
        /// </summary>
        public static ICommand Mailto
        {
            get
            {
                return new RelayCommand<string>(async p =>
                {
                    if (!string.IsNullOrEmpty(p))
                    {
                        await Launcher.LaunchUriAsync(new Uri(string.Format("mailto:{0}", p)));
                    }
                });
            }
        }

        /// <summary>
        /// Gets the command used to call to a telephone.
        /// </summary>
        public static ICommand CallToPhone
        {
            get
            {
                return new RelayCommand<string>(async p =>
                {
                    if (!string.IsNullOrEmpty(p))
                    {
                        await Launcher.LaunchUriAsync(new Uri(string.Format("tel:{0}", p)));
                    }
                });
            }
        }

        /// <summary>
        /// Gets the command used to navigate to an Url.
        /// </summary>
        public static ICommand NavigateToUrl
        {
            get
            {
                return new RelayCommand<string>(async p =>
                {
                    if (!string.IsNullOrEmpty(p))
                    {
                        await Launcher.LaunchUriAsync(new Uri(p));
                    }
                });
            }
        }

        public static ICommand MapsPosition
        {
            get
            {
                return new RelayCommand<string>(async p =>
                {
                    if (!string.IsNullOrEmpty(p))
                    {
                        await Launcher.LaunchUriAsync(new Uri("bingmaps:" + p, UriKind.Absolute));
                    }
                });
            }
        }

        public static ICommand MapsHowToGet
        {
            get
            {
                return new RelayCommand<string>(async p =>
                {
                    if (!string.IsNullOrEmpty(p))
                    {
                        await Launcher.LaunchUriAsync(new Uri("bingmaps:?rtp=~adr." + p, UriKind.Absolute));
                    }
                });
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
                });
            }
        }
    }
}
