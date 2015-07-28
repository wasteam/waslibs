using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Exceptions;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.Storage.Streams;

namespace AppStudio.DataProviders.TouchDevelop
{
    public class TouchDevelopDataProvider : DataProviderBase<TouchDevelopDataConfig, TouchDevelopSchema>
    {
        public override async Task<IEnumerable<TouchDevelopSchema>> LoadDataAsync(TouchDevelopDataConfig config)
        {
            return await LoadDataAsync(config, new TouchDevelopParser());
        }

        public override async Task<IEnumerable<TouchDevelopSchema>> LoadDataAsync(TouchDevelopDataConfig config, IParser<TouchDevelopSchema> parser)
        {
            try
            {
                TouchDevelopSchema record = await GetRemoteProject(config, parser);
                if (record == null)
                {
                    record = await GetLocalProject(config);
                }

                return new List<TouchDevelopSchema>() { record };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw new RequestFailedException();
            }
        }

        private async Task<TouchDevelopSchema> GetRemoteProject(TouchDevelopDataConfig config, IParser<TouchDevelopSchema> parser)
        {
            HttpRequestSettings settings = new HttpRequestSettings()
            {
                RequestedUri = new Uri(string.Format("https://www.touchdevelop.com/api/{0}", config.ScriptId))
            };

            var result = await HttpRequest.DownloadAsync(settings);
            if (result.Success)
            {
                return parser.Parse(result.Result).FirstOrDefault();
            }
            return null;
        }

        private async Task<TouchDevelopSchema> GetLocalProject(TouchDevelopDataConfig config)
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(string.Format("ms-appx://{0}", config.LocalDataSource)));
            IRandomAccessStreamWithContentType randomStream = await file.OpenReadAsync();

            using (StreamReader r = new StreamReader(randomStream.AsStreamForRead()))
            {
                string data = await r.ReadToEndAsync();
                return JsonConvert.DeserializeObject<TouchDevelopSerialized>(data).Items.FirstOrDefault();
            }
        }
    }
}
