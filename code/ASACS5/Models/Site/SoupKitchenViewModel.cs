using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ASACS5.Models.Site
{
    public class SoupKitchenViewModel
    {
        public int SiteID { get; set; }
        public string StatusType { get; set; }
        public string StatusMessage { get; set; }

        [DisplayName("Hours of Operation")]
        public string HoursOfOperation { get; set; }

        [DisplayName("Conditions For Use")]
        public string ConditionsForUse { get; set; }

        [DisplayName("Total Seats Available")]
        public int TotalSeatsAvailable { get; set; }

        [DisplayName("Remaining Seats Available")]
        public int RemainingSeatsAvailable { get; set; }
    }
}