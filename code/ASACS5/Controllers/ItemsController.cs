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

            // set up the SQL to get all Items -----------------------------------------------------------
            string sql = String.Format(
                "SELECT ItemID, ItemName, NumberOfUnits, ExpirationDate, StorageType, SiteID FROM item");

            // run the SQL
            List<object[]> queryResponse = SqlHelper.ExecuteMultiSelect(sql, 6);

            // Initialize the view model Items list
            vm.Items = GetItemListFromQueryResponse(queryResponse);

            vm.SiteSearchOptions = GetSiteNameSelectList(true);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ItemsIndexViewModel vm)
        {
            ModelState.Remove("Items");

            // dynamically build our search query here
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT ItemID, ItemName, NumberOfUnits, ExpirationDate, StorageType, SiteID FROM item ");

            bool firstWhereLineStarted = false; // need to know to use WHERE or AND..

            if (vm.SiteNameFilterEnabled)
            {
                sb.Append(" WHERE SiteID = " + vm.SiteNameFilterValue.ToString() + " ");
                firstWhereLineStarted = true;
            }

            if (vm.StorageTypeFilterEnabled && !String.IsNullOrWhiteSpace(vm.StorageTypeFilterValue))
            {
                if (!firstWhereLineStarted) sb.Append(" WHERE "); else sb.Append(" AND ");
                sb.Append(" StorageType like '%" + vm.StorageTypeFilterValue + "%' ");
                firstWhereLineStarted = true;
            }

            // run the SQL
            List<object[]> queryResponse = SqlHelper.ExecuteMultiSelect(sb.ToString(), 6);

            // Initialize the view model Items list
            vm.Items = GetItemListFromQueryResponse(queryResponse);

            // Re-populate the site name select list
            vm.SiteSearchOptions = GetSiteNameSelectList(false);

            return View(vm);
        }

        private List<Item> GetItemListFromQueryResponse(List<object[]> queryResponse)
        {
            var response = new List<Item>();

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
                    SiteID = int.Parse(row[5].ToString())
                });
            }

            return response;
        }

        private SelectList GetSiteNameSelectList(bool force = false)
        {
            // if it's already in the session and we're not forcing, return the session val
            if ((Session["Items.SiteNameSelectList"] as SelectList) != null && !force)
            {
                return Session["Items.SiteNameSelectList"] as SelectList;
            }
            
            // otherwise grab them from the db

            // set up the SQL to get all sites for search options
            string sql = String.Format("SELECT SiteID, SiteName from site ORDER BY SiteName asc");

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
    }
}