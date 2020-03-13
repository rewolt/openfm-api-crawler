using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace OpenFM_API_Crawler_Service.Repositories
{
    public class LocalTextRepositry : ILocalRepository
    {
        private readonly string _appPath = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
        private const string _dbFolder = "Database";
        private const string _databseName = "songs.db";
        private readonly string _dbFullPath;
        private readonly ILogger _logger;

        public IEnumerable<SharedModels.Models.Saved.Channel> GetChannels()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SharedModels.Models.Saved.Song> GetSongs()
        {
            throw new NotImplementedException();
        }

        public void UpsertChannel(SharedModels.Models.DTO.Channel channel)
        {
            throw new NotImplementedException();
        }

        public void UpsertSong(SharedModels.Models.DTO.Song song)
        {
            throw new NotImplementedException();
        }
    }
}
