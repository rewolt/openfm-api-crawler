using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using YoutubeExplode;

namespace OpenFM_Youtube_Downloader.Services
{
    class YoutubeParsingService
    {
        private readonly YoutubeClient _client;
        public YoutubeParsingService(HttpClient httpClient)
        {
            _client = new YoutubeClient(httpClient);
        }


    }
}
