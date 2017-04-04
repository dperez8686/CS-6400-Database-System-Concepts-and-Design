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

            return View(vm);
        }
    }
}