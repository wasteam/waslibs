using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using AppStudio.DataProviders.Exceptions;

namespace AppStudio.DataProviders
{
    public abstract class DataProviderBase<TConfig>
    {
        TConfig _config;

        int _pageSize;

        object _parser;

        public abstract bool HasMoreItems { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an async method, so nesting generic types is necessary.")]
        public async Task<IEnumerable<TSchema>> LoadDataAsync<TSchema>(TConfig config, int maxRecords, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            if (config == null)
            {
                throw new ConfigNullException();
            }
            if (parser == null)
            {
                throw new ParserNullException();
            }
            ValidateConfig(config);

            _parser = parser;
            _config = config;
            _pageSize = maxRecords;

            var result = await GetDataAsync(config, maxRecords, parser);
            if (result == null)
            {
                return new TSchema[0];
            }
            return result;
        }

        public async Task<IEnumerable<TSchema>> LoadMoreDataAsync<TSchema>() where TSchema : SchemaBase
        {
            if (HasMoreItems)
            {
                var parser = _parser as IParser<TSchema>;
                var result = await GetMoreDataAsync(_config, _pageSize, parser);
                if (result != null)
                {
                    return result;
                }
            }
           
            return new TSchema[0];
        }

        protected abstract Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(TConfig config, int maxRecords, IParser<TSchema> parser) where TSchema : SchemaBase;
        protected abstract Task<IEnumerable<TSchema>> GetMoreDataAsync<TSchema>(TConfig config, int maxRecords, IParser<TSchema> parser) where TSchema : SchemaBase;
        protected abstract void ValidateConfig(TConfig config);
    }

    public abstract class DataProviderBase<TConfig, TSchema> : DataProviderBase<TConfig> where TSchema : SchemaBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an async method, so nesting generic types is necessary.")]
        public async Task<IEnumerable<TSchema>> LoadDataAsync(TConfig config, int maxRecords = 20)
        {
            return await LoadDataAsync(config, maxRecords, GetDefaultParser(config));
        }

        public async Task<IEnumerable<TSchema>> LoadMoreDataAsync()
        {
            return await LoadMoreDataAsync<TSchema>();
        }

        public IParser<TSchema> GetDefaultParser(TConfig config)
        {
            if (config == null)
            {
                throw new ConfigNullException();
            }
            ValidateConfig(config);

            return GetDefaultParserInternal(config);
        }
        protected abstract IParser<TSchema> GetDefaultParserInternal(TConfig config);
    }





    public abstract class DataProviderBasePagination<TConfig>
    {
        public abstract bool HasMoreItems { get; }

        protected TConfig Config { get; private set; }

        protected int PageSize { get; private set; }

        object _parser;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an async method, so nesting generic types is necessary.")]
        public async Task<IEnumerable<TSchema>> LoadDataAsync<TSchema>(TConfig config, int maxRecords, IPaginationParser<TSchema> parser) where TSchema : SchemaBase
        {
            if (config == null)
            {
                throw new ConfigNullException();
            }
            if (parser == null)
            {
                throw new ParserNullException();
            }
            ValidateConfig(config);

            _parser = parser;
            Config = config;
            PageSize = maxRecords;

            var result = await GetDataAsync(config, maxRecords, parser);
            if (result != null)
            {
                return result
                    .Take(maxRecords)
                    .ToList();
            }
            return new TSchema[0];
        }

        protected abstract Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(TConfig config, int maxRecords, IPaginationParser<TSchema> parser) where TSchema : SchemaBase;

        public virtual async Task<IEnumerable<TSchema>> LoadMoreDataAsync<TSchema>() where TSchema : SchemaBase
        {
            if (HasMoreItems)
            {
                var parser = _parser as IPaginationParser<TSchema>;
                return await LoadDataAsync(Config, PageSize, parser);
            }

            return new TSchema[0];
        }

        protected abstract void ValidateConfig(TConfig config);
    }

    public abstract class DataProviderBasePagination<TConfig, TSchema> : DataProviderBasePagination<TConfig> where TSchema : SchemaBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an async method, so nesting generic types is necessary.")]
        public async Task<IEnumerable<TSchema>> LoadDataAsync(TConfig config, int maxRecords = 20)
        {
            return await LoadDataAsync(config, maxRecords, GetDefaultParser(config));
        }

        public async Task<IEnumerable<TSchema>> LoadMoreDataAsync()
        {
            return await LoadMoreDataAsync<TSchema>();
        }

        public IPaginationParser<TSchema> GetDefaultParser(TConfig config)
        {
            if (config == null)
            {
                throw new ConfigNullException();
            }
            ValidateConfig(config);

            return GetDefaultParserInternal(config);
        }

        protected abstract IPaginationParser<TSchema> GetDefaultParserInternal(TConfig config);
    }

    public interface IParserResponse<T>
    {
        string ContinuationToken { get; }
        IEnumerable<T> GetItems();
    }

    public class ParserResponseCollection<T> : Collection<T>, IParserResponse<T>
    {
        public string ContinuationToken { get; set; }

        public IEnumerable<T> GetItems()
        {
            return Items;
        }
    }





















    #region Old DataProvider
    public abstract class DataProviderBaseOld<TConfig, TSchema> : DataProviderBaseOld<TConfig> where TSchema : SchemaBase
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an async method, so nesting generic types is necessary.")]
        public async Task<IEnumerable<TSchema>> LoadDataAsync(TConfig config, int maxRecords = 20)
        {
            return await LoadDataAsync(config, maxRecords, GetDefaultParser(config));
        }

        public IParser<TSchema> GetDefaultParser(TConfig config)
        {
            if (config == null)
            {
                throw new ConfigNullException();
            }
            ValidateConfig(config);

            return GetDefaultParserInternal(config);
        }
        protected abstract IParser<TSchema> GetDefaultParserInternal(TConfig config);
    }

    public abstract class DataProviderBaseOld<TConfig>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an async method, so nesting generic types is necessary.")]
        public async Task<IEnumerable<TSchema>> LoadDataAsync<TSchema>(TConfig config, int maxRecords, IParser<TSchema> parser) where TSchema : SchemaBase
        {
            if (config == null)
            {
                throw new ConfigNullException();
            }
            if (parser == null)
            {
                throw new ParserNullException();
            }
            ValidateConfig(config);

            var result = await GetDataAsync(config, maxRecords, parser);
            if (result != null)
            {
                return result
                    .Take(maxRecords)
                    .ToList();
            }
            return new TSchema[0];
        }

        protected abstract Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(TConfig config, int maxRecords, IParser<TSchema> parser) where TSchema : SchemaBase;
        protected abstract void ValidateConfig(TConfig config);
    }
    #endregion

    #region Poc
    //public abstract class IncrementalDataProviderBase<TConfig, TSource>
    //     where TSource : class
    //{
    //    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an async method, so nesting generic types is necessary.")]
    //    public async Task<IEnumerable<TSchema>> LoadDataAsync<TSchema>(TConfig config, int maxRecords, IParserIncremental<TSource, TSchema> parser)
    //        where TSchema : SchemaBase
    //    {
    //        if (config == null)
    //        {
    //            throw new ConfigNullException();
    //        }
    //        if (parser == null)
    //        {
    //            throw new ParserNullException();
    //        }
    //        ValidateConfig(config);

    //        var result = await GetDataAsync(config, maxRecords, parser);
    //        if (result != null)
    //        {
    //            return result
    //                .Take(maxRecords)
    //                .ToList();
    //        }
    //        return new TSchema[0];
    //    }

    //    protected abstract Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(TConfig config, int maxRecords, IParserIncremental<TSource, TSchema> parser)
    //        where TSchema : SchemaBase;

    //    protected abstract void ValidateConfig(TConfig config);
    //}

    //public abstract class IncrementalDataProviderBase<TConfig, TSource, TSchema> : IncrementalDataProviderBase<TConfig, TSource>
    //    where TSource : class
    //    where TSchema : SchemaBase
    //{
    //    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an async method, so nesting generic types is necessary.")]
    //    public async Task<IEnumerable<TSchema>> LoadDataAsync(TConfig config, int maxRecords = 20)
    //    {
    //        return await LoadDataAsync(config, maxRecords, GetDefaultParser(config));
    //    }

    //    public IParserIncremental<TSource, TSchema> GetDefaultParser(TConfig config)
    //    {
    //        if (config == null)
    //        {
    //            throw new ConfigNullException();
    //        }
    //        ValidateConfig(config);

    //        return GetDefaultParserInternal(config);
    //    }
    //    protected abstract IParserIncremental<TSource, TSchema> GetDefaultParserInternal(TConfig config);
    //}

    #endregion

}
