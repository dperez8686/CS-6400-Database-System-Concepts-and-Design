using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ASACS5.Models.Home;
using ASACS5.Services;

namespace ASACS5.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            HomeIndexViewModel vm = new HomeIndexViewModel();

            // comment this out if you want to test the actual login.
            //Session["SiteID"] = 1;
            //Session["SiteName"] = "site1";
            //Session["Username"] = "emp1";

            // grab the Site info from session, if it exists
            int? SiteID = Session["SiteID"] as int?;
            if (SiteID.HasValue)
            {
                vm.SiteID = SiteID.Value;

                vm.SiteName = Session["SiteName"].ToString();
                vm.Username = Session["Username"].ToString();

                // find out if the current Site has a Food Bank or not
                vm.HasFoodBank = Int32.Parse(SqlHelper.ExecuteScalar("SELECT COUNT(*) FROM foodbank WHERE SiteID = " + SiteID.Value).ToString()) > 0;
            }

            return View(vm);
        }
    }
}