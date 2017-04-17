using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASACS5.Models.Items
{
    public class AddUpdateItemViewModel
    {
        public int SiteID { get; set; }
        public int ItemID { get; set; }
        public string StatusMessage { get; set; }

        public IEnumerable<SelectListItem> FoodOrSupplyFilterOptions { get; set; }
        public IEnumerable<SelectListItem> Category2FilterOptions { get; set; }
        public IEnumerable<SelectListItem> StorageTypeFilterOptions { get; set; }

        [DisplayName("Item Name")]
        [Required(ErrorMessage = "Required")]
        [MaxLength(250, ErrorMessage = "Max length is 250 characters")]
        public string ItemName { get; set; }

        [DisplayName("Number of Units")]
        [Required(ErrorMessage = "Required")]
        [Range(0, int.MaxValue, ErrorMessage = "Cannot be a negative number")]
        public int NumberOfUnits { get; set; }

        [DisplayName("Category")]
        [Required(ErrorMessage = "Required")]
        public string Category { get; set; }

        [DisplayName("Sub-Category")]
        [Required(ErrorMessage = "Required")]
        public string SubCategory { get; set; }

        [DisplayName("Storage Type")]
        [Required(ErrorMessage = "Required")]
        public string StorageType { get; set; }

        [DisplayName("Expiration Date")]
        [Required(ErrorMessage = "Required")]
        public DateTime ExpirationDate { get; set; }

    }
}