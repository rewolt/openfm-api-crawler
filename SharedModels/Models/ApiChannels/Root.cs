using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels.Models.ApiChannels
{
    public class Root
    {
        public List<Channel> Channels { get; set; }
        public List<Group> Groups { get; set; }
        public int Max_instance_id { get; set; }
        public List<Server> Servers { get; set; }
        public int Refresh { get; set; }
        public int Date { get; set; }
    }
}
