﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels.Models.ApiSongs
{
    public class Song
    {
        public Album Album { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
    }
}
