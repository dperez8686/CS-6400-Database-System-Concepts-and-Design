using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASACS5.Models.Site
{
    public class ShelterViewModel
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

        [DisplayName("Male Bunks Available")]
        [Required(ErrorMessage = "Male Bunks Available is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Male Bunks Available cannot be a negative number")]
        public int MaleBunksAvailable { get; set; }

        [DisplayName("Female Bunks Available")]
        [Required(ErrorMessage = "Female Bunks Available is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Female Bunks Available cannot be a negative number")]
        public int FemaleBunksAvailable { get; set; }

        [DisplayName("Mixed Bunks Available")]
        [Required(ErrorMessage = "Mixed Bunks Available is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Mixed Available cannot be a negative number")]
        public int MixedBunksAvailable { get; set; }

        [DisplayName("Rooms Available")]
        [Required(ErrorMessage = "Rooms Available is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Rooms Available cannot be a negative number")]
        public int RoomsAvailable { get; set; }
    }
}