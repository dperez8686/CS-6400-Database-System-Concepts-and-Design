using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASACS5.Models.Requests
{
    public class AddRequestViewModel
    {
        public string StatusMessage { get; set; }
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }

        public int ItemID { get; set; }    

        [DisplayName("Your site")]
        public string UserSiteName { get; set; }

        [DisplayName("Your name")]
        public string UserName { get; set; }

        [DisplayName("Owner site ID")]
        public int OwnerSiteID { get; set; }

        [DisplayName("Item name")]
        public string ItemName { get; set; }

        [DisplayName("Number of units")]
        [Required(ErrorMessage = "Required")]
        [Range(0, int.MaxValue, ErrorMessage = "Cannot be a negative number")]
        public int NumberOfUnits { get; set; }

        public int MaxNumberOfUnits { get; set; }

        [DisplayName("Expiration Date")]
        public DateTime ExpirationDate { get; set; }

    }
}