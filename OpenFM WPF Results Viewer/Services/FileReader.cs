using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace OpenFM_WPF.Services
{
    class FileReader
    {
        private readonly string _fileName = ConfigurationManager.AppSettings["File"];
        private FileInfo _fileInfo;

        public FileReader()
        {
            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            _fileInfo = new FileInfo(documents + "/" + _fileName);
            _fileInfo.Refresh();

            if (!_fileInfo.Exists)
                throw new Exception("Missing data file");
        }

        public IEnumerable<SharedModels.Models.SavedObjects.Channel> GetData()
        {
            using (var sr = _fileInfo.OpenText())
            {
                return JsonConvert.DeserializeObject<List<SharedModels.Models.SavedObjects.Channel>>(sr.ReadToEnd());
            }
        }
    }
}
