using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASACS5.Models.Home
{
    public class HomeIndexViewModel
    {
        public int SiteID { get; set; }
        public string SiteName { get; set; }
        public string Username { get; set; }
        public bool HasFoodBank { get; set; }
    }
}