using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASACS5.Models.Items
{
    public class Item
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int NumberOfUnits { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Category1 { get; set; }
        public string Category2 { get; set; }
        public string StorageType { get; set; }
        public int SiteID { get; set; }
    }
}