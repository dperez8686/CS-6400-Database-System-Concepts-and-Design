using ASACS5.Models.Clients;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASACS5.Models.Site
{
    public class SearchClientViewModel
    {
        public int SiteID { get; set; }
        public string SiteName { get; set; }

        public string StatusType { get; set; }
        public string StatusMessage { get; set; }

        public Boolean DisplayQueryResults { get; set; }
        public int selectedClient { get; set; } 

        [DisplayName("Client ID")]
        public string ClientID { get; set; }

        [DisplayName("Descriptive ID")]
        public string DescriptiveID { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        public string MiddleName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        public List<Client> Clients { get; set; }


    }
}