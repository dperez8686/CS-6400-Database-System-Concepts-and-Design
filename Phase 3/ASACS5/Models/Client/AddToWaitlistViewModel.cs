using ASACS5.Models.Logs;
using ASACS5.Models.Waitlists;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASACS5.Models.Site
{
    public class AddToWaitlistViewModel
    {
        public int SiteID { get; set; }
        public string SiteName { get; set; }

        public string StatusType { get; set; }
        public string StatusMessage { get; set; }

        public int ClientID { get; set; }
        public List<Waitlist> Waitlist { get; set; }
    }
}