using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASACS5.Models.Site
{
    public class DeleteServiceViewModel
    {
        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public string ServiceType { get; set; }
        public bool ServiceExists { get; set; }
        public bool DeleteCompleted { get; set; }
    }
}