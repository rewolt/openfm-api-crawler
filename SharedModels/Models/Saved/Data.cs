using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels.Models.Saved
{
    [Serializable]
    public struct Data
    {
        public List<Channel> _channels;
        public List<Song> _songs;
        public Data(List<Channel> channels, List<Song> songs)
        {
            _channels = channels;
            _songs = songs;
        }
    }
}
