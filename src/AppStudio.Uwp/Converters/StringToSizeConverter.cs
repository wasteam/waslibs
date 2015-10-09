// ***********************************************************************
// <copyright file="BoolToVisibilityConverter.cs" company="Microsoft">
//     Copyright (c) 2015 Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

/// <summary>
/// This class converts a string value into a Size for XAML property (Width, MaxWidth, Heihgt, MaxHeight).
/// </summary>
namespace AppStudio.Uwp.Converters
{
    public class StringToSizeConverter : IValueConverter
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
            if (parameter == null)
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    return (Double)0;
                }
                else
                {
                    return Double.PositiveInfinity;
                }
            }
            else
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                {
                    return Double.PositiveInfinity;
                }
                else
                {
                    return (Double)0;
                }
            }
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
            throw new NotImplementedException();
        }
    }
}
