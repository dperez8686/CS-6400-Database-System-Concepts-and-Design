using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASACS5.Models.Site
{
    public class SiteIndexViewModel
    {
        public int SiteID { get; set; }
        public bool HasFoodBank { get; set; }
        public bool HasShelter { get; set; }
        public bool HasSoupKitchen { get; set; }
        public bool HasFoodPantry { get; set; }
    }
}