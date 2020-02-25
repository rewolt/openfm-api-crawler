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

        public FileInfo DownloadTrack(Uri songUri)
        {
            var trackId = YoutubeClient.ParseVideoId(songUri.AbsolutePath);
            var streamInfoSet = await _client.GetVideoMediaStreamInfosAsync(trackId);
            var title = video.Title; // "Infected Mushroom - Spitfire [Monstercat Release]"
            var author = video.Author;
            var streamInfo = streamInfoSet.Audio.WithHighestBitrate();

            var ext = streamInfo.Container.GetFileExtension();
        }
    }
}
