using System.Collections.Generic;

namespace OpenFM_API_Crawler_Service.Repositories
{
    public interface ILocalRepository
    {
        IEnumerable<SharedModels.Models.Saved.Channel> GetChannels();
        IEnumerable<SharedModels.Models.Saved.Song> GetSongs();
        void UpsertChannel(SharedModels.Models.DTO.Channel channel);
        void UpsertSong(SharedModels.Models.DTO.Song song);
    }
}