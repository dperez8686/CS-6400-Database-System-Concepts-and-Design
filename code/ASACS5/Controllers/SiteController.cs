using ASACS5.Models.Site;
using ASACS5.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Diagnostics;

namespace ASACS5.Controllers
{
    public class SiteController : Controller
    {
        // GET: Site
        public ActionResult Index()
        {
            // Get the logged in Site ID from the session
            // Run SQL to find the data and populate the view model:

            SiteIndexViewModel vm = new SiteIndexViewModel();

            vm.SiteID = 1;
            vm.HasFoodBank = true;
            vm.HasSoupKitchen = true;

            return View(vm);
        }

        public ActionResult SoupKitchen()
        {
            // Get the logged in Site ID from the session
            int SiteID = 2; // hardcoded for now

            // set up the response object
            SoupKitchenViewModel vm = new SoupKitchenViewModel();

            // set up the sql query
            string sql = String.Format(
                "SELECT TotalSeatsAvailable, RemainingSeatsAvailable, HoursOfOperaion, ConditionsForUse " +
                "FROM soupkitchen WHERE SiteID = {0}; ", SiteID.ToString());

            // run the sql against the db
            object[] result = SqlHelper.ExecuteSingleSelect(sql, 4);

            // if we got a result, populate the view model fields
            if (result != null)
            {
                vm.SiteID = SiteID;
                vm.TotalSeatsAvailable = int.Parse(result[0].ToString());
                vm.RemainingSeatsAvailable = int.Parse(result[1].ToString());
                vm.HoursOfOperation = result[2].ToString();
                vm.ConditionsForUse = result[3].ToString();
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SoupKitchen(SoupKitchenViewModel vm)
        {
            if (ModelState.IsValid)
            {
                // find out if the soup kitchen exists for this SiteID already
                // and set up the SQL to INSERT or UPDATE accordingly

                if (vm.SiteID.Equals(0))
                {
                    // we didn't find an existing soup kitchen. so insert a new one based on the logged in users Site ID
                    int SiteID = 2; // make sure to find it from the session

                    string sql = String.Format(
                        "INSERT INTO soupkitchen (SiteID, TotalSeatsAvailable, RemainingSeatsAvailable, HoursOfOperaion, ConditionsForUse) " +
                        "VALUES ({0}, {1}, {2}, '{3}', '{4}'); ",
                        SiteID.ToString(), vm.TotalSeatsAvailable, vm.RemainingSeatsAvailable, vm.HoursOfOperation, vm.ConditionsForUse
                    );

                    SqlHelper.ExecuteNonQuery(sql);

                    vm.SiteID = SiteID; // set the ID since it now exists
                    vm.StatusMessage = "Succesfully added!";
                }
                else
                {
                    // update the existing record 

                    string sql = String.Format(
                        "UPDATE soupkitchen " +
                        "SET TotalSeatsAvailable = {0}, " +
                        "RemainingSeatsAvailable = {1}, " +
                        "HoursOfOperaion = '{2}', " +
                        "ConditionsForUse = '{3}' " +
                        "WHERE SiteID = {4}; ",
                        vm.TotalSeatsAvailable, vm.RemainingSeatsAvailable, vm.HoursOfOperation, vm.ConditionsForUse, vm.SiteID
                    );

                    SqlHelper.ExecuteNonQuery(sql);

                    vm.StatusMessage = "Succesfully updated!";
                }

            }
            return View(vm);
        }

    }
}