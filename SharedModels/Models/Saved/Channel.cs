using LiteDB;
using System;

namespace SharedModels.Models.Saved
{
    public class Channel
    {
        public ObjectId _id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastSeen { get; set; }
    }
}
