using Microsoft.Extensions.Configuration;
using SharedModels.Models.DTO;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace OpenFM_API_Crawler.Repositories
{
    class FileRepository
    {
        private readonly string _fileFullPath;
        private readonly IConfiguration _config;
        public FileRepository()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appconfig.json", false, true)
                .Build();

            _fileFullPath = Path.Combine(_config["Database:path"], _config["Database:fileName"]);
        }

        public void Save(List<Channel> channels)
        {
            File.WriteAllText(_fileFullPath, JsonSerializer.Serialize(channels));
        }

        public List<Channel> Read()
        {
            var list = new List<Channel>();

            if (!File.Exists(_fileFullPath))
                return list;

            list = JsonSerializer.Deserialize<List<Channel>>(File.ReadAllText(_fileFullPath));
            return list;
        }
    }
}
