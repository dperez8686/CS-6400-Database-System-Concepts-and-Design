using ASACS5.Models.Items;
using ASACS5.Models.Requests;
using ASACS5.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Diagnostics;
using System.Text;

namespace ASACS5.Controllers
{
    public class RequestsController : Controller
    {
        // GET: Site
        public ActionResult Index()
        {
            // Get the logged in Site ID from the session
            int? SiteID = Session["SiteID"] as int?;

            // if there is none, redirect to the login page
            if (!SiteID.HasValue) return RedirectToAction("Login", "Account");

            // find out if the current Site has a Food Bank or not
            var hasFoodBank = Int32.Parse(SqlHelper.ExecuteScalar("SELECT COUNT(*) FROM foodbank WHERE SiteID = " + SiteID.Value).ToString()) > 0;

            if (!hasFoodBank) throw new Exception("Access Denied to Outstanding Requests Report. Site ID " + SiteID.Value.ToString() + " does not have a food bank.");

            RequestsIndexViewModel vm = new RequestsIndexViewModel
            {
                SiteID = SiteID.Value,
                SiteName = Session["SiteName"].ToString(),
                SortByChoiceOptions = GetSortByChoiceOptions(),
                SortByOrderOptions = GetSortByOrderOptions(),
                SortByChoice = GetSortByChoiceOptions().First().Value,
                SortByOrder = GetSortByOrderOptions().First().Value
            };

            string sql = String.Format("SELECT r.RequestID, i.ItemName, i.Category1, i.Category2, i.StorageType, r.RequestedQuantity, r.Status, r.FulfilledQuantity, i.ItemID " +
                "FROM request r " + 
                "INNER JOIN item i on i.ItemID = r.ItemID " +
                "WHERE i.SiteID = {0} AND r.Status='Pending' " +
                "ORDER BY {1} {2} ;", SiteID.Value.ToString(), vm.SortByChoice, vm.SortByOrder);

            vm.Requests = GetRequests(sql);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(RequestsIndexViewModel vm)
        {
            // Get the logged in Site ID from the session
            int? SiteID = Session["SiteID"] as int?;

            // if there is none, redirect to the login page
            if (!SiteID.HasValue) return RedirectToAction("Login", "Account");

            ModelState.Remove("Items");

            vm.SortByChoiceOptions = GetSortByChoiceOptions();
            vm.SortByOrderOptions = GetSortByOrderOptions();

            string sql = String.Format("SELECT r.RequestID, i.ItemName, i.Category1, i.Category2, i.StorageType, r.RequestedQuantity, r.Status, r.FulfilledQuantity, i.ItemID " +
                "FROM request r " +
                "INNER JOIN item i on i.ItemID = r.ItemID " +
                "WHERE i.SiteID = {0} AND r.Status='Pending' " +
                "ORDER BY {1} {2} ;", SiteID.Value.ToString(), vm.SortByChoice, vm.SortByOrder);

            vm.Requests = GetRequests(sql);

            return View(vm);
        }

        public ActionResult MyRequests()
        {
            // Get the logged in Site ID from the session
            int? SiteID = Session["SiteID"] as int?;

            // if there is none, redirect to the login page
            if (!SiteID.HasValue) return RedirectToAction("Login", "Account");

            MyRequestsViewModel vm = new MyRequestsViewModel
            {
                SiteID = SiteID.Value
            };

            string sql = String.Format("SELECT r.RequestID, i.ItemName, i.Category1, i.Category2, i.StorageType, r.RequestedQuantity, r.Status, r.FulfilledQuantity, i.ItemID " +
                "FROM request r " +
                "INNER JOIN item i on i.ItemID = r.ItemID " +
                "WHERE r.Username = '{0}' " +
                "ORDER BY r.RequestID DESC ;", Session["Username"].ToString());

            vm.Requests = GetRequests(sql);

            // update the Closed property to a more informative value based on the fulfilled / requested quantities
            foreach (Request r in vm.Requests)
            {
                if (r.Status.Equals("Closed"))
                {
                    if (r.FulfilledQuantity.Equals(r.RequestedQuantity))
                    {
                        r.Status = "Fulfilled";
                    }
                    else if (r.FulfilledQuantity > 0)
                    {
                        r.Status = "Partially Fulfilled";
                    }
                    else r.Status = "Rejected";
                }
            }

            return View(vm);
        }

        [Route("Requests/MyRequests/Delete/{RequestID:int}")]
        public ActionResult DeleteMyRequest(int RequestID)
        {
            // Get the logged in Site ID from the session
            int? SiteID = Session["SiteID"] as int?;

            // if there is none, redirect to the login page
            if (!SiteID.HasValue) return RedirectToAction("Login", "Account");

            // get the specified request to see if it exists and this user is allowed to delete it
            string sql = String.Format("SELECT r.Username, r.Status FROM request r " +
                "WHERE r.RequestID = {0} ", RequestID.ToString());

            var queryResult = SqlHelper.ExecuteSingleSelect(sql, 2);

            if (queryResult == null)
            {
                throw new Exception("No Request found for Request ID " + RequestID.ToString());
            }
            else if (queryResult[0].ToString() != Session["Username"].ToString())
            {
                throw new Exception("Request ID " + RequestID.ToString() + " does not belong to your username");
            }
            else if (queryResult[1].ToString() != "Pending")
            {
                throw new Exception("Request ID " + RequestID.ToString() + " has already been closed and cannot be deleted.");
            }

            // they are, so now delete the request
            string sql2 = String.Format("DELETE FROM request WHERE RequestID = {0} ;", RequestID.ToString());

            SqlHelper.ExecuteNonQuery(sql2);

            // send them back to index
            return RedirectToAction("MyRequests");
        }

        [Route("Requests/Add/{ItemID:int}")]
        public ActionResult Add(int ItemID)
        {
            // Get the logged in Site ID from the session
            int? SiteID = Session["SiteID"] as int?;

            // if there is none, redirect to the login page
            if (!SiteID.HasValue) return RedirectToAction("Login", "Account");

            Item itemToRequest = ItemService.GetItemById(ItemID);

            if (itemToRequest == null) throw new Exception("No item exists for Item ID " + ItemID.ToString());

            AddRequestViewModel vm = new AddRequestViewModel {
                ItemID = ItemID,
                UserSiteName = Session["SiteName"].ToString(),
                OwnerSiteID = itemToRequest.SiteID,
                ItemName = itemToRequest.ItemName,
                NumberOfUnits = itemToRequest.NumberOfUnits,
                MaxNumberOfUnits = itemToRequest.NumberOfUnits,
                ExpirationDate = itemToRequest.ExpirationDate
            };

            return View(vm);
        }

        [HttpPost]
        [Route("Requests/Add/{ItemID:int}")]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddRequestViewModel vm, int ItemID)
        {
            if (vm.NumberOfUnits > vm.MaxNumberOfUnits)
            {
                vm.ErrorMessage = "The maximum quanity available for this item is " + vm.MaxNumberOfUnits.ToString();
            }
            else if (ModelState.IsValid)
            {
                string username = Session["Username"].ToString();

                string sql = String.Format("INSERT INTO request (Username, ItemID, RequestedQuantity, Status) " +
                                           "VALUES ('{0}', {1}, {2}, 'Pending') ;", username, vm.ItemID, vm.NumberOfUnits);

                SqlHelper.ExecuteNonQuery(sql);

                vm.StatusMessage = "Succesfully added!";
                vm.Success = true;

            }
            return View(vm);
        }

        [Route("Requests/Update/{RequestID:int}")]
        public ActionResult Update(int RequestID)
        {
            // Get the logged in Site ID from the session
            int? SiteID = Session["SiteID"] as int?;

            // if there is none, redirect to the login page
            if (!SiteID.HasValue) return RedirectToAction("Login", "Account");

            UpdateRequestViewModel vm = new UpdateRequestViewModel();

            string sql = String.Format("SELECT s.SiteName, u.Username, i.ItemName, r.RequestedQuantity as 'RequestedQuantity', i.NumberOfUnits as 'QuantityAvailable', i.SiteID as 'OwnerSiteID', i.ItemID " +
                "FROM request r " +
                "INNER JOIN item i on i.ItemId = r.ItemID " +
                "INNER JOIN user u on r.Username = u.Username " +
                "INNER JOIN site s on u.SiteID = s.SiteID " +
                "WHERE r.RequestId = {0} AND r.Status='Pending' ;", RequestID.ToString());

            object[] queryResponse = SqlHelper.ExecuteSingleSelect(sql, 7);

            if (queryResponse != null)
            {
                vm.RequestorSiteName = queryResponse[0].ToString();
                vm.RequestorUsername = queryResponse[1].ToString();
                vm.ItemName = queryResponse[2].ToString();
                vm.QuantityRequested = int.Parse(queryResponse[3].ToString());
                vm.QuantityAvailable = int.Parse(queryResponse[4].ToString());
                vm.OwnerSiteID = int.Parse(queryResponse[5].ToString());
                vm.ItemID = int.Parse(queryResponse[6].ToString());
            }
            else throw new Exception("No pending request found with RequestID = " + RequestID.ToString());

            // setup the default Quantity to Fulfill
            if (vm.QuantityRequested > vm.QuantityAvailable)
                vm.QuantityToFulfill = vm.QuantityAvailable;
            else vm.QuantityToFulfill = vm.QuantityRequested;

            if (vm.OwnerSiteID != SiteID.Value) throw new Exception("You cannot update a request that is not for an item in your Site.");

            return View(vm);
        }

        [HttpPost]
        [Route("Requests/Update/{RequestID:int}")]
        [ValidateAntiForgeryToken]
        public ActionResult Update(UpdateRequestViewModel vm, int RequestID)
        {
            if (vm.QuantityToFulfill > vm.QuantityAvailable || vm.QuantityToFulfill > vm.QuantityRequested)
            {
                vm.ErrorMessage = "Quantity To Fulfill must be less than or equal to the Quantity Requested and Quantity Available";
            }
            else if (ModelState.IsValid)
            {
                string username = Session["Username"].ToString();

                // Ideally the below  2 queries will be done in a transaction

                // Update the request to Closed status with the Fulfilled Quantity
                string sql = String.Format("UPDATE request SET FulfilledQuantity = {0}, Status='Closed' " +
                                           "WHERE RequestID = {1} ;", vm.QuantityToFulfill, RequestID);

                SqlHelper.ExecuteNonQuery(sql);

                // Update the Item Inventory as appropriate
                string sql2 = String.Format("UPDATE item SET NumberOfItems = (NumberOfItems - {0}) WHERE ItemID = {1} ;",
                                        vm.QuantityToFulfill, vm.ItemID);

                SqlHelper.ExecuteNonQuery(sql2);

                vm.StatusMessage = "Succesfully completed!!";
                vm.Success = true;

            }
            return View(vm);
        }

        private List<Request> GetRequests(string sql)
        {
            var response = new List<Request>();

            // run the SQL
            List<object[]> queryResponse = SqlHelper.ExecuteMultiSelect(sql, 9);

            foreach (object[] row in queryResponse)
            {
                // create a new request and add it to the Requests List for each row in the query results
                response.Add(new Request
                {
                    RequestID = int.Parse(row[0].ToString()),
                    ItemName = row[1].ToString(),
                    Category = row[2].ToString(),
                    SubCategory = row[3].ToString(),
                    StorageType = row[4].ToString(),
                    RequestedQuantity = int.Parse(row[5].ToString()),
                    Status = row[6].ToString(),
                    FulfilledQuantity = int.Parse(row[7].ToString()),
                    ItemID = int.Parse(row[8].ToString())
                });
            }

            // we need to meet this requirement: If more than one request for a particular item exists
            // such that the total number of items requested is greater than the total number available, the report
            // should indicate this shortfall in inventory to the user by highlighting numbers in red or some other
            // suitable method.

            List<int> ItemIDsOverRequestLimit = GetItemIDsOverRequestLimit();

            foreach (Request r in response)
            {
                if (ItemIDsOverRequestLimit.Contains(r.ItemID)) r.OverTheLimit = true;
            }

            return response;
        }

        private List<int> GetItemIDsOverRequestLimit()
        {
            // Get all the Items for which the requests are over the limit
            string sql = "select r.ItemID from request r " +
                          "where r.Status = 'Pending' " +
                          "group by r.ItemID having sum(r.RequestedQuantity) > " +
                          "(select i.NumberOfUnits from item i where i.ItemID = r.ItemID); ";

            var result = SqlHelper.ExecuteMultiSelect(sql, 1);

            List<int> response = new List<int>();

            foreach (object[] x in result)
            {
                response.Add(Int32.Parse(x[0].ToString()));
            }

            return response;
        }

        private IEnumerable<SelectListItem> GetSortByChoiceOptions()
        {
            var selectListItems = new List<SelectListItem>();

            selectListItems.Add(new SelectListItem { Value = "i.Category1", Text = "Category" });
            selectListItems.Add(new SelectListItem { Value = "i.Category2", Text = "Sub-Category" });
            selectListItems.Add(new SelectListItem { Value = "i.StorageType", Text = "Storage Type" });
            selectListItems.Add(new SelectListItem { Value = "r.RequestedQuantity", Text = "Requested Quantity" });

            return new SelectList(selectListItems, "Value", "Text");
        }

        private IEnumerable<SelectListItem> GetSortByOrderOptions()
        {
            var selectListItems = new List<SelectListItem>();

            selectListItems.Add(new SelectListItem { Value = "ASC", Text = "Ascending" });
            selectListItems.Add(new SelectListItem { Value = "DESC", Text = "Descending" });

            return new SelectList(selectListItems, "Value", "Text");
        }

    }
}