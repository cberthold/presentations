using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web1.Code
{
    public class ZipcodeEntry
    {
        public string zip { get; set; }
        public string type { get; set; }
        public string primary_city { get; set; }
        public string acceptable_cities { get; set; }
        public string unacceptable_cities { get; set; }
        public string state { get; set; }
        public string county { get; set; }
        public string timezone { get; set; }
        public string area_codes { get; set; }
        public decimal latitude { get; set; }
        public decimal longitude { get; set; }
        public string world_region { get; set; }
        public string country { get; set; }
        public string decommissioned { get; set; }
        public string estimated_population { get; set; }
        public string notes { get; set; }

    }
}