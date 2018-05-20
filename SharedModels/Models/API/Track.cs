using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels.Models.API
{
    public class Track
    {
        public double Begin { get; set; }
        public double End { get; set; }
        public int Id { get; set; }
        public Song Song { get; set; }
    }
}
