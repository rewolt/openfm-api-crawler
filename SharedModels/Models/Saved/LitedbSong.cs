using LiteDB;

namespace SharedModels.Models.Saved
{
    public class LitedbSong : Song
    {
        public ObjectId _id { get; set; }
    }
}
