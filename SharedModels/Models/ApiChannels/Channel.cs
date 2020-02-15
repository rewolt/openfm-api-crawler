using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels.Models.ApiChannels
{
    public class Channel
    {
        public string Group_id { get; set; }
        public string Instance_id { get; set; }
        public Logo Logo { get; set; }
        public string Mnt { get; set; }
        public string Name { get; set; }
        public List<int> Group_ids { get; set; }
        public int Id { get; set; }
        public Params Params { get; set; }
    }
}
