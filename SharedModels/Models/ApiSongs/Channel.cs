﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels.Models.ApiSongs
{
    public class Channel
    {
        public List<Track> Tracks { get; set; }
        public int Id { get; set; }
    }
}
