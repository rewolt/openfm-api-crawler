using OpenFM_API_Crawler.Repositories;
using SharedModels.Models.Saved;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace OpenFM_API_Crawler.Services
{
    class MainService
    {
        private readonly ApiRepository _apiRepository;
        private readonly FileRepository _fileRepository;
        private readonly DateTime _date = DateTime.UtcNow;
        public MainService()
        {
            _apiRepository = new ApiRepository();
            _fileRepository = new FileRepository();
        }
        public async Task Execute()
        {
            var openChannels = await _apiRepository.GetChannelsList();
            var openChannelsData = await _apiRepository.GetChannelsData();
            var localChannelsData = _fileRepository.Read();

            AddNewChannels(openChannels, localChannelsData);
            AddNewSongs(openChannelsData, localChannelsData);
            _fileRepository.Save(localChannelsData);
        }

        private void AddNewSongs(SharedModels.Models.ApiSongs.Root openChannelsData, List<Channel> localChannelsData)
        {
            foreach (var localChannel in localChannelsData)
            {
                var possibleNewSongs = openChannelsData.Channels
                .SingleOrDefault(x => x.Id == localChannel.Id)
                ?.Tracks.Select(x => new Song
                {
                    Album = x.Song.Album?.Title ?? "",
                    Artist = x.Song.Artist,
                    Name = x.Song.Title,
                    CreatedAt = _date
                })
                .ToList();

                if (possibleNewSongs == null || possibleNewSongs.Count == 0)
                    continue;

                var newSongs = possibleNewSongs.Except(localChannel.Songs, new SongsComparer());
                localChannel.Songs.AddRange(newSongs);
            }
        }

        private void AddNewChannels(SharedModels.Models.ApiChannels.Root openChannels, List<Channel> channels)
        {
            var missingChannelsIds =
                openChannels.Channels
                .Select(x => x.Id)
                .Except(channels.Select(x => x.Id));

            channels.AddRange(
                openChannels.Channels.Where(x => missingChannelsIds.Contains(x.Id))
                        .Select(x => new Channel { Id = x.Id, Name = x.Name }));
        }

        private class SongsComparer : IEqualityComparer<Song>
        {
            public bool Equals([AllowNull] Song x, [AllowNull] Song y)
            {
                return x.Album == y.Album && x.Artist == y.Artist && x.Name == y.Name;
            }

            public int GetHashCode([DisallowNull] Song obj)
            {
                return obj.Name.GetHashCode() ^ obj.Album.GetHashCode() ^ obj.Artist.GetHashCode();
            }
        }
    }
}
