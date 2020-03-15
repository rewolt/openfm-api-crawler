using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SharedModels.Models.Saved;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace OpenFM_API_Crawler_Service.Repositories
{
    public class LocalBinaryRepository : ILocalRepository, IDisposable
    {
        private readonly string _appPath = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
        private const string _dbFolder = "Database";
        private const string _databseName = "songs.bin";
        private readonly string _dbFullPath;
        private readonly ILogger _logger;
        private Task _doSavingDataPeriodicaly;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private Data _data;

        [Serializable]
        private struct Data
        {
            public List<Channel> _channels;
            public List<Song> _songs;
            public Data(List<Channel> channels, List<Song> songs)
            {
                _channels = channels;
                _songs = songs;
            }
        }

        public LocalBinaryRepository(ILogger logger)
        {
            _logger = logger;
            _dbFullPath = Path.Combine(_appPath, _dbFolder, _databseName);
            _data = new Data { _channels = new List<Channel>(), _songs = new List<Song>() };
            _cancellationTokenSource = new CancellationTokenSource();
            ReadData();
            _doSavingDataPeriodicaly = SaveDataPeriodicaly(_cancellationTokenSource.Token);
        }

        public IEnumerable<SharedModels.Models.Saved.Channel> GetChannels()
        {
            return _data._channels;
        }

        public IEnumerable<SharedModels.Models.Saved.Song> GetSongs()
        {
            return _data._songs;
        }

        public void UpsertChannel(SharedModels.Models.DTO.Channel channel, DateTime lastSeen)
        {
            var existsingChannel = _data._channels.FirstOrDefault(x => x.Name == channel.Name);
            if (existsingChannel is null)
            {
                existsingChannel = new Channel { Name = channel.Name, CreatedAt = lastSeen, LastSeen = lastSeen };
                _data._channels.Add(existsingChannel);
            }
            else
            {
                existsingChannel.LastSeen = lastSeen;
            }
        }

        public void UpsertSong(SharedModels.Models.DTO.Song song, DateTime lastSeen)
        {
            var existingSong = _data._songs
                .FirstOrDefault(x => x.Name == song.Name
                                  && x.Album == song.Album
                                  && x.Artist == song.Artist);
            if(existingSong is null)
            {
                existingSong = new Song
                {
                    Album = song.Album,
                    Artist = song.Artist,
                    Name = song.Name,
                    CreatedAt = lastSeen,
                    LastSeen = lastSeen,
                    OpenfmChannelIds = new List<int> { song.OpenfmChannelId }
                };
                _data._songs.Add(existingSong);
            }
            else
            {
                existingSong.LastSeen = lastSeen;
                if (!existingSong.OpenfmChannelIds.Contains(song.OpenfmChannelId))
                    existingSong.OpenfmChannelIds.Add(song.OpenfmChannelId);
            }
        }

        private void ReadData()
        {
            _logger.Information($"{nameof(LocalBinaryRepository)} - Reading data from {_dbFullPath}...");
            try
            {
                if (!File.Exists(_dbFullPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_dbFullPath));
                    File.WriteAllText(_dbFullPath, string.Empty);
                }

                using var fileStream = File.OpenRead(_dbFullPath);
                if (fileStream.Length == 0)
                {
                    _logger.Information($"{nameof(LocalBinaryRepository)} - Data is empty. New collection initialized.");
                    return;
                }
                    

                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                var data = formatter.Deserialize(fileStream);
                if (data != null)
                    _data = (Data)data;

                _logger.Information($"{nameof(LocalBinaryRepository)} - Data read and loaded.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString(), "Cannot read data from saved file.");
                throw;
            }
        }

        public void SaveData()
        {
            _logger.Information($"{nameof(LocalBinaryRepository)} - Saving data into {_dbFullPath}...");
            try
            {
                if (!File.Exists(_dbFullPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_dbFullPath));
                }

                using var fileStream = File.OpenWrite(_dbFullPath);
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(fileStream, _data);
                _logger.Information($"{nameof(LocalBinaryRepository)} - Data saved.");
            }
            catch(Exception ex)
            {
                _logger.Error(ex.ToString(), "Cannot save data to file.");
                throw;
            }
        }

        private Task SaveDataPeriodicaly(CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                while(!cancellationToken.IsCancellationRequested)
                {
                    SaveData();
                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                }
            },cancellationToken);
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            SaveData();
            _cancellationTokenSource.Dispose();
        }
    }
}
