using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace OpenFM_API_Crawler_Service.Factories
{
    public class HttpClientFactory
    {
        private static HttpClient _client;
        public HttpClientFactory()
        {
            _client = new HttpClient();
            InitializeHttpClient();
        }

        public HttpClient GetHttpClient() => _client;

        private void InitializeHttpClient()
        {
            _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Mozilla", "5.0"));
            _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppleWebKit", "537.36"));
            _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Chrome", "64.0.3282.119"));
            _client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Safari", "537.36"));

            _client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            _client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            _client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("pl-PL"));
        }
    }
}
