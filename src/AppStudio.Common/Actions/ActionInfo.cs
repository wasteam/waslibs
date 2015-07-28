using System.Windows.Input;

namespace AppStudio.Common.Actions
{
    public class ActionInfo
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string Style { get; set; }
        public ICommand Command { get; set; }
        public object CommandParameter { get; set; }
        public ActionType ActionType { get; set; }
    }

    public static class ActionKnownStyles
    {
        public const string Refresh = "AppBarRefresh";
        public const string About = "AppBarAbout";
        public const string Privacy = "AppBarPrivacy";
        public const string Share = "AppBarShare";
        public const string Link = "AppBarLink";
        public const string Phone = "AppBarPhone";
        public const string Mail = "AppBarMail";
        public const string Play = "AppBarPlay";
        public const string Directions = "AppBarDirections";
        public const string Address = "AppBarAddress";
        public const string FontSmall = "AppBarFontSizeSmall";
        public const string FontNormal = "AppBarFontSizeNormal";
        public const string FontLarge = "AppBarFontSizeLarge";
    }

    public enum ActionType
    {
        Primary,
        Secondary
    }
}
