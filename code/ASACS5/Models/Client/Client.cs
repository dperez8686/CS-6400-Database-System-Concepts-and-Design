using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASACS5.Models.Clients
{
    public class Client
    {
        public int ClientID { get; set; }
        public string DescriptionID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string  PhoneNumber { get; set; }
    }
}
