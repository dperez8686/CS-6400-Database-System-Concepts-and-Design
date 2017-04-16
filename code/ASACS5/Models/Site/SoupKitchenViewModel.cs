using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASACS5.Models.Site
{
    public class SoupKitchenViewModel
    {
        public int SiteID { get; set; }
        public string SiteName { get; set; }

        public string StatusType { get; set; }
        public string StatusMessage { get; set; }

        [DisplayName("Description")]
        [Required(ErrorMessage = "Description is required")]
        [MaxLength(1000, ErrorMessage = "Max length is 1000 characters")]
        public string Description { get; set; }

        [DisplayName("Hours of Operation")]
        [Required(ErrorMessage = "Hours of Operation is required")]
        [MaxLength(1000, ErrorMessage = "Max length is 1000 characters")]
        public string HoursOfOperation { get; set; }

        [DisplayName("Conditions For Use")]
        [MaxLength(1000, ErrorMessage = "Max length is 1000 characters")]
        public string ConditionsForUse { get; set; }

        [DisplayName("Total Seats Available")]
        [Required(ErrorMessage = "Total Seats Available is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Total Seats Available cannot be a negative number")]
        public int TotalSeatsAvailable { get; set; }

        [DisplayName("Remaining Seats Available")]
        [Required(ErrorMessage = "Remaining Seats Available is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Remaining Seats Available cannot be a negative number")]
        public int RemainingSeatsAvailable { get; set; }
    }
}