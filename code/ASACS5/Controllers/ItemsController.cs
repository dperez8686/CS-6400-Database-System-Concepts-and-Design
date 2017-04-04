using ASACS5.Models.Items;
using ASACS5.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Diagnostics;

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
            vm.Items = new List<Item>();

            foreach (object[] row in queryResponse)
            {
                // create a new item and add it to the Items List for each row in the query results
                vm.Items.Add(new Item
                {
                    ItemID = int.Parse(row[0].ToString()),
                    ItemName = row[1].ToString(),
                    NumberOfUnits = int.Parse(row[2].ToString()),
                    ExpirationDate = DateTime.Parse(row[3].ToString()),
                    StorageType = row[4].ToString(),
                    SiteID = int.Parse(row[5].ToString())
                });
            }

            // set up the SQL to get all sites for search options --------------------------
            sql = String.Format("SELECT SiteID, SiteName from site ORDER BY SiteName asc");

            queryResponse = SqlHelper.ExecuteMultiSelect(sql, 2);

            vm.SiteSearchOptions = new List<SelectListItem>();

            foreach (object[] row in queryResponse)
            {
                // create a new item and add it to the Items List for each row in the query results
                //vm.SiteSearchOptions.Add(new SelectListItem
                //    {
                //        Value = row[0].ToString(),
                //        Text = row[1].ToString() + " (" + row[0].ToString() + ")"

                //});
            }

            return View(vm);
        }
    }
}