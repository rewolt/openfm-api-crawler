using OpenFM_Youtube_Downloader.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenFM_Youtube_Downloader.Repositories
{
    class FilesRepository : IFileRepository
    {
        public FilesRepository()
        {
        }

        public DateTime GetDateOfLastRun()
        {
            throw new NotImplementedException();
        }

        public void SaveDateOfLastRun(DateTime date)
        {
            throw new NotImplementedException();
        }

        public void SaveSong(FileInfo fileInfo, MemoryStream stream)
        {
            throw new NotImplementedException();
        }
    }
}
