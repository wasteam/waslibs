// ***********************************************************************
// <copyright file="BoolToVisibilityConverter.cs" company="Microsoft">
//     Copyright (c) 2015 Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AppStudio.Common.Converters
{
    using System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    /// <summary>
    /// This class converts a boolean value into a Visibility enumeration.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The type of the target property, as a type reference (System.Type for Microsoft .NET, a TypeName helper struct for Visual C++ component extensions (C++/CX)).</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Visibility visibility = Visibility.Collapsed;
            if (value is bool && (bool)value)
            {
                visibility = Visibility.Visible;
            }

            var invertResult = SafeParseBool(parameter);

            if (invertResult)
            {
                visibility = Opposite(visibility);
            }

            return visibility;
        }

        /// <summary>
        /// Modifies the target data before passing it to the source object. This method is called only in TwoWay bindings.
        /// </summary>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The type of the target property, as a type reference (System.Type for Microsoft .NET, a TypeName helper struct for Visual C++ component extensions (C++/CX)).</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the source object.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            Visibility visibility = (Visibility)value;
            bool returnValue = (visibility == Visibility.Visible) ? true : false;
            bool invertResult;
            if (parameter != null && bool.TryParse(parameter.ToString(), out invertResult))
            {
                if (invertResult)
                {
                    returnValue = !(returnValue);
                }
            }
            return returnValue;
        }

        private static bool SafeParseBool(object parameter)
        {
            var parsed = false;
            if (parameter != null)
            {
                bool.TryParse(parameter.ToString(), out parsed);
            }
            return parsed;
        }

        private static Visibility Opposite(Visibility target)
        {
            if (target == Visibility.Visible)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }
    }
}
