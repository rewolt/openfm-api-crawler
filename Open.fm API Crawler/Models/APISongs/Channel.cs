using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFM_API_Crawler.Models.APISongs
{
    class Channel
    {
        public List<Track> Tracks { get; set; }
        public int Id { get; set; }
    }
}
