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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Client(AddClientViewModel vm)
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
                        "INSERT INTO client (ClientID, DescriptiveID, FirstName, MiddleName, LastName, PhoneNumber) " +
                        "VALUES ({0}, '{1}', '{2}', '{3}', '{4}', '{5}'); ",
                        1, vm.ClientID, vm.DescriptiveID, vm.FirstName, vm.MiddleName, vm.LastName, vm.PhoneNumber
                    );
                    
                    SqlHelper.ExecuteNonQuery(sql);

                    vm.SiteID = SiteID.Value; // set the ID since it now exists
                    vm.StatusMessage = "Succesfully added!";
                }
                else
                {
                    // update the existing record 

                    string sql = String.Format(
                        "UPDATE client " +
                        "SET ClientID = {0}, " +
                        "DescriptiveID = '{1}', " +
                        "FirstName = '{2}', " +
                        "MiddleName = '{3}', " +
                        "LastName = '{4}', " +
                        "PhoneNumber = '{5}'; ",
                        vm.DescriptiveID, vm.FirstName, vm.MiddleName, vm.LastName, vm.PhoneNumber, vm.ClientID
                    );

                    SqlHelper.ExecuteNonQuery(sql);

                    vm.StatusMessage = "Succesfully updated!";
                }

            }
            return View(vm);
        }

       
    }
}