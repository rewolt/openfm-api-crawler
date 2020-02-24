using OpenFM_Youtube_Downloader.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFM_Youtube_Downloader.Services
{
    class MainService
    {
        private readonly ISongsRepository _songsRepository;
        private readonly IFileRepository _fileRepository;
        private readonly DateTime _dateOfLastRun;
        public MainService()
        {
            _fileRepository = new FilesRepository();
            _songsRepository = new SongsRepository();
        }
        public void Execute()
        {
            
        }
    }
}
