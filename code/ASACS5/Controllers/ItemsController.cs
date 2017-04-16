using ASACS5.Models.Items;
using ASACS5.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Diagnostics;
using System.Text;

namespace ASACS5.Controllers
{
    public class ItemsController : Controller
    {
        // GET: Site
        public ActionResult Index()
        {
            // Get the logged in Site ID from the session
            int? SiteID = Session["SiteID"] as int?;

            // if there is none, redirect to the login page
            if (!SiteID.HasValue) return RedirectToAction("Login", "Account");

            ItemsIndexViewModel vm = new ItemsIndexViewModel { SiteID = SiteID.Value };

            // find out if the current Site has a Food Bank or not
            vm.HasFoodBank = Int32.Parse(SqlHelper.ExecuteScalar("SELECT COUNT(*) FROM foodbank WHERE SiteID = " + SiteID.Value).ToString()) > 0;

            // set up the SQL to get all Items
            string sql = String.Format("SELECT ItemID, ItemName, NumberOfUnits, ExpirationDate, StorageType, SiteID, Category1, Category2 FROM item");

            // Initialize the view model Items list
            vm.Items = GetItems(sql);

            // Populate the select lists
            vm.SiteNameFilterOptions = GetFoodBankSelectList(true);
            vm.ExpirationDateFilterOptions = GetDateFilterOperators();
            vm.FoodOrSupplyFilterOptions = GetFoodOrSupplyFilterOptions();
            vm.StorageTypeFilterOptions = GetStorageTypeFilterOptions();
            vm.Category2FilterOptions = GetCategory2FilterOptionsAsSelectList(vm.FoodOrSupplyFitlerValue);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ItemsIndexViewModel vm)
        {
            ModelState.Remove("Items");

            // dynamically build our search query here
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT ItemID, ItemName, NumberOfUnits, ExpirationDate, StorageType, SiteID, Category1, Category2 FROM item ");

            bool firstWhereLineStarted = false; // need to know to use WHERE or AND..

            if (vm.SiteNameFilterEnabled)
            {
                sb.Append(" WHERE SiteID = " + vm.SiteNameFilterValue.ToString() + " ");
                firstWhereLineStarted = true;
            }

            if (vm.ExpirationDateFilterEnabled && !String.IsNullOrWhiteSpace(vm.ExpirationDateFilterValue))
            {
                if (!firstWhereLineStarted) sb.Append(" WHERE "); else sb.Append(" AND ");
                sb.Append(" ExpirationDate " + vm.ExpirationDateFilterOperator + " '" + vm.ExpirationDateFilterValue + "' ");
                firstWhereLineStarted = true;
            }

            if (vm.StorageTypeFilterEnabled && !String.IsNullOrWhiteSpace(vm.StorageTypeFilterValue))
            {
                if (!firstWhereLineStarted) sb.Append(" WHERE "); else sb.Append(" AND ");
                sb.Append(" StorageType = '" + vm.StorageTypeFilterValue + "' ");
                firstWhereLineStarted = true;
            }

            if (vm.FoodOrSupplyFilterEnabled && !String.IsNullOrWhiteSpace(vm.FoodOrSupplyFitlerValue))
            {
                if (!firstWhereLineStarted) sb.Append(" WHERE "); else sb.Append(" AND ");
                sb.Append(" Category1 = '" + vm.FoodOrSupplyFitlerValue + "' ");
                firstWhereLineStarted = true;
            }

            if (vm.Category2FilterEnabled && !String.IsNullOrWhiteSpace(vm.Category2FitlerValue))
            {
                if (!firstWhereLineStarted) sb.Append(" WHERE "); else sb.Append(" AND ");
                sb.Append(" Category2 = '" + vm.Category2FitlerValue + "' ");
                firstWhereLineStarted = true;
            }

            if (vm.ItemNameFilterEnabled && !String.IsNullOrWhiteSpace(vm.ItemNameFilterValue))
            {
                if (!firstWhereLineStarted) sb.Append(" WHERE "); else sb.Append(" AND ");
                sb.Append(" ItemName like '%" + vm.ItemNameFilterValue + "%' ");
                firstWhereLineStarted = true;
            }

            // Initialize the view model Items list
            vm.Items = GetItems(sb.ToString());

            // Re-populate the select lists
            vm.SiteNameFilterOptions = GetFoodBankSelectList(false);
            vm.ExpirationDateFilterOptions = GetDateFilterOperators();
            vm.FoodOrSupplyFilterOptions = GetFoodOrSupplyFilterOptions();
            vm.StorageTypeFilterOptions = GetStorageTypeFilterOptions();
            vm.Category2FilterOptions = GetCategory2FilterOptionsAsSelectList(vm.FoodOrSupplyFitlerValue);

            return View(vm);
        }

        // This method will get the sub-categories for the specified top-level category
        [Route("Items/SubCategories/{category}")]
        public ActionResult SubCategories(string category)
        {
            var items = GetCategory2FilterOptions(category);

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            // Get the logged in Site ID from the session
            int? SiteID = Session["SiteID"] as int?;

            // if there is none, redirect to the login page
            if (!SiteID.HasValue) return RedirectToAction("Login", "Account");

            // make sure current Site has a Food Bank. only these users are allowed / makes sense to add
            if (Int32.Parse(SqlHelper.ExecuteScalar("SELECT COUNT(*) FROM foodbank WHERE SiteID = " + SiteID.Value).ToString()) <= 0)
                throw new Exception("Access is denied: Food Bank sites only can add items");

            AddUpdateItemViewModel vm = new AddUpdateItemViewModel { SiteID = SiteID.Value, Category = "Food", ExpirationDate = new DateTime(9999, 1, 1) };

            vm.FoodOrSupplyFilterOptions = GetFoodOrSupplyFilterOptions();
            vm.StorageTypeFilterOptions = GetStorageTypeFilterOptions();
            vm.Category2FilterOptions = GetCategory2FilterOptions(vm.Category).Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() });

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddUpdateItemViewModel vm)
        {
            // repopulate the dropdown values
            vm.FoodOrSupplyFilterOptions = GetFoodOrSupplyFilterOptions();
            vm.StorageTypeFilterOptions = GetStorageTypeFilterOptions();
            vm.Category2FilterOptions = GetCategory2FilterOptionsAsSelectList(vm.Category);

            if (ModelState.IsValid)
            {
                string sql = String.Format(
                        "INSERT INTO item (ItemName, NumberOfUnits, ExpirationDate, Category1, Category2, StorageType, SiteID) " +
                        "VALUES ('{0}', {1}, '{2}', '{3}', '{4}', '{5}', {6}); ",
                        vm.ItemName, vm.NumberOfUnits.ToString(), vm.ExpirationDate.ToString("yyyy-MM-dd"), vm.Category, vm.SubCategory, vm.StorageType, vm.SiteID.ToString()
                    );

                SqlHelper.ExecuteNonQuery(sql);

                vm.StatusMessage = "Succesfully added!";
            }

            return View(vm);
        }

        [Route("Items/{ItemID:int}")]
        public ActionResult Update(int ItemID)
        {
            // Get the logged in Site ID from the session
            int? SiteID = Session["SiteID"] as int?;

            // if there is none, redirect to the login page
            if (!SiteID.HasValue) return RedirectToAction("Login", "Account");

            // check the existance of and the SiteID of an item with the specified ID
            Item item = ItemService.GetItemById(ItemID);

            // throw an exception if an item with this ID is not found
            if (item == null) throw new Exception("No Item found for Item ID " + ItemID);

            // throw an exception if the SiteID does not match the Item's Site ID
            if (!item.SiteID.Equals(SiteID.Value)) throw new Exception("Update not authorized for Item ID " + ItemID + ", Site ID " + SiteID.Value);

            AddUpdateItemViewModel vm = new AddUpdateItemViewModel {
                SiteID = SiteID.Value,
                ItemID = ItemID,
                ItemName = item.ItemName,
                NumberOfUnits = item.NumberOfUnits,
                Category = item.Category1,
                SubCategory = item.Category2,
                StorageType = item.StorageType,
                ExpirationDate = item.ExpirationDate,
                FoodOrSupplyFilterOptions = GetFoodOrSupplyFilterOptions(),
                StorageTypeFilterOptions = GetStorageTypeFilterOptions(),
                Category2FilterOptions = GetCategory2FilterOptionsAsSelectList(item.Category1)
            };

            return View(vm);
        }

        [HttpPost]
        [Route("Items/{ItemID:int}")]
        [ValidateAntiForgeryToken]
        public ActionResult Update(AddUpdateItemViewModel vm, int ItemID)
        {
            // repopulate the dropdown values
            vm.FoodOrSupplyFilterOptions = GetFoodOrSupplyFilterOptions();
            vm.StorageTypeFilterOptions = GetStorageTypeFilterOptions();
            vm.Category2FilterOptions = GetCategory2FilterOptionsAsSelectList(vm.Category);

            if (ModelState.IsValid)
            {
                string sql = String.Format(
                        "UPDATE item " +
                        "SET ItemName='{0}', NumberOfUnits={1}, ExpirationDate='{2}', Category1='{3}', Category2='{4}', StorageType='{5}' " +
                        "WHERE ItemID = {6} ;",
                        vm.ItemName, vm.NumberOfUnits.ToString(), vm.ExpirationDate.ToString("yyyy-MM-dd"), vm.Category, vm.SubCategory, vm.StorageType, ItemID.ToString()
                    );

                SqlHelper.ExecuteNonQuery(sql);

                vm.StatusMessage = "Succesfully updated!";
            }

            return View(vm);
        }

        private List<Item> GetItems(string sql)
        {
            var response = new List<Item>();

            // run the SQL
            List<object[]> queryResponse = SqlHelper.ExecuteMultiSelect(sql, 8);

            foreach (object[] row in queryResponse)
            {
                // create a new item and add it to the Items List for each row in the query results
                response.Add(new Item
                {
                    ItemID = int.Parse(row[0].ToString()),
                    ItemName = row[1].ToString(),
                    NumberOfUnits = int.Parse(row[2].ToString()),
                    ExpirationDate = DateTime.Parse(row[3].ToString()),
                    StorageType = row[4].ToString(),
                    SiteID = int.Parse(row[5].ToString()),
                    Category1 = row[6].ToString(),
                    Category2 = row[7].ToString()
                });
            }

            return response;
        }

        private SelectList GetFoodBankSelectList(bool force = false)
        {
            // if it's already in the session and we're not forcing, return the session val
            if ((Session["Items.SiteNameSelectList"] as SelectList) != null && !force)
            {
                return Session["Items.SiteNameSelectList"] as SelectList;
            }
            
            // otherwise grab them from the db

            // set up the SQL to get all sites for search options
            string sql = String.Format("SELECT s.SiteID, s.SiteName from site s INNER JOIN foodbank fb on s.SiteID = fb.SiteID ORDER BY s.SiteName asc");

            List<object[]> queryResponse = SqlHelper.ExecuteMultiSelect(sql, 2);

            var selectListItems = new List<SelectListItem>();

            foreach (object[] row in queryResponse)
            {
                // create a new item and add it to the Items List for each row in the query results
                selectListItems.Add(new SelectListItem
                {
                    Value = row[0].ToString(),
                    Text = row[1].ToString() + " (" + row[0].ToString() + ")"

                });
            }

            return new SelectList(selectListItems, "Value", "Text");
        }

        private IEnumerable<SelectListItem> GetDateFilterOperators()
        {
            var selectListItems = new List<SelectListItem>();

            selectListItems.Add(new SelectListItem { Value = "<", Text = "<" });
            selectListItems.Add(new SelectListItem { Value = "=", Text = "=" });
            selectListItems.Add(new SelectListItem { Value = ">", Text = ">" });

            return new SelectList(selectListItems, "Value", "Text");
        }

        private IEnumerable<SelectListItem> GetFoodOrSupplyFilterOptions()
        {
            var selectListItems = new List<SelectListItem>();

            selectListItems.Add(new SelectListItem { Value = "Food", Text = "Food" });
            selectListItems.Add(new SelectListItem { Value = "Supply", Text = "Supply" });

            return new SelectList(selectListItems, "Value", "Text");
        }

        private IEnumerable<SelectListItem> GetStorageTypeFilterOptions()
        {
            var selectListItems = new List<SelectListItem>();

            selectListItems.Add(new SelectListItem { Value = "Dry Good", Text = "Dry Good" });
            selectListItems.Add(new SelectListItem { Value = "Refrigerated", Text = "Refrigerated" });
            selectListItems.Add(new SelectListItem { Value = "Frozen", Text = "Frozen" });

            return new SelectList(selectListItems, "Value", "Text");
        }

        private List<String> GetCategory2FilterOptions(string category)
        {
            var items = new List<string>();

            switch (category)
            {
                case "Food":
                    items.Add("Vegetables");
                    items.Add("Nuts/grains/beans");
                    items.Add("Meat/seafood");
                    items.Add("Dairy/eggs");
                    items.Add("Sauce/Condiment/Seasoning");
                    items.Add("Juice/Drink");
                    break;
                case "Supply":
                    items.Add("Personal hygiene");
                    items.Add("Clothing");
                    items.Add("Shelter");
                    items.Add("Other");
                    break;
            }

            return items;
        }

        private IEnumerable<SelectListItem> GetCategory2FilterOptionsAsSelectList(string category)
        {
            return GetCategory2FilterOptions(category).Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() });
        }
    }
}