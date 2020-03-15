using System;

namespace SharedModels.Models.Saved
{
    [Serializable]
    public class Channel
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastSeen { get; set; }
    }
}
