using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASACS5.Models.Logs
{
    public class Log
    {
        public int ClientID { get; set; }
        public int LogID { get; set; }
        public string DateTimeStamp { get; set; }
        public string ServiceName { get; set; }
        public string SiteName { get; set; }
        public string Description { get; set; }
    }
}
