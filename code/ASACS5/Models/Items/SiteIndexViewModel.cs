using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASACS5.Models.Items
{
    public class ItemsIndexViewModel
    {
        public int SiteID { get; set; }
        public List<Item> Items { get; set; }
        public IEnumerable<SelectListItem> SiteSearchOptions { get; set; }

        public int SiteNameFilterValue { get; set; }
        public bool SiteNameFilterEnabled { get; set; }

        public string StorageTypeFilterValue { get; set; }
        public bool StorageTypeFilterEnabled { get; set; }

    }
}