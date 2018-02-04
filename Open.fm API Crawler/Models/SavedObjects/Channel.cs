using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace OpenFM_API_Crawler.Models.SavedObjects
{
    [Serializable]
    class Channel : ISerializable
    {
        public int Id { get; set; }
        public List<Song> Songs { get; set; } 

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("channelId", Id, typeof(int));
            info.AddValue("tracks", Songs, typeof(List<Song>));
        }

        public Channel()
        {
            Songs = new List<Song>();
        }

        public Channel(SerializationInfo info, StreamingContext context)
        {
            Id = (int)info.GetValue("channelId", typeof(int));
            Songs = (List<Song>)info.GetValue("tracks", typeof(List<Song>));
        }
    }
}
