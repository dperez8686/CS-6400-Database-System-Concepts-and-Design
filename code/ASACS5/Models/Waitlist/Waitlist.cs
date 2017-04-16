using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASACS5.Models.Waitlists
{
    public class Waitlist
    {
        public int ClientID { get; set; }
        public int SiteID { get; set; }
        public int Ranking { get; set; }
        public int oldRanking { get; set; }
    }
}
