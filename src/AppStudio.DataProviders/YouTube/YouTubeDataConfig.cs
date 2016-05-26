using AppStudio.DataProviders.Core;

namespace AppStudio.DataProviders.YouTube
{
    public class YouTubeDataConfig
    {
        public YouTubeQueryType QueryType { get; set; }

        public string Query { get; set; }

        public YouTubeSearchOrderBy SearchVideosOrderBy { get; set; }
    }

    public enum YouTubeQueryType
    {
        Videos,
        Channels,
        Playlist
    }

    public class YouTubeOAuthTokens
    {
        public string ApiKey { get; set; }
    }

    public enum YouTubeSearchOrderBy
    {
        None,
        [StringValue("date")]
        Date,
        [StringValue("rating")]
        Rating,
        [StringValue("relevance")]
        Relevance,
        [StringValue("title")]
        Title,
        [StringValue("videoCount")]
        VideoCount,
        [StringValue("viewCount")]
        ViewCount
    }
}
