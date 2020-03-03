using OpenFM_Youtube_Downloader.Repositories;
using OpenFM_Youtube_Downloader.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenFM_Youtube_Downloader.Services
{
    class MainService
    {
        private readonly ISongsRepository _songsRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IYoutubeHttpRepository _youtubeHttpRepository;
        private readonly DateTime _dateOfLastRun;
        private readonly DateTime _dateOfOngoingRun;
        public MainService()
        {
            _fileRepository = new FilesRepository();
            _songsRepository = new SongsRepository();
            _youtubeHttpRepository = new YoutubeHttpRepository();
            _dateOfOngoingRun = DateTime.UtcNow;

        }
        public void Execute()
        {
            var lastRunDate = _fileRepository.GetDateOfLastRun();
            var newSongsByChannel = _songsRepository.GetAllSongsByChannelFromLastCheck(lastRunDate);

            foreach(var channel in newSongsByChannel)
            {
                foreach (var song in channel.Songs)
                {
                    var searchPattern = $"{song.Name} {song.Artist}";
                    var songsUris = _youtubeHttpRepository.GetSongsByWordKeys(searchPattern);
                    var bestMatch = songsUris.FirstOrDefault();

                    if (bestMatch is null)
                        continue;
                    
                    var cos = _youtubeHttpRepository.

                }
            }
        }
    }
}
