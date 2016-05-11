namespace AppStudio.DataProviders.WordPress
{
    public class WordPressDataConfig
    {
        public WordPressQueryType QueryType { get; set; }

        public string Query { get; set; }

        public string FilterBy { get; set; }

        public WordPressOrderBy OrderBy { get; set; } 

        public SortDirection Direction { get; set; } = SortDirection.Descending;
    }

    public enum WordPressQueryType
    {
        Posts,
        Tag,
        Category
    }

    public enum WordPressOrderBy
    {   
        None,        
        Date,
        Modified,
        Title,
        Comment_Count,
        Id
    }
}
