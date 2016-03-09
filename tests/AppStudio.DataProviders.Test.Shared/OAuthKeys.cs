using AppStudio.DataProviders.Facebook;
using AppStudio.DataProviders.Instagram;
using AppStudio.DataProviders.Twitter;
using AppStudio.DataProviders.YouTube;

namespace AppStudio.DataProviders.Test
{
    public static class OAuthKeys
    {
        internal static FacebookOAuthTokens FacebookValidKeys = new FacebookOAuthTokens
        {
            AppId = "681022385337074",
            AppSecret = "36fad28efd9c9b84e6526da394af61df"
        };

        internal static FacebookOAuthTokens FacebookRevokedKeys = new FacebookOAuthTokens
        {
            AppId = "441136052740397",
            AppSecret = "222a7088938f1da1ada1d539289b0d04"
        };

        internal static InstagramOAuthTokens InstagramValidKeys = new InstagramOAuthTokens
        {
            ClientId = "073fae06ae95451bbf280b7b4859091a"
        };

        internal static InstagramOAuthTokens InstagramRevokedKeys = new InstagramOAuthTokens
        {
            ClientId = "1154efa3afc84e9d9289d9e361a54f4a"
        };

        internal static TwitterOAuthTokens TwitterValidKeys = new TwitterOAuthTokens
        {
            ConsumerKey = "OszocwdQB1zaFzXHlQCn4rVkZ",
            ConsumerSecret = "tehGYqkm7390zhdtDxoyEvvsuqgC3JTCsycn6E5pkQXxgzE4Av",
            AccessToken = "3223747883-e1DPeXqEoDm1JpkpTHHaHUPpw1jfGMw9CGOIK0F",
            AccessTokenSecret = "gq7nf0LCqtgdXTA6by3gx7kSkfrqG3MnXYwFTHvJW16mp"
        };

        internal static TwitterOAuthTokens TwitterRevokedKeys = new TwitterOAuthTokens
        {
            ConsumerKey = "pdm2sg4WXB8F2cgsHRrZsAKUF",
            ConsumerSecret = "rCLKeJKGIZWSX0veLXFK8A551siFsGrAIT805PdcTAY7eGzfk3",
            AccessToken = "3223747883-cuu5w6hoeBZLOhzYyY4P3TAnTYRxzGI9ipzPVC9",
            AccessTokenSecret = "YSxC2zP4b8E7mE2GxsfYcRu3abF7G3tXNFDJwof4xHLXc"
        };

        internal static YouTubeOAuthTokens YouTubeValidKeys = new YouTubeOAuthTokens
        {
            ApiKey = "AIzaSyDW1n4GxCJVOHLQfikihwFdukYtZb0Jxo4"
        };

        internal static YouTubeOAuthTokens YouTubeRevokedKeys = new YouTubeOAuthTokens
        {
            ApiKey = "AIzaSyBQnkOMJzVtlXOaXpyH3n59bR3IWuDtzQA"
        };
    }
}
