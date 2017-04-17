using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASACS5.Models.Requests
{
    public class Request
    {
        public int RequestID { get; set; }
        public string UserName { get; set; }
        public int OwnerSiteID { get; set; }
        public string ItemName { get; set; }
        public int ItemID { get; set; }
        public int RequestedQuantity { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string StorageType { get; set; }
        public DateTime ExpirationDate { get; set; }
        public String Status { get; set; }
        public int FulfilledQuantity { get; set; }
        public bool OverTheLimit { get; set; }
    }
}