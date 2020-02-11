using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenFM_API_Crawler.Services;
using System.Threading.Tasks;
using System.Text.Json;

namespace OpenFM_API_Crawler
{
    class Program
    {

        private static string _fileName => "openfm_channels.json";
        private static string _saveDirectory;

        //// Browser header to API
        //GET /api/api-ext/v2/channels/long.json? _ = 1517168611.79 HTTP/1.1
        // Host: open.fm
        // Connection: keep-alive
        // Accept: */*
        // X-Requested-With: XMLHttpRequest
        // User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/64.0.3282.119 Safari/537.36
        // Referer: http://open.fm/play/82
        // Accept-Encoding: gzip, deflate
        // Accept-Language: pl-PL,pl;q=0.9,en-US;q=0.8,en;q=0.7
        // Cookie: PWA_adbd=1; pvid=f29de175c69358e6ce40

        static async Task Main(string[] args)
        {
            _saveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

           
            var downloader = new OpenFmRepository();
            
            var openChannelsList = await downloader.GetChannelsList();
            var openChannelsData = await downloader.GetChannelsData();

            if(openChannelsList == null || openChannelsData == null)
            {
                Console.WriteLine("Server returned null json");
                Console.ReadKey();
                return;
            }
            

           
            var localChannels = ReadFromLocal();

            var joinedChannels =
                (
                    from openChannel in openChannelsList.Channels
                    join localChannel in localChannels on openChannel.Id equals localChannel.Id
                    select openChannel
                ).ToArray();
            var missingChannels = openChannelsList.Channels.Except(joinedChannels);

            localChannels.AddRange(missingChannels.Select(x => new SharedModels.Models.SavedObjects.Channel { Id = x.Id, Name = x.Name }));

            for (int i = 0; i < localChannels.Count; i++)
            {
                if (!openChannelsData.Channels.Any(x => x.Id == localChannels[i].Id))
                    continue;

                localChannels[i].Name = openChannelsList.Channels.Where(x => x.Id == localChannels[i].Id).First().Name;

                var openTracks = openChannelsData.Channels.Where(x => x.Id == localChannels[i].Id).Single().Tracks.ToArray();
                var joinedTracks =
                    (
                        from openSong in openTracks
                        join localSong in localChannels[i].Songs on
                        new
                        {
                            Artist = openSong.Song.Artist ?? "",
                            Album = openSong.Song.Album == null ? "" : openSong.Song.Album.Title,
                            Name = openSong.Song.Title ?? ""
                        }
                        equals
                        new
                        {
                            Artist = localSong.Artist,
                            Album = localSong.Album,
                            Name = localSong.Name
                        }
                        select openSong
                    ).ToArray();
                var missingSongs = openTracks.Except(joinedTracks);

                localChannels[i].Songs
                    .AddRange(missingSongs.Select(x =>
                    new SharedModels.Models.SavedObjects.Song
                    {
                        Album = x.Song.Album == null ? "" : x.Song.Album.Title,
                        Artist = x.Song.Artist ?? "",
                        Name = x.Song.Title ?? ""
                    }));
            }


            SaveToLocal(localChannels);
            Console.WriteLine("Done");
        }

        public static void SaveToLocal(List<SharedModels.Models.SavedObjects.Channel> channels)
        {
            var fullPath = _saveDirectory + "/" + _fileName;
            File.WriteAllText(fullPath, JsonSerializer.Serialize(channels));
        }


        public static List<SharedModels.Models.SavedObjects.Channel> ReadFromLocal()
        {
            var fullPath = _saveDirectory + "/" + _fileName;
            var list = new List<SharedModels.Models.SavedObjects.Channel>();

            if (!File.Exists(fullPath))
                return list;

            list = JsonSerializer.Deserialize<List<SharedModels.Models.SavedObjects.Channel>>(File.ReadAllText(fullPath));
            return list;
        }
    }
}
