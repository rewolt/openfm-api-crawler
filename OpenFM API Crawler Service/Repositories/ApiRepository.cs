using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OpenFM_API_Crawler_Service.Repositories
{
    class ApiRepository
    {
        private readonly string _apiUrl = "https://open.fm/api/";
        private readonly string _apiChannelsList = "static/stations/stations_new.json";
        private readonly string _apiChannelsData = "api-ext/v2/channels/long.json";
        private readonly HttpClient _client;

        public ApiRepository()
        {
            _client = new HttpClient();
            InitializeHttpClient();
        }

        private void InitializeHttpClient()
        {
            _client.BaseAddress = new Uri(_apiUrl);

            _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
            _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppleWebKit", "537.36"));
            _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Chrome", "64.0.3282.119"));
            _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Safari", "537.36"));

            _client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            _client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            _client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("pl-PL"));
        }

        public async Task<string> GetText(string apiMethod)
        {
            var response = await _client.GetAsync(apiMethod);
            response.EnsureSuccessStatusCode();

            var decompressedData = DecompressStream(await response.Content.ReadAsStreamAsync());
            return Encoding.UTF8.GetString(decompressedData);
        }

        public async Task<SharedModels.Models.ApiChannels.Root> GetChannelsList()
        {
            var json = await GetText(_apiChannelsList);
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<SharedModels.Models.ApiChannels.Root>(json);
            return data;
        }

        public async Task<SharedModels.Models.ApiSongs.Root> GetChannelsData()
        {
            var json = await GetText(_apiChannelsData);
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<SharedModels.Models.ApiSongs.Root>(json);
            return data;
        }

        private byte[] DecompressStream(Stream compressedStream)
        {
            using var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress);
            using var resultStream = new MemoryStream();
            zipStream.CopyTo(resultStream);
            return resultStream.ToArray();
        }
    }
}
