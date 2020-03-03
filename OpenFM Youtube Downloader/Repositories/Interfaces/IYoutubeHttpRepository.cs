using System;
using System.Collections.Generic;

namespace OpenFM_Youtube_Downloader.Repositories.Interfaces
{
    public interface IYoutubeHttpRepository
    {
        public IEnumerable<Uri> GetSongsByWordKeys(string wordKeys);
    }
}