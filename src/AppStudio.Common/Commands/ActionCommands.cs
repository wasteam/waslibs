// ***********************************************************************
// <copyright file="ActionCommands.cs" company="Microsoft">
//     Copyright (c) 2015 Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AppStudio.Common.Commands
{
    using System;
    using System.Windows.Input;
    using AppStudio.Common.Navigation;
    using Windows.ApplicationModel.DataTransfer;

    /// <summary>
    /// This class defines commands used to implement the actions.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors", Justification = "This class needs to be instantiated from XAML.")]
    public class ActionCommands
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
                        await NavigationService.NavigateTo(new Uri(string.Format("mailto:{0}", p)));
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
                        await NavigationService.NavigateTo(new Uri(string.Format("tel:{0}", p)));
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
                        await NavigationService.NavigateTo(new Uri(p));
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
                        await NavigationService.NavigateTo(new Uri("bingmaps:?collection=" + System.Net.WebUtility.UrlEncode(p) + "&lvl18", UriKind.Absolute));
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
                        await NavigationService.NavigateTo(new Uri("bingmaps:?rtp=~adr." + p, UriKind.Absolute));
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
    }
}
