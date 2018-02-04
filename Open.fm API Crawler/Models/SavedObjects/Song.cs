using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFM_API_Crawler.Models.SavedObjects
{
    [Serializable]
    class Song
    {
        public string Name { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
    }
}
