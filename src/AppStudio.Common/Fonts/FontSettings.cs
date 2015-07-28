using System;
using AppStudio.Common.Commands;
using Windows.Storage;

namespace AppStudio.Common.Fonts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1053:StaticHolderTypesShouldNotHaveConstructors", Justification = "This class needs to be instantiated from XAML.")]
    public class FontSettings
    {
        static FontSettings()
        {
            Current = new FontSizeSettings();
        }

        public static RelayCommand<string> ChangeFontSizeCommand
        {
            get
            {
                return new RelayCommand<string>((s) =>
                {
                    FontSize fontSize;
                    Enum.TryParse<FontSize>(s, out fontSize);
                    Current.FontSize = fontSize;
                }); ;
            }
        }

        public static FontSizeSettings Current { get; private set; }
    }

    public class FontSizeSettings : ObservableBase
    {
        public FontSize FontSize
        {
            get
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(LocalSettingNames.TextViewerFontSize))
                {
                    FontSize fontSizes;
                    Enum.TryParse<FontSize>(ApplicationData.Current.LocalSettings.Values[LocalSettingNames.TextViewerFontSize].ToString(), out fontSizes);
                    return fontSizes;
                }
                return FontSize.Normal;
            }
            set
            {
                ApplicationData.Current.LocalSettings.Values[LocalSettingNames.TextViewerFontSize] = value.ToString();
                this.OnPropertyChanged("FontSize");
            }
        }
    }
}
