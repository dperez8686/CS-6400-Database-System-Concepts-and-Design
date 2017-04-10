using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASACS5.Models.Site
{
    public class SearchClientByNameViewModel
    {
        public int SiteID { get; set; }
        public string SiteName { get; set; }

        public string StatusType { get; set; }
        public string StatusMessage { get; set; }

        public int ClientID { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }


    }
}