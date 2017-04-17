using ASACS5.Models.Site;
using ASACS5.Services;
using ASACS5.Models.Waitlists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Diagnostics;

namespace ASACS5.Controllers
{
    public class WaitlistController : Controller
    {
        // GET: Site
        public ActionResult UpdateWaitlist()
        {
            // Get the logged in Site ID from the session
            int? SiteID = Session["SiteID"] as int?;

            // if there is none, redirect to the login page
            if (!SiteID.HasValue) return RedirectToAction("Login", "Account");

            // Set up the view model and run SQL to populate the properties

            UpdateWaitlistViewModel vm = new UpdateWaitlistViewModel { SiteID = SiteID.Value };

            // Determine if Site has Shelter first 
            object queryResult = null;
            queryResult = SqlHelper.ExecuteScalar(String.Format("SELECT COUNT(SiteID) FROM shelter WHERE SiteID = {0}", SiteID.Value));
            if (queryResult != null && int.Parse(queryResult.ToString()) > 0)
            {
                vm.HasShelter = true;
                
            } else
            {
                vm.HasShelter = false;
                return View(vm);
            }
            string sql = String.Format(
                       "SELECT ClientID, Ranking " +
                       "FROM waitlist WHERE SiteID = {0} ORDER BY RANKING ASC", SiteID);

            // run the sql against the db
            List<object[]> result = SqlHelper.ExecuteMultiSelect(sql, 2);

            if (result != null)
            {
                vm.Waitlist = GetWaitListFromQueryResponse(result);
            }



            return View(vm);
        }

        [Route("Waitlist/DeleteClient/{clientID}")]
        public ActionResult DeleteFromWaitlist(string clientID)
        {
            // Get the logged in Site ID from the session
            int? SiteID = Session["SiteID"] as int?;

            // if there is none, they can't do anything so redirect to the login page
            if (!SiteID.HasValue) return RedirectToAction("Login", "Account");

            // start setting up the view model
            DeleteFromWaitlistViewModel vm = new DeleteFromWaitlistViewModel();
            vm.SiteID = SiteID.Value;
            vm.ClientID = int.Parse(clientID); 

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Waitlist/DeleteClient")]
        public ActionResult DeleteFromWaitlist(DeleteFromWaitlistViewModel vm)
        {
            string sql = null;

            // Get client ranking on waitlist
            sql = String.Format("SELECT Ranking FROM waitlist WHERE ClientID = {0} AND SiteID = {1}", vm.ClientID, vm.SiteID);
            object ranking = SqlHelper.ExecuteScalar(sql);

            //Redo rankings decreasing ranking for anything lower on the list than selected client.
            sql = String.Format("UPDATE waitlist SET Ranking = Ranking - 1 WHERE Ranking > {0} AND SiteID = {1}", int.Parse(ranking.ToString()), vm.SiteID);
            SqlHelper.ExecuteNonQuery(sql);

            //Delete selected client.
            sql = String.Format("DELETE FROM waitlist WHERE ClientID = {0} AND SiteID = {1}", vm.ClientID, vm.SiteID);
            SqlHelper.ExecuteNonQuery(sql);

            vm.DeleteCompleted = true;
            return View(vm);
        }

        [Route("Waitlist/MoveInWaitlist/{clientID}/{direction}")]
        public ActionResult MoveInWaitlist(string clientID, string direction)
        {
            // Get the logged in Site ID from the session
            int? SiteID = Session["SiteID"] as int?;

            // if there is none, they can't do anything so redirect to the login page
            if (!SiteID.HasValue) return RedirectToAction("Login", "Account");

            MoveInWaitlistViewModel vm = new MoveInWaitlistViewModel();
            vm.ClientID = int.Parse(clientID);
            vm.SiteID = int.Parse(SiteID.ToString());

            string sql = String.Format(
                       "SELECT ClientID, Ranking " +
                       "FROM waitlist WHERE SiteID = {0} ORDER BY RANKING ASC", vm.SiteID);

            // run the sql against the db
            List<object[]> result = SqlHelper.ExecuteMultiSelect(sql, 2);
            List<Waitlist> current_Waitlist = GetWaitListFromQueryResponse(result);

            sql = String.Format("SELECT Ranking FROM waitlist WHERE ClientID = {0} AND SiteID = {1}", vm.ClientID, vm.SiteID);
            object result2 = SqlHelper.ExecuteScalar(sql);
            int ranking = int.Parse(result2.ToString());

            int count = 0;
            foreach (Waitlist entry in current_Waitlist)
            {
                current_Waitlist[count].oldRanking = current_Waitlist[count].Ranking;
                if (direction == "up")
                {
                    if (entry.Ranking == ranking - 1)
                    {
                        current_Waitlist[count].Ranking += 1;
                    }
                    else if (entry.Ranking == ranking)
                    {
                        current_Waitlist[count].Ranking -= 1;
                    }    
                }
                else if (direction == "down")
                {
                    if (entry.Ranking == ranking + 1)
                    {
                        current_Waitlist[count].Ranking -= 1;
                    }
                    else if (entry.Ranking == ranking)
                    {
                        current_Waitlist[count].Ranking += 1;
                    }
                }
                sql = String.Format("UPDATE waitlist SET Ranking = {0} WHERE ClientID = {1} AND SiteID = {2}", entry.Ranking, entry.ClientID, vm.SiteID);
                SqlHelper.ExecuteNonQuery(sql);
                count += 1;
            }
            List<Waitlist> SortedList = current_Waitlist.OrderBy(o => o.Ranking).ToList();
            vm.WaitlistList = SortedList;


            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Waitlist/MoveInWaitlist/{clientID}/{direction}")]
        public ActionResult MoveInWaitlist(string clientID, string direction, MoveInWaitlistViewModel vm)
        {
            string sql = null;
            // Get the logged in Site ID from the session
            int? SiteID = Session["SiteID"] as int?;

            // if there is none, they can't do anything so redirect to the login page
            if (!SiteID.HasValue) return RedirectToAction("Login", "Account");

            foreach (Waitlist entry in vm.WaitlistList)
            {
                sql = String.Format("UPDATE waitlist SET Ranking = {0} WHERE ClientID = {1} AND SiteID = {2}", entry.Ranking, entry.ClientID, vm.SiteID);
                SqlHelper.ExecuteNonQuery(sql);
            }

            vm.StatusMessage = "Waitlist successfully updated";
            return View(vm);
        }

        private List<Waitlist> GetWaitListFromQueryResponse(List<object[]> queryResponse)
        {
            var response = new List<Waitlist>();

            foreach (object[] row in queryResponse)
            {
                response.Add(new Waitlist
                {
                    ClientID = int.Parse(row[0].ToString()),
                    Ranking = int.Parse(row[1].ToString()),
                });
            }

            return response;
        }
    }     
}