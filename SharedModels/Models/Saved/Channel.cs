using System;

namespace SharedModels.Models.Saved
{
    [Serializable]
    public class Channel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastSeen { get; set; }
    }
}
