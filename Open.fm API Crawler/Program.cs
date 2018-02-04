using OpenFM_API_Crawler.Models;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

namespace OpenFM_API_Crawler
{
    class Program
    {
        private static string _apiUrl => "http://open.fm/api/api-ext/v2";
        private static string _apiMethod => "channels/long.json";

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

        static void Main(string[] args)
        {
            if (!Uri.IsWellFormedUriString(_apiUrl, UriKind.Absolute))
            {
                Console.WriteLine("Wrong URI");
                Console.ReadKey();
                return;
            }
                
            var uri = new Uri(_apiUrl);
            var downloader = new JsonDownloader(uri);
            downloader.ApiMethod = _apiMethod;
            var jsonResponse = "";
            jsonResponse = downloader.GetJson();

            var openChannels = JsonConvert.DeserializeObject<Models.API.ApiResponse>(jsonResponse);

            if(openChannels == null)
            {
                Console.WriteLine("Server returned null json");
                Console.ReadKey();
                return;
            }





            IFormatter formatter = new BinaryFormatter();
            var localChannels = DeserializeItem(formatter);

            var joinedChannels =
                (
                    from openChannel in openChannels.Channels
                    join localChannel in localChannels on openChannel.Id equals localChannel.Id
                    select openChannel
                ).ToArray();
            var missingChannels = openChannels.Channels.Except(joinedChannels);

            localChannels.AddRange(missingChannels.Select(x => new Models.SavedObjects.Channel { Id = x.Id }));

            foreach (var localChannel in localChannels)
            {
                var openTracks = openChannels.Channels.Where(x => x.Id == localChannel.Id).Single().Tracks.ToArray();
                var joinedTracks =
                    (
                        from openSong in openTracks
                        join localSong in localChannel.Songs on 
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

                localChannel.Songs
                    .AddRange(missingSongs.Select(x => 
                    new Models.SavedObjects.Song
                    {
                        Album = x.Song.Album == null ? "" : x.Song.Album.Title,
                        Artist = x.Song.Artist ?? "",
                        Name = x.Song.Title ?? ""
                    }));
            }


            SerializeItem(localChannels, formatter);
            Console.WriteLine("Done");
        }

        public static void SerializeItem(List<Models.SavedObjects.Channel> channels, IFormatter formatter)
        {
            using (FileStream s = new FileStream("test.dat", FileMode.Create))
            {
                formatter.Serialize(s, channels);
                s.Close();
            }
        }


        public static List<Models.SavedObjects.Channel> DeserializeItem(IFormatter formatter)
        {
            if (!File.Exists("test.dat"))
                return new List<Models.SavedObjects.Channel>();

            using (FileStream s = new FileStream("test.dat", FileMode.Open))
            {
                List<Models.SavedObjects.Channel> t = (List<Models.SavedObjects.Channel>)formatter.Deserialize(s);
                return t;
            }
        }
    }
}
