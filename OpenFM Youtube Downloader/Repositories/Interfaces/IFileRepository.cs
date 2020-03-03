using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenFM_Youtube_Downloader.Repositories.Interfaces
{
    interface IFileRepository
    {
        void SaveSong(FileInfo fileInfo, MemoryStream stream);
        DateTime GetDateOfLastRun();
        void SaveDateOfLastRun(DateTime date);
    }
}
