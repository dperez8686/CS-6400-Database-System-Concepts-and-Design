using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASACS5.Models.Site
{
    public class SelectClientViewModel
    {
        public int SiteID { get; set; }
        public string SiteName { get; set; }

        public string StatusType { get; set; }
        public string StatusMessage { get; set; }

        public int ClientID { get; set; }

        [DisplayName("Descriptive ID")]
        [Required(ErrorMessage = "Descriptive ID is required")]
        [MaxLength(1000, ErrorMessage = "Max length is 250 characters")]
        public string DescriptiveID { get; set; }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(1000, ErrorMessage = "Max length is 250 characters")]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        [MaxLength(1000, ErrorMessage = "Max length is 250 characters")]
        public string MiddleName { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(1000, ErrorMessage = "Max length is 250 characters")]
        public string LastName { get; set; }

        [DisplayName("Phone Number")]
        [MaxLength(10, ErrorMessage = "Max length is 80 characters")]
        public string PhoneNumber { get; set; }

    }
}