using System.Collections.Generic;

namespace AppStudio.DataProviders
{

    public interface IParser<TSchema>
        where TSchema : SchemaBase
    {
        IEnumerable<TSchema> Parse(string data);
    }

    public interface IPaginationParser<TSchema>
        where TSchema : SchemaBase
    {
        IResponseBase<TSchema> Parse(string data);
    }
}
