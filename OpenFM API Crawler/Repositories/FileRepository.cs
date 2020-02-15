using Microsoft.Extensions.Configuration;
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

        public void Save(List<SharedModels.Models.Saved.Channel> channels)
        {
            File.WriteAllText(_fileFullPath, JsonSerializer.Serialize(channels));
        }

        public List<SharedModels.Models.Saved.Channel> Read()
        {
            var list = new List<SharedModels.Models.Saved.Channel>();

            if (!File.Exists(_fileFullPath))
                return list;

            list = JsonSerializer.Deserialize<List<SharedModels.Models.Saved.Channel>>(File.ReadAllText(_fileFullPath));
            return list;
        }
    }
}
