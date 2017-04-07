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
    public class ClientController : Controller
    {

        public ActionResult Index()
        {
            // Get the logged in Site ID from the session
            int? SiteID = Session["SiteID"] as int?;

            // if there is none, redirect to the login page
            if (!SiteID.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            // set up the response object
            AddClientViewModel vm = new AddClientViewModel();



            return View(vm);
        }

        public ActionResult AddClient()
        {
            // Get the logged in Site ID from the session
            int? SiteID = Session["SiteID"] as int?;

            // if there is none, redirect to the login page
            if (!SiteID.HasValue) return RedirectToAction("Login", "Account");


            // set up the response object
            AddClientViewModel vm = new AddClientViewModel();


            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddClient(AddClientViewModel vm)
        {
            if (ModelState.IsValid)
            {
                // this is needed for some reason.. come back to it
                // http://stackoverflow.com/questions/4837744/hiddenfor-not-getting-correct-value-from-view-model
                ModelState.Remove("SiteID");


                object queryResult = null;

                queryResult = SqlHelper.ExecuteScalar(String.Format("SELECT COUNT(ClientID) FROM client WHERE DescriptiveID = '{0}'", vm.DescriptiveID));

                if (queryResult.ToString() == "0")
                {
                    string sql = String.Format(
                        "INSERT INTO client (DescriptiveID, FirstName, MiddleName, LastName, PhoneNumber) " +
                        "VALUES ('{0}','{1}', '{2}', '{3}', '{4}'); ",
                        vm.DescriptiveID, vm.FirstName, vm.MiddleName, vm.LastName, vm.PhoneNumber
                    );

                    SqlHelper.ExecuteNonQuery(sql);

                    vm.StatusMessage = "Succesfully added!";
                }
                else
                {
                    vm.StatusMessage = "Descriptive ID already found within database. Please enter new ID.";

                }

            }
            return View(vm);
        }

       
    }
}