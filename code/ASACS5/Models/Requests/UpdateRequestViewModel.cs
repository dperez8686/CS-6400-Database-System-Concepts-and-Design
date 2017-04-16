using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASACS5.Models.Requests
{
    public class UpdateRequestViewModel
    {
        public string StatusMessage { get; set; }
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }
        public int OwnerSiteID { get; set; }

        [DisplayName("Requesting Site")]
        public string RequestorSiteName { get; set; }

        [DisplayName("Requestor Username")]
        public string RequestorUsername { get; set; }

        [DisplayName("Item name")]
        public string ItemName { get; set; }

        [DisplayName("Quantity Requested")]
        public int QuantityRequested { get; set; }

        [DisplayName("Total Quantity Available")]
        public int QuantityAvailable { get; set; }

        [DisplayName("Quantity to Fulfill")]
        [Required(ErrorMessage = "Required")]
        [Range(0, int.MaxValue, ErrorMessage = "Cannot be a negative number")]
        public int QuantityToFulfill { get; set; }

    }
}