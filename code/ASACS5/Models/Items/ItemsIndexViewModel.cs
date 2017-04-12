using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASACS5.Models.Items
{
    public class ItemsIndexViewModel
    {
        public int SiteID { get; set; }
        public bool HasFoodBank { get; set; }
        public List<Item> Items { get; set; }
        public IEnumerable<SelectListItem> SiteNameFilterOptions { get; set; }
        public IEnumerable<SelectListItem> ExpirationDateFilterOptions { get; set; }
        public IEnumerable<SelectListItem> FoodOrSupplyFilterOptions { get; set; }
        public IEnumerable<SelectListItem> Category2FilterOptions { get; set; }
        public IEnumerable<SelectListItem> StorageTypeFilterOptions { get; set; }

        public bool SiteNameFilterEnabled { get; set; }
        public int SiteNameFilterValue { get; set; }

        public bool StorageTypeFilterEnabled { get; set; }
        public string StorageTypeFilterValue { get; set; }
        
        public bool ExpirationDateFilterEnabled { get; set; }
        public string ExpirationDateFilterOperator { get; set; }
        public string ExpirationDateFilterValue { get; set; }

        public bool FoodOrSupplyFilterEnabled { get; set; }
        public string FoodOrSupplyFitlerValue { get; set; }

        public bool Category2FilterEnabled { get; set; }
        public string Category2FitlerValue { get; set; }

        public bool ItemNameFilterEnabled { get; set; }
        public string ItemNameFilterValue { get; set; }
    }
}