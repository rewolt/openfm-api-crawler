using System;

namespace SharedModels.Models.Saved
{
    [Serializable]
    public class Song
    {
        public string Name { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            return $"{Artist} - {Name} ({Album})";
        }
    }
}
