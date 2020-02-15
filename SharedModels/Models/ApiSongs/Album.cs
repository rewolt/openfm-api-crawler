using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels.Models.ApiSongs
{
    public class Album
    {
        public int? Year { get; set; }
        public string Cover_uri { get; set; }
        public string Title { get; set; }
    }
}
