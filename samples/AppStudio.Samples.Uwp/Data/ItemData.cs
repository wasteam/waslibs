using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace AppStudio.Samples.Uwp
{
    public class ItemData
    {
        private Random _random = new Random();

        public ItemData(int n, bool hasChildren = true)
        {
            Id = n;
            Date = DateTime.Parse("2000/01/03").AddDays(n).DayOfWeek.ToString();

            Title = String.Format("{0:00} {1}", n, SystemColors.Keys.ToArray()[n]);
            Summary = Lorems[n % 7].Substring(0, 100);
            Body = Lorems[(n + 1) % 7];

            ImageUrl = $"/Images/Sample0{n % 5 + 1}.jpg";
            Color = new SolidColorBrush(SystemColors.Values.ToArray()[n]);

            if (hasChildren)
            {
                SubItems = new List<ItemData>();
                for (int m = 0; m < _random.Next(2, 10); m++)
                {
                    SubItems.Add(new ItemData((n + m) % 5, false));
                }
            }
        }

        public int Id { get; set; }

        public string Date { get; set; }

        public string Title { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }

        public string ImageUrl { get; set; }
        public Brush Color { get; set; }

        public List<ItemData> SubItems { get; set; }

        #region SystemColors
        static private Dictionary<string, Color> _systemColors = null;

        static public Dictionary<string, Color> SystemColors
        {
            get { return _systemColors ?? (_systemColors = BuildSystemColors()); }
        }

        static private Dictionary<string, Color> BuildSystemColors()
        {
            var colors = typeof(Colors).GetRuntimeProperties().Select(c => new { Color = (Color)c.GetValue(null), Name = c.Name });
            return colors.ToDictionary(x => x.Name, x => x.Color);
        }
        #endregion

        #region Lorems
        string[] Lorems = new[] {
            "Etiam ac diam nisi, et mattis diam. Praesent congue porta libero a feugiat. Suspendisse in nisl et nisl gravida faucibus. Quisque sagittis sodales faucibus. Integer rhoncus interdum mi, a gravida orci eleifend id. Curabitur tristique, lacus sed volutpat imperdiet, nisl velit pretium sem, et scelerisque quam nibh at libero. Donec felis enim, ornare at molestie ac, fringilla quis dolor. Integer nec orci vel purus sodales aliquet. Quisque ac erat mi. Maecenas in ligula tellus. Quisque non sapien vel diam fringilla lacinia. In et metus odio, vel rhoncus ipsum.",
            "Quisque sollicitudin mi nec mi suscipit tincidunt laoreet tortor luctus. Nullam ante felis, hendrerit vel eleifend sed, porttitor pharetra enim. Mauris dictum tincidunt aliquet. Aliquam erat volutpat. Proin vel elit dui, faucibus ultrices mi. Nulla dictum viverra lorem, vitae varius justo dignissim eget. Morbi et odio metus. Integer id magna dui, eleifend aliquam sem. Mauris semper quam nulla. Quisque eget est libero. Integer est nunc, bibendum a sollicitudin quis, pulvinar quis dui. Suspendisse pellentesque porta tortor, tempus mollis nisl fringilla non. Nam laoreet magna ac mi iaculis pharetra ultrices urna aliquet.",
            "In hac habitasse platea dictumst. Sed iaculis, nulla congue sodales congue, purus lacus hendrerit risus, non fringilla elit neque venenatis enim. Integer vel leo libero, eget ultrices nisl. Proin sit amet dolor eget ante viverra vehicula. Nam libero mi, pretium a sagittis non, hendrerit vel nibh. Cras lacinia, nulla quis varius tincidunt, enim felis semper velit, et luctus dui nulla ut dui. Integer sit amet eros arcu, sit amet lobortis dui. Sed lectus ante, ullamcorper sed scelerisque a, viverra a arcu. Duis ut commodo ipsum. Fusce varius semper pharetra. Morbi magna nunc, malesuada vitae lacinia ac, aliquam sed massa. Maecenas tempus tincidunt rhoncus.",
            "Proin id mi ligula. Phasellus massa neque, malesuada id facilisis ac, mattis sagittis nisl. Maecenas vel mi massa. Duis in pretium elit. Nullam ut ante sapien. Sed leo dolor, pharetra ac varius nec, mattis quis purus. Nullam blandit tristique lacinia. Nam massa nibh, iaculis sit amet suscipit eu, bibendum eu felis. Etiam purus orci, sagittis non venenatis eu, semper et elit. Suspendisse dictum urna vitae sem vehicula eget lacinia eros ullamcorper. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Sed vel auctor orci. Ut eu diam a orci vulputate rutrum et sit amet dolor. Pellentesque sagittis eros id lacus iaculis in ultrices sapien varius. In convallis laoreet nisi sed pharetra. Nunc eleifend convallis pellentesque.",
            "Aliquam consectetur odio in elementum tincidunt. Mauris maximus dolor eu odio maximus, non facilisis ligula tincidunt. Aliquam accumsan sapien sed urna mollis, nec auctor neque suscipit. Donec condimentum, risus et vehicula maximus, lorem neque pharetra augue, quis euismod justo dui in velit. Nunc vel nunc bibendum, luctus tortor non, facilisis justo. Nunc sit amet sapien nisi. Aliquam purus lacus, facilisis non orci ac, gravida congue urna. Praesent sit amet blandit ipsum. Pellentesque condimentum ipsum sit amet dapibus aliquam. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi ipsum eros, lacinia eget nibh et, varius pulvinar purus. Nullam vitae est dignissim nunc finibus blandit ut at ligula. Vestibulum eu lacinia dolor. Sed vel velit felis.",
            "Praesent a nisl quis ante volutpat aliquet sit amet ac felis. Quisque dui erat, lacinia tempus lacinia at, pretium fermentum leo. Nullam ultrices eleifend arcu at laoreet. Aenean vehicula, purus sit amet egestas viverra, felis ligula pharetra odio, quis rhoncus leo odio quis odio. Curabitur pretium iaculis ex vitae posuere. Ut porta mi at dictum facilisis. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas.",
            "Quisque fringilla vestibulum leo sit amet sagittis. Nulla facilisi. Nulla pharetra lectus placerat, pulvinar nisl id, hendrerit massa. Cras eget congue lorem. Sed suscipit metus in pretium tristique. Donec urna orci, imperdiet a finibus non, imperdiet eget metus. Praesent gravida auctor nisi, sit amet imperdiet felis egestas eu. In a euismod augue. Vestibulum viverra, augue quis cursus euismod, sem risus convallis mi, ultricies consectetur mauris velit lacinia leo."
        };
        #endregion

        public static IEnumerable<ItemData> GetItems(int count)
        {
            for (int n = 0; n < count; n++)
            {
                yield return new ItemData(n);
            }
        }

        public override string ToString()
        {
            return Id.ToString();
        }
    }

    //public class ItemData
    //{
    //    private Random _random = new Random();

    //    public ItemData(int n)
    //    {
    //        Id = n;
    //        Date = DateTime.Parse("2000/01/03").AddDays(n).DayOfWeek.ToString();

    //        Title = String.Format("{0:00} {1}", n, SystemColors.Keys.ToArray()[n]);
    //        Summary = Lorems[n % 7];
    //        Body = Lorems[(n + 1) % 7];

    //        ImageUrl = $"/Images/Sample0{n % 5 + 1}.jpg";
    //        Color = new SolidColorBrush(SystemColors.Values.ToArray()[n]);

    //        SubItems = new List<int>();
    //        int count = _random.Next(10, 40);
    //        for (int m = 0; m < count; m++)
    //        {
    //            SubItems.Add(n + m);
    //        }
    //    }

    //    public int Id { get; set; }

    //    public string Date { get; set; }

    //    public string Title { get; set; }
    //    public string Summary { get; set; }
    //    public string Body { get; set; }

    //    public string ImageUrl { get; set; }
    //    public Brush Color { get; set; }

    //    public List<int> SubItems { get; set; }

    //    #region SystemColors
    //    static private Dictionary<string, Color> _systemColors = null;

    //    static public Dictionary<string, Color> SystemColors
    //    {
    //        get { return _systemColors ?? (_systemColors = BuildSystemColors()); }
    //    }

    //    static private Dictionary<string, Color> BuildSystemColors()
    //    {
    //        var colors = typeof(Colors).GetRuntimeProperties().Select(c => new { Color = (Color)c.GetValue(null), Name = c.Name });
    //        return colors.ToDictionary(x => x.Name, x => x.Color);
    //    }
    //    #endregion

    //    #region Lorems
    //    string[] Lorems = new[] {
    //        "Etiam ac diam nisi, et mattis diam. Praesent congue porta libero a feugiat. Suspendisse in nisl et nisl gravida faucibus. Quisque sagittis sodales faucibus. Integer rhoncus interdum mi, a gravida orci eleifend id. Curabitur tristique, lacus sed volutpat imperdiet, nisl velit pretium sem, et scelerisque quam nibh at libero. Donec felis enim, ornare at molestie ac, fringilla quis dolor. Integer nec orci vel purus sodales aliquet. Quisque ac erat mi. Maecenas in ligula tellus. Quisque non sapien vel diam fringilla lacinia. In et metus odio, vel rhoncus ipsum.",
    //        "Quisque sollicitudin mi nec mi suscipit tincidunt laoreet tortor luctus. Nullam ante felis, hendrerit vel eleifend sed, porttitor pharetra enim. Mauris dictum tincidunt aliquet. Aliquam erat volutpat. Proin vel elit dui, faucibus ultrices mi. Nulla dictum viverra lorem, vitae varius justo dignissim eget. Morbi et odio metus. Integer id magna dui, eleifend aliquam sem. Mauris semper quam nulla. Quisque eget est libero. Integer est nunc, bibendum a sollicitudin quis, pulvinar quis dui. Suspendisse pellentesque porta tortor, tempus mollis nisl fringilla non. Nam laoreet magna ac mi iaculis pharetra ultrices urna aliquet.",
    //        "In hac habitasse platea dictumst. Sed iaculis, nulla congue sodales congue, purus lacus hendrerit risus, non fringilla elit neque venenatis enim. Integer vel leo libero, eget ultrices nisl. Proin sit amet dolor eget ante viverra vehicula. Nam libero mi, pretium a sagittis non, hendrerit vel nibh. Cras lacinia, nulla quis varius tincidunt, enim felis semper velit, et luctus dui nulla ut dui. Integer sit amet eros arcu, sit amet lobortis dui. Sed lectus ante, ullamcorper sed scelerisque a, viverra a arcu. Duis ut commodo ipsum. Fusce varius semper pharetra. Morbi magna nunc, malesuada vitae lacinia ac, aliquam sed massa. Maecenas tempus tincidunt rhoncus.",
    //        "Proin id mi ligula. Phasellus massa neque, malesuada id facilisis ac, mattis sagittis nisl. Maecenas vel mi massa. Duis in pretium elit. Nullam ut ante sapien. Sed leo dolor, pharetra ac varius nec, mattis quis purus. Nullam blandit tristique lacinia. Nam massa nibh, iaculis sit amet suscipit eu, bibendum eu felis. Etiam purus orci, sagittis non venenatis eu, semper et elit. Suspendisse dictum urna vitae sem vehicula eget lacinia eros ullamcorper. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Sed vel auctor orci. Ut eu diam a orci vulputate rutrum et sit amet dolor. Pellentesque sagittis eros id lacus iaculis in ultrices sapien varius. In convallis laoreet nisi sed pharetra. Nunc eleifend convallis pellentesque.",
    //        "Aliquam consectetur odio in elementum tincidunt. Mauris maximus dolor eu odio maximus, non facilisis ligula tincidunt. Aliquam accumsan sapien sed urna mollis, nec auctor neque suscipit. Donec condimentum, risus et vehicula maximus, lorem neque pharetra augue, quis euismod justo dui in velit. Nunc vel nunc bibendum, luctus tortor non, facilisis justo. Nunc sit amet sapien nisi. Aliquam purus lacus, facilisis non orci ac, gravida congue urna. Praesent sit amet blandit ipsum. Pellentesque condimentum ipsum sit amet dapibus aliquam. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi ipsum eros, lacinia eget nibh et, varius pulvinar purus. Nullam vitae est dignissim nunc finibus blandit ut at ligula. Vestibulum eu lacinia dolor. Sed vel velit felis.",
    //        "Praesent a nisl quis ante volutpat aliquet sit amet ac felis. Quisque dui erat, lacinia tempus lacinia at, pretium fermentum leo. Nullam ultrices eleifend arcu at laoreet. Aenean vehicula, purus sit amet egestas viverra, felis ligula pharetra odio, quis rhoncus leo odio quis odio. Curabitur pretium iaculis ex vitae posuere. Ut porta mi at dictum facilisis. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas.",
    //        "Quisque fringilla vestibulum leo sit amet sagittis. Nulla facilisi. Nulla pharetra lectus placerat, pulvinar nisl id, hendrerit massa. Cras eget congue lorem. Sed suscipit metus in pretium tristique. Donec urna orci, imperdiet a finibus non, imperdiet eget metus. Praesent gravida auctor nisi, sit amet imperdiet felis egestas eu. In a euismod augue. Vestibulum viverra, augue quis cursus euismod, sem risus convallis mi, ultricies consectetur mauris velit lacinia leo."
    //    };
    //    #endregion

    //    public static IEnumerable<ItemData> GetItems(int count)
    //    {
    //        for (int n = 0; n < count; n++)
    //        {
    //            yield return new ItemData(n);
    //        }
    //    }

    //    public override string ToString()
    //    {
    //        return Id.ToString();
    //    }
    //}
}
