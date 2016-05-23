namespace AppStudio.DataProviders.WordPress
{
    public class WordPressDataConfig
    {
        public WordPressQueryType QueryType { get; set; }

        public string Query { get; set; }

        public string FilterBy { get; set; }

        public WordPressOrderByType OrderBy { get; set; } 

        public SortDirection OrderDirection { get; set; } = SortDirection.Descending;
    }

    public enum WordPressQueryType
    {
        Posts,
        Tag,
        Category
    }

    public enum WordPressOrderByType
    {   
        None,        
        Date,
        Modified,
        Title,
        Comment_Count,
        Id
    }
}
