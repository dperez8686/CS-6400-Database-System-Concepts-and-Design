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
            Session["SiteID"] = 1;
            Session["SiteName"] = "Food City #2";
            Session["Username"] = "jeffro96";

            // grab the Site info from session, if it exists
            int? SiteID = Session["SiteID"] as int?;
            if (SiteID.HasValue) vm.SiteID = SiteID.Value;

            vm.SiteName = Session["SiteName"] as string;

            return View(vm);
        }
    }
}