﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SharedModels.Models.SavedObjects
{
    public class Channel
    {
        public int Id { get; set; }
        public string Name { get; set;}
        public List<Song> Songs { get; set; } 

        public Channel()
        {
            Songs = new List<Song>();
        }

        public override string ToString()
        {
            return $"Name ({Songs.Count})";
        }
    }
}