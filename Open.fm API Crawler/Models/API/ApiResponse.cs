using System;
using System.Collections.Generic;
using System.Text;

namespace OpenFM_API_Crawler.Models.API
{
    class ApiResponse
    {
        public List<Channel> Channels { get; set; }
        public double Creation_time { get; set; }
        public double Expires { get; set; }
        public double Refresh_point { get; set; }
    }
}
