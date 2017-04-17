using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASACS5.Models.Items;

namespace ASACS5.Models.Reports
{
    public class RoomsBunksAvailabilityViewModel
    {
        public List<RoomsReportRow> reportRows { get; set; }
	}

    public class RoomsReportRow
    {
        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PrimaryContactNumber { get; set; }

        public int MaleBunksAvailable { get; set; }

        public int FemaleBunksAvailable { get; set; }

        public int MixedBunksAvailable { get; set; }

        public int RoomsAvailable { get; set; }

        public string HoursOfOperation { get; set; }

        public string ConditionsForUse { get; set; }
    }
}
	
