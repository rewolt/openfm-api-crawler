using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFM_API_Crawler.Models.APISongs
{
    class Song
    {
        public Album Album { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
    }
}
