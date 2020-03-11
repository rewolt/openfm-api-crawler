using LiteDB;
using System;
using System.Collections.Generic;

namespace SharedModels.Models.Saved
{
    [Serializable]
    public class Song
    {
        public ObjectId _id { get; set; }
        public List<int> OpenfmChannelIds { get; set; }
        public string Name { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastSeen { get; set; }
    }
}
