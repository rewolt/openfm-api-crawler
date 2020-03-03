using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SharedModels.Models.Saved;

namespace OpenFM_Youtube_Downloader.Repositories.Interfaces
{
    interface ISongsRepository
    {
        IEnumerable<Channel> GetAllChannelsWithSongs();
        IEnumerable<Channel> GetAllSongsByChannelFromLastCheck(DateTime lastCheck);
        IEnumerable<Song> GetAllSongsFromLastCheck(string channelName, DateTime lastCheck);
    }
}
