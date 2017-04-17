using ASACS5.Models.Reports;
using System.Web.Mvc;
using System.Data;
using System;
using ASACS5.Models.Items;
using ASACS5.Services;
using System.Collections.Generic;
using System.Text;

using System.Linq;

namespace ASACS5.Controllers
{
	public class ReportsController : Controller
	{
		private object result;

		public ActionResult Index()
		{
            return View();
		}

		public ActionResult MealsRemaining()
		{
			// set up the response object
			MealsRemainingViewModel vm = new MealsRemainingViewModel();

			// Find the sums of all non-expired items by supported sub-category.
            // Sort by the fewest count first; therefore the first item will be the category most-needed
            //
			string sql = String.Format("SELECT Category2, SUM(NumberOfUnits) " +
                                    "FROM item " +
                                    "WHERE Category1 = 'Food'  " +
                                    "  AND ExpirationDate > NOW()  " +
                                    "  AND Category2 in ('Meat/seafood', 'Vegetables', 'Dairy/eggs', 'Nuts/grains/beans') " +
                                    "GROUP BY Category2 " +
                                    "ORDER BY SUM(NumberOfUnits) ASC ;");

            List<object[]> queryResponse = SqlHelper.ExecuteMultiSelect(sql, 2);

            if (queryResponse != null && queryResponse.Count > 0)
            {
                vm.CategoryOfFood = queryResponse.First()[0].ToString();
                vm.MaxMealsAvaible = Int32.Parse(queryResponse.First()[1].ToString());
            }

            return View(vm);
		}

        public ActionResult RoomsBunksAvailability()
        {
            RoomsBunksAvailabilityViewModel vm = new RoomsBunksAvailabilityViewModel();

            // set up the sql query
            string sql = "SELECT s.SiteID, s.SiteName, s.City, s.State, s.PrimaryContactNumber, " +
                        "sh.MaleBunksAvailable, sh.FemaleBunksAvailable, sh.MixedBunksAvailable, " +
                        "sh.RoomsAvailable, sh.HoursOfOperation, sh.ConditionsForUse " +
                        "FROM site s " +
                        "INNER JOIN shelter sh on s.SiteID = sh.SiteID " +
                        "WHERE(sh.MaleBunksAvailable > 0 OR sh.FemaleBunksAvailable > 0 OR sh.MixedBunksAvailable > 0 OR sh.RoomsAvailable > 0);";

            List<object[]> queryResponse = SqlHelper.ExecuteMultiSelect(sql, 11);

            vm.reportRows = new List<RoomsReportRow>();

            if (queryResponse != null && queryResponse.Count > 0)
			{
                foreach(object[] row in queryResponse)
                {
                    vm.reportRows.Add(new RoomsReportRow
                    {
                        SiteID = Int32.Parse(row[0].ToString()),
                        SiteName = row[1].ToString(),
                        City = row[2].ToString(),
                        State = row[3].ToString(),
                        PrimaryContactNumber = row[4].ToString(),
                        MaleBunksAvailable = Int32.Parse(row[5].ToString()),
                        FemaleBunksAvailable = Int32.Parse(row[6].ToString()),
                        MixedBunksAvailable = Int32.Parse(row[7].ToString()),
                        RoomsAvailable = Int32.Parse(row[8].ToString()),
                        HoursOfOperation = row[9].ToString(),
                        ConditionsForUse = row[10].ToString()
                    });
                }
			}
		
			return View(vm);
        }

		
	} 


}