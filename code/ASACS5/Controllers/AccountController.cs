using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ASACS5.Models.Account;
using ASACS5.Services;

namespace ASACS5.Controllers
{
    public class AccountController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            LoginViewModel vm = new LoginViewModel();

            // Find out if user is already logged in
            int? SiteID = Session["SiteID"] as int?;

            if (SiteID.HasValue) vm.AlreadyLoggedIn = true;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                // set up the SQL to check username and password
                string sql = String.Format(
                    "SELECT u.FirstName, u.SiteID, s.SiteName " +
                    "FROM user u " +
                    "INNER JOIN site s " +
                    "on u.SiteID = s.SiteID " +
                    "WHERE u.Username = '{0}' " +
                    "AND u.Password = '{1}'; ",
                    vm.Username, vm.Password
                );

                object[] result = SqlHelper.ExecuteSingleSelect(sql, 3);

                if (result != null)
                {
                    vm.FirstName = result[0].ToString(); // this is to show a welcome message
                    vm.LoginSuccess = true;

                    Session["SiteID"] = int.Parse(result[1].ToString()); // set the SiteID properties in the session
                    Session["SiteName"] = result[2].ToString();
                    Session["Username"] = vm.Username;
                }
                else
                {
                    vm.ErrorMessage = "No user was found with the specified information. Please try again";
                }
            }

            return View(vm);
        }
    }
}