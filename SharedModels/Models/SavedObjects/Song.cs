using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels.Models.SavedObjects
{
    [Serializable]
    public class Song
    {
        public string Name { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
    }
}
