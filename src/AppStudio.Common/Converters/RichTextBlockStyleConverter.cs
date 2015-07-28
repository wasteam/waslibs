// ***********************************************************************
// <copyright file="RichTextBlockStyleConverter.cs" company="Microsoft">
//     Copyright (c) 2015 Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AppStudio.Common.Converters
{
    using System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;
    using AppStudio.Common.Fonts;

    /// <summary>
    /// This class converts a FontSize value into a a Style.
    /// </summary>
    public class RichTextBlockStyleConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the large style.
        /// </summary>
        public Style LargeStyle { get; set; }

        /// <summary>
        /// Gets or sets the normal style.
        /// </summary>
        public Style NormalStyle { get; set; }

        /// <summary>
        /// Gets or sets the small style.
        /// </summary>
        public Style SmallStyle { get; set; }

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
            var font = SafeParse(value);

            switch (font)
            {
                case FontSize.Big:
                    return LargeStyle;
                case FontSize.Small:
                    return SmallStyle;
                default:
                    return NormalStyle;
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
        /// <exception cref="NotImplementedException">Method not implemented.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        private static FontSize SafeParse(object value)
        {
            FontSize result = FontSize.Normal;
            if (value != null)
            {
                Enum.TryParse(value.ToString(), out result);
            }

            return result;
        }
    }
}