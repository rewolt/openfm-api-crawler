using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace OpenFM_API_Crawler.Repositories
{
    class FileRepository
    {
        private const string _fileName = "openfm_channels.json";
        private readonly string _fileFullPath;
        public FileRepository()
        {
            _fileFullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), _fileName);
        }

        public void Save(List<SharedModels.Models.SavedObjects.Channel> channels)
        {
            File.WriteAllText(_fileFullPath, JsonConvert.SerializeObject(channels));
        }

        public List<SharedModels.Models.SavedObjects.Channel> Read()
        {
            var list = new List<SharedModels.Models.SavedObjects.Channel>();

            if (!File.Exists(_fileFullPath))
                return list;

            list = JsonConvert.DeserializeObject<List<SharedModels.Models.SavedObjects.Channel>>(File.ReadAllText(_fileFullPath));
            return list;
        }
    }
}
