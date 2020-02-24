using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenFM_Youtube_Downloader.Repositories
{
    interface IFileRepository
    {
        void SaveSong(FileInfo fileInfo, MemoryStream stream);
    }
}
