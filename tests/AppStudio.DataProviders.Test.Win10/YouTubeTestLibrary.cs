
using System;
using System.Threading.Tasks;
using Windows.Web.Http;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AppStudio.DataProviders.Test
{
    [TestClass]
    public partial class YouTubeTestLibrary
    {
        [TestMethod]
        public async Task TestHttp20()
        {
            var query = new Uri("https://www.googleapis.com/youtube/v3/channels?forUsername=wizardsmtg&part=contentDetails&maxResults=1&key=AIzaSyDdOl3JfYah7b74Bz6BN9HzsnewSqVTItQ");

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            HttpResponseMessage response = await httpClient.GetAsync(query);
            string result = await response.Content.ReadAsStringAsync();
            Assert.IsNotNull(result);
        }
    }
}
