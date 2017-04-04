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
            int? SiteID = Session["SiteID"] as int?;

            // if there is none, redirect to the login page
            if (!SiteID.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            // Run SQL to find the data and populate the view model:

            SiteIndexViewModel vm = new SiteIndexViewModel();

            vm.SiteID = SiteID.Value;
            vm.HasFoodBank = true;
            vm.HasSoupKitchen = true;
            vm.HasShelter = true;
            return View(vm);
        }

        public ActionResult SoupKitchen()
        {
            // Get the logged in Site ID from the session
            int? SiteID = Session["SiteID"] as int?;

            // if there is none, redirect to the login page
            if (!SiteID.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            // set up the response object
            SoupKitchenViewModel vm = new SoupKitchenViewModel();

            // set up the sql query
            string sql = String.Format(
                "SELECT TotalSeatsAvailable, RemainingSeatsAvailable, HoursOfOperaion, ConditionsForUse " +
                "FROM soupkitchen WHERE SiteID = {0}; ", SiteID.Value.ToString());

            // run the sql against the db
            object[] result = SqlHelper.ExecuteSingleSelect(sql, 4);

            // if we got a result, populate the view model fields
            if (result != null)
            {
                vm.SiteID = SiteID.Value;
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
                // this is needed for some reason.. come back to it
                // http://stackoverflow.com/questions/4837744/hiddenfor-not-getting-correct-value-from-view-model
                ModelState.Remove("SiteID");

                // find out if the soup kitchen exists for this SiteID already
                // and set up the SQL to INSERT or UPDATE accordingly

                if (vm.SiteID.Equals(0))
                {
                    // we didn't find an existing soup kitchen. so insert a new one based on the logged in users Site ID
                    int? SiteID = Session["SiteID"] as int?;

                    string sql = String.Format(
                        "INSERT INTO soupkitchen (SiteID, TotalSeatsAvailable, RemainingSeatsAvailable, HoursOfOperaion, ConditionsForUse) " +
                        "VALUES ({0}, {1}, {2}, '{3}', '{4}'); ",
                        SiteID.Value.ToString(), vm.TotalSeatsAvailable, vm.RemainingSeatsAvailable, vm.HoursOfOperation, vm.ConditionsForUse
                    );

                    SqlHelper.ExecuteNonQuery(sql);

                    vm.SiteID = SiteID.Value; // set the ID since it now exists
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

        public ActionResult Shelter()
        {
            // Get the logged in Site ID from the session
            int? SiteID = Session["SiteID"] as int?;

            // if there is none, redirect to the login page
            if (!SiteID.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            // set up the response object
            ShelterViewModel vm = new ShelterViewModel();

            // set up the sql query
            string sql = String.Format(
                "SELECT MaleBunksAvailable, FemaleBunksAvailable, MixedBunksAvailable, RoomsAvailable, HoursOfOperaion, ConditionsForUse " +
                "FROM shelter WHERE SiteID = {0}; ", SiteID.Value.ToString());

            // run the sql against the db
            object[] result = SqlHelper.ExecuteSingleSelect(sql, 6);

            // if we got a result, populate the view model fields
            if (result != null)
            {
                vm.SiteID = SiteID.Value;
                vm.MaleBunksAvailable = int.Parse(result[0].ToString());
                vm.FemaleBunksAvailable = int.Parse(result[1].ToString());
                vm.MixedBunksAvailable = int.Parse(result[2].ToString());
                vm.RoomsAvailable = int.Parse(result[3].ToString());
                vm.HoursOfOperation = result[4].ToString();
                vm.ConditionsForUse = result[5].ToString();
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Shelter(ShelterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                // this is needed for some reason.. come back to it
                // http://stackoverflow.com/questions/4837744/hiddenfor-not-getting-correct-value-from-view-model
                ModelState.Remove("SiteID");

                // find out if the soup kitchen exists for this SiteID already
                // and set up the SQL to INSERT or UPDATE accordingly

                if (vm.SiteID.Equals(0))
                {
                    // we didn't find an existing soup kitchen. so insert a new one based on the logged in users Site ID
                    int? SiteID = Session["SiteID"] as int?;

                    string sql = String.Format(
                        "INSERT INTO shelter (SiteID, MaleBunksAvailable, FemaleBunksAvailable, MixedBunksAvailable, RoomsAvailable, HoursOfOperaion, ConditionsForUse) " +
                        "VALUES ({0}, {1}, {2}, '{3}', '{4}', '{5}, {6}); ",
                        SiteID.Value.ToString(), vm.MaleBunksAvailable, vm.FemaleBunksAvailable, vm.MixedBunksAvailable, vm.RoomsAvailable, vm.HoursOfOperation, vm.ConditionsForUse
                    );
                    
                    SqlHelper.ExecuteNonQuery(sql);

                    vm.SiteID = SiteID.Value; // set the ID since it now exists
                    vm.StatusMessage = "Succesfully added!";
                }
                else
                {
                    // update the existing record 

                    string sql = String.Format(
                        "UPDATE shelter " +
                        "SET MaleBunksAvailable = {0}, " +
                        "FemaleBunksAvailable = {1}, " +
                        "MixedBunksAvailable = {2}, " +
                        "RoomsAvailable = {3}, " +
                        "HoursOfOperaion = '{4}', " +
                        "ConditionsForUse = '{5}'; ",
                        vm.MaleBunksAvailable, vm.FemaleBunksAvailable, vm.MixedBunksAvailable, vm.RoomsAvailable, vm.HoursOfOperation, vm.ConditionsForUse, vm.SiteID
                    );

                    SqlHelper.ExecuteNonQuery(sql);

                    vm.StatusMessage = "Succesfully updated!";
                }

            }
            return View(vm);
        }

        [Route("Site/DeleteService/{serviceType}")]
        public ActionResult DeleteService(string serviceType)
        {
            // Get the logged in Site ID from the session
            int? SiteID = Session["SiteID"] as int?;

            // if there is none, they can't do anything so redirect to the login page
            if (!SiteID.HasValue) return RedirectToAction("Login", "Account");

            // start setting up the view model
            DeleteServiceViewModel vm = new DeleteServiceViewModel();
            vm.SiteID = SiteID.Value;

            // now find out if this siteID has a service of the specified type
            string sql = null;

            switch (serviceType.ToLower())
            {
                case "foodbank":
                    sql = String.Format("SELECT COUNT(SiteID) FROM foodbank WHERE SiteID = {0}", SiteID.Value);
                    vm.ServiceType = "Food Bank";
                    break;
                case "foodpantry":
                    sql = String.Format("SELECT COUNT(SiteID) FROM foodpantry WHERE SiteID = {0}", SiteID.Value);
                    vm.ServiceType = "Food Pantry";
                    break;
                case "shelter":
                    sql = String.Format("SELECT COUNT(SiteID) FROM shelter WHERE SiteID = {0}", SiteID.Value);
                    vm.ServiceType = "Shelter";
                    break;
                case "soupkitchen":
                    sql = String.Format("SELECT COUNT(SiteID) FROM soupkitchen WHERE SiteID = {0}", SiteID.Value);
                    vm.ServiceType = "Soup Kitchen";
                    break;
                default:
                    throw new Exception("SiteController.DeleteService: non-supported serviceType");
            }

            int count = int.Parse(SqlHelper.ExecuteScalar(sql).ToString());

            if (count > 0)
            {
                vm.ServiceExists = true;
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Site/DeleteService/{serviceType}")]
        public ActionResult DeleteService(string serviceType, DeleteServiceViewModel vm)
        {
            string sql = null;

            switch (serviceType.ToLower())
            {
                case "foodbank":
                    sql = String.Format("DELETE FROM foodbank WHERE SiteID = {0}", vm.SiteID);
                    break;
                case "foodpantry":
                    sql = String.Format("DELETE FROM foodpantry WHERE SiteID = {0}", vm.SiteID);
                    break;
                case "shelter":
                    sql = String.Format("DELETE FROM shelter WHERE SiteID = {0}", vm.SiteID);
                    break;
                case "soupkitchen":
                    sql = String.Format("DELETE FROM soupkitchen WHERE SiteID = {0}", vm.SiteID);
                    break;
                default:
                    throw new Exception("SiteController.DeleteService: non-supported serviceType");
            }

            SqlHelper.ExecuteNonQuery(sql);

            vm.DeleteCompleted = true;

            return View(vm);
        }
    }
}