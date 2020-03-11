using LiteDB;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace OpenFM_API_Crawler_Service.Repositories
{
    public class LocalRepository : IDisposable
    {
        private readonly string _appPath = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
        private const string _dbFolder = "Database";
        private const string _databseName = "songs.db";
        private readonly string _dbFullPath;
        private readonly ILogger _logger;
        private ILiteDatabase _db;

        public LocalRepository(ILogger logger)
        {
            _logger = logger;
            _dbFullPath = Path.Combine(_appPath, _dbFolder, _databseName);
            Directory.CreateDirectory(Path.Combine(_appPath, _dbFolder));
            CreateDatabase();
        }

        public IEnumerable<SharedModels.Models.Saved.Channel> GetChannels()
        {
            var channels =  _db.GetCollection<SharedModels.Models.Saved.Channel>("channels");
            return channels.FindAll();
        }

        public IEnumerable<SharedModels.Models.Saved.Song> GetSongs()
        {
            var songs = _db.GetCollection<SharedModels.Models.Saved.Song>("songs");
            return songs.FindAll();
        }

        public void UpsertChannel(SharedModels.Models.DTO.Channel channel)
        {
            var now = DateTime.UtcNow;

            var channels = _db.GetCollection<SharedModels.Models.Saved.Channel>("channels");
            var foundChannel = channels.FindOne(x => x.Name.Equals(channel.Name, StringComparison.Ordinal));
            if(foundChannel is null)
            {
                var newChannel = new SharedModels.Models.Saved.Channel
                {
                    LastSeen = now,
                    CreatedAt = now,
                    Name = channel.Name
                };
                
                var channelId = channels.Insert(newChannel);
                _logger.Information($"New channel \"{channel.Name}\" added with id: {channelId.ToString()}.");
            }
            else
            {
                foundChannel.LastSeen = now;
                channels.Update(foundChannel);
                //_logger.Information($"Channel \"{foundChannel.Name}\" already exists.");
            }
            _db.Commit();
        }

        public void UpsertSong(SharedModels.Models.DTO.Song song)
        {
            var now = DateTime.UtcNow;

            var songs = _db.GetCollection<SharedModels.Models.Saved.Song>("songs");
            var foundSong = songs
                .FindOne(x => x.Name.Equals(song.Name, StringComparison.Ordinal)
                           && x.Album.Equals(song.Album, StringComparison.Ordinal)
                           && x.Artist.Equals(song.Artist, StringComparison.Ordinal));

            if(foundSong is null)
            {
                var newSong = new SharedModels.Models.Saved.Song
                {
                    LastSeen = now,
                    CreatedAt = now,
                    Album = song.Album,
                    Artist = song.Artist,
                    Name = song.Name,
                    OpenfmChannelIds = new List<int> { song.OpenfmChannelId }
                };
                
                var songId = songs.Insert(newSong);
                _logger.Information($"New song \"{song.Artist} - {song.Name}\" added with id: {songId.ToString()}");
            }
            else
            {
                if (!foundSong.OpenfmChannelIds.Contains(song.OpenfmChannelId))
                    foundSong.OpenfmChannelIds.Add(song.OpenfmChannelId);

                foundSong.LastSeen = now;
                songs.Update(foundSong);
                //_logger.Information($"Song \"{song.Artist} - {song.Name}\" already exists.");
            }
        }

        private void CreateDatabase()
        {
            try
            {
                _db = new LiteDatabase(_dbFullPath);
                _logger.Information("Database songs.db opened.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Cannot create or open songs.db.");
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _db.Commit();
                    _db.Dispose();
                }
                disposedValue = true;
            }
        }
        ~LocalRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
