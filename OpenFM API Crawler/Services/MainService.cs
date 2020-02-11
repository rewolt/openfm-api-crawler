using OpenFM_API_Crawler.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenFM_API_Crawler.Services
{
    class MainService
    {
        private readonly ApiRepository _apiRepository;
        private readonly FileRepository _fileRepository;
        public MainService()
        {
            _apiRepository = new ApiRepository();
            _fileRepository = new FileRepository();
        }
        public async Task Execute()
        {

            var openChannelsList = await _apiRepository.GetChannelsList();
            var openChannelsData = await _apiRepository.GetChannelsData();

            if (openChannelsList == null || openChannelsData == null)
            {
                Console.WriteLine("Server returned null json");
                Console.ReadKey();
                return;
            }



            var localChannels = _fileRepository.Read();

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


            _fileRepository.Save(localChannels);
            Console.WriteLine("Done");
        }
    }
}
