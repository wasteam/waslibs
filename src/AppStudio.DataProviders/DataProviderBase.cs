using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppStudio.DataProviders
{
    public abstract class DataProviderBase<TConfig, TSchema> where TSchema : SchemaBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an async method, so nesting generic types is necessary.")]
        public abstract Task<IEnumerable<TSchema>> LoadDataAsync(TConfig config);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an async method, so nesting generic types is necessary.")]
        public abstract Task<IEnumerable<TSchema>> LoadDataAsync(TConfig config, IParser<TSchema> parser);

        public virtual bool IsLocal
        {
            get
            {
                return false;
            }
        }
    }
}
