using ASACS5.Models.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MySql.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace ASACS5.Controllers
{
    public class SiteController : Controller
    {
        const string connStr = "server=localhost;user=root;database=cs6400_sp17_team090;port=3306;password=admin;";

        // GET: Site
        public ActionResult Index()
        {
            // Get the logged in Site ID from the session
            // Run SQL to find the data and populate the view model:

            SiteIndexViewModel vm = new SiteIndexViewModel();

            vm.SiteID = 52;
            vm.HasFoodBank = true;
            vm.HasSoupKitchen = true;

            return View(vm);
        }

        public ActionResult SoupKitchen()
        {
            // Get the logged in Site ID from the session
            int SiteID = 1;

            // Get the SoupKitchen from the DB if it exists. otherwise it will return empty vm
            SoupKitchenViewModel vm = GetSoupKitchen(SiteID);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SoupKitchen(SoupKitchenViewModel vm)
        {
            // do the updates with the new data

            vm.StatusMessage = "Succesfully updated!";

            return View(vm);
        }

        private SoupKitchenViewModel GetSoupKitchen(int SiteID)
        {
            // set up the response object
            SoupKitchenViewModel sk = new SoupKitchenViewModel();

            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();

                string sql = String.Format(
                    "SELECT TotalSeatsAvailable, RemainingSeatsAvailable, HoursOfOperaion, ConditionsForUse " +
                    "FROM soupkitchen WHERE SiteID = {0}; ", SiteID.ToString());

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                // if there are no rows, return empty object
                if (!rdr.HasRows) return sk;

                while (rdr.Read())
                {
                    sk.SiteID = SiteID;
                    sk.TotalSeatsAvailable = int.Parse(rdr[0].ToString());
                    sk.RemainingSeatsAvailable = int.Parse(rdr[1].ToString());
                    sk.HoursOfOperation = rdr[2].ToString();
                    sk.ConditionsForUse = rdr[3].ToString();
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            conn.Close();

            return sk;
        }
    }
}