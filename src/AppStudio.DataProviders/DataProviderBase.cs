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
        protected TConfig Config
        {
            get { return _config; }
            set { _config = value; }
        }

        int _pageSize;
        protected int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        object _parser;
        protected object Parser
        {
            get { return _parser; }
            set { _parser = value; }
        }

        protected string ContinuationToken { get; set; }

        public virtual bool IsInitialized
        {
            get { return (_config != null && _parser != null); }
        }

        public abstract bool HasMoreItems { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "This is an async method, so nesting generic types is necessary.")]
        public async Task<IEnumerable<TSchema>> LoadDataAsync<TSchema>(TConfig config, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase
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
            _pageSize = pageSize;

            var result = await GetDataAsync(config, pageSize, parser);
            if (result == null)
            {
                return new TSchema[0];
            }
            return result;
        }

        public async Task<IEnumerable<TSchema>> LoadMoreDataAsync<TSchema>() where TSchema : SchemaBase
        {
            if (_config == null || _parser == null)
            {
                throw new InvalidOperationException("LoadMoreDataAsync can not be called. You must call the LoadDataAsync method prior to calling this method");
            }

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

        protected abstract Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(TConfig config, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase;
        protected abstract Task<IEnumerable<TSchema>> GetMoreDataAsync<TSchema>(TConfig config, int pageSize, IParser<TSchema> parser) where TSchema : SchemaBase;
        protected abstract void ValidateConfig(TConfig config);
    }

    public abstract class DataProviderBase<TConfig, TSchema> : DataProviderBase<TConfig> where TSchema : SchemaBase
    {
        public async Task<IEnumerable<TSchema>> LoadDataAsync(TConfig config, int pageSize = 20)
        {
            return await LoadDataAsync(config, pageSize, GetDefaultParser(config));
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
}
