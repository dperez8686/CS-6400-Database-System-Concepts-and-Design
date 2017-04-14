using ASACS5.Models.Site;
using ASACS5.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Diagnostics;
using ASACS5.Models.Clients;
using ASACS5.Models.Logs;

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
                //Determine if DescriptiveID is found within table
                queryResult = SqlHelper.ExecuteScalar(String.Format("SELECT COUNT(ClientID) FROM client WHERE DescriptiveID = '{0}'", vm.DescriptiveID));
                //If DescriptiveID is not found, update database. If not, display error message.
                if (queryResult.ToString() == "0")
                {
                    string sql = String.Format(
                        "INSERT INTO client (DescriptiveID, FirstName, MiddleName, LastName, PhoneNumber) " +
                        "VALUES ('{0}','{1}', '{2}', '{3}', '{4}'); ",
                        vm.DescriptiveID, vm.FirstName, vm.MiddleName, vm.LastName, vm.PhoneNumber
                    );

                    SqlHelper.ExecuteNonQuery(sql);

                    // Get Client ID and add entry into log table saying client was added
                    sql = String.Format(
                    "SELECT ClientID" +
                        " FROM client WHERE DescriptiveID = '{0}'; ", vm.DescriptiveID);
                    
                    object[] result = SqlHelper.ExecuteSingleSelect(sql, 1);
                    int clientID = int.Parse(result[0].ToString());

                    // TODO: Fix the whole issue with getting the correct Site ID.
                    sql = String.Format(
                        "INSERT INTO clientlogentry (ClientID, SiteName, Description) " +
                        "VALUES ({0}, '{1}', '{2}'); ",
                        clientID, "Site Name", "Profile created"
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

        public ActionResult SearchClient()
        {
            // Get the logged in Site ID from the session
            int? SiteID = Session["SiteID"] as int?;

            // if there is none, redirect to the login page
            if (!SiteID.HasValue) return RedirectToAction("Login", "Account");


            // set up the response object
            SearchClientViewModel vm = new SearchClientViewModel();
            vm.DisplayQueryResults = false;

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchClient(SearchClientViewModel vm)
        {
            if (ModelState.IsValid)
            {
                // this is needed for some reason.. come back to it
                // http://stackoverflow.com/questions/4837744/hiddenfor-not-getting-correct-value-from-view-model
                ModelState.Remove("SiteID");


                object queryResult = null;
                //Determine if DescriptiveID is found within table
                // TODO: For some reason, we're not getting count of 0. Fix this. 
                queryResult = SqlHelper.ExecuteScalar(String.Format("SELECT COUNT(*) FROM client WHERE DescriptiveID='{0}' OR FirstName LIKE '%{1}%' AND LastName LIKE '%{2}%'", vm.DescriptiveID, vm.FirstName,vm.LastName));

                // If less than 5 results found in query, display list. If not, display appropiate messages. 
                if (int.Parse(queryResult.ToString()) == 0)
                {
                    vm.StatusMessage = "No results found. Try search again.";
                }
                else if (int.Parse(queryResult.ToString()) < 5)
                {
                    // set up the sql query
                    string sql = String.Format(
                        "SELECT ClientID, DescriptiveID, FirstName, MiddleName, LastName, PhoneNumber " +
                        "FROM client WHERE DescriptiveID='{0}' OR FirstName LIKE '%{1}%' AND LastName LIKE '%{2}%' ", vm.DescriptiveID, vm.FirstName, vm.LastName);

                    // run the sql against the db
                    List <object[]> result = SqlHelper.ExecuteMultiSelect(sql, 6);

                    // if we got a result, populate the view model fields
                    if (result != null)
                    {
                        vm.DisplayQueryResults = true;
                        vm.Clients = GetClientListFromQueryResponse(result);
                        return View(vm);
                    }

                }
                else
                {
                    vm.StatusMessage = "More than 5 entries exist. Please enter more specific query.";
                }

            }
            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectClient(SearchClientViewModel vm)
        {
            
            string sql = String.Format(
                "SELECT DescriptiveID, FirstName, MiddleName, LastName, PhoneNumber " +
                "FROM client WHERE ClientID = {0}; ", vm.selectedClient.ToString());
            SelectClientViewModel om = new SelectClientViewModel();
            // run the sql against the db
            object[] result = SqlHelper.ExecuteSingleSelect(sql, 5);

            
            // if we got a result, populate the view model fields
            if (result != null)
            {
                om.ClientID = vm.selectedClient;
                om.DescriptiveID = result[0].ToString();
                om.FirstName = result[1].ToString();
                om.MiddleName = result[2].ToString();
                om.LastName = result[3].ToString();
                om.PhoneNumber = result[4].ToString();
                om.oldDescriptiveID = result[0].ToString();
                om.oldFirstName = result[1].ToString();
                om.oldMiddleName = result[2].ToString();
                om.oldLastName = result[3].ToString();
                om.oldPhoneNumber = result[4].ToString();

                string sql2 = String.Format(
                       "SELECT LogID, ClientID, DateTimeStamp, SiteName, Description " +
                       "FROM clientlogentry WHERE ClientID = {0} ", om.ClientID);

                // run the sql against the db
                List<object[]> result2 = SqlHelper.ExecuteMultiSelect(sql2, 5);

                // if we got a result, populate the view model fields
                if (result2 != null)
                {
                    om.Logs = GetLogListFromQueryResponse(result2);
                }

            }

            return View(om);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateClient(SelectClientViewModel vm)
        {

            string sql = String.Format(
                "UPDATE client SET DescriptiveID = '{0}', " +
                "FirstName = '{1}', " +
                "MiddleName = '{2}', " +
                "LastName = '{3}', " +
                "PhoneNumber = '{4}' " +
                "WHERE ClientID = {5}; ", 
                vm.DescriptiveID.ToString(), vm.FirstName.ToString(), vm.MiddleName, vm.LastName.ToString(), 
                vm.PhoneNumber.ToString(), vm.ClientID);

            SqlHelper.ExecuteNonQuery(sql);

            string mod_string = String.Format("Profile updated/edited: ");
            if (vm.oldDescriptiveID != vm.DescriptiveID)
            {
                mod_string += String.Format("DescriptiveID changed from {0} to {1}; ", vm.oldDescriptiveID, vm.DescriptiveID);
            }
            if (vm.oldFirstName != vm.FirstName)
            {
                mod_string += String.Format("First name changed from {0} to {1}; ",vm.oldFirstName,vm.FirstName);
            }
            if (vm.oldMiddleName != vm.MiddleName)
            {
                mod_string += String.Format("Middle name changed from {0} to {1}; ", vm.oldMiddleName, vm.MiddleName);
            }
            if (vm.oldLastName != vm.LastName)
            {
                mod_string += String.Format("Last name changed from {0} to {1}; ", vm.oldLastName, vm.LastName);
            }
            if (vm.oldPhoneNumber != vm.PhoneNumber)
            {
                mod_string += String.Format("Phone Number changed from {0} to {1}; ", vm.oldPhoneNumber, vm.PhoneNumber);
            }


                sql = String.Format(
                        "INSERT INTO clientlogentry (ClientID, SiteName, Description) " +
                        "VALUES ({0}, '{1}', '{2}'); ",
                        vm.ClientID, "Site Name", mod_string
                    );

            SqlHelper.ExecuteNonQuery(sql);

            if (vm.LogEntry != null)
            {
                sql = String.Format(
                    "INSERT INTO clientlogentry (ClientID, SiteName, Description) " +
                    "VALUES ({0}, '{1}', '{2}'); ",
                    vm.ClientID, "Site Name", vm.LogEntry
                );

                SqlHelper.ExecuteNonQuery(sql);
            }

            UpdateClientViewModel om = new UpdateClientViewModel();

            om.StatusMessage = "Succesfully updated!";


            // TODO: Try and get site name for logging purposes. 
            int? SiteID = Session["SiteID"] as int?;
            if (SiteID.HasValue) vm.SiteID = SiteID.Value;

            vm.SiteName = Session["SiteName"] as string;
 
            return View(om);

        }

        
        public ActionResult AddManualLogIndex(SelectClientViewModel vm)
        {
            Log om = new Log();
            om.ClientID = vm.ClientID;
            return View(om);
        }


        private List<Log> GetLogListFromQueryResponse(List<object[]> queryResponse)
        {
            var response = new List<Log>();

            foreach (object[] row in queryResponse)
            {
                // create a new client and add it to the Client List for each row in the query results
                response.Add(new Log
                {
                    LogID = int.Parse(row[0].ToString()),
                    ClientID = int.Parse(row[1].ToString()),
                    DateTimeStamp = row[2].ToString(),
                    SiteName = row[3].ToString(),
                    Description = row[4].ToString(),
                });
            }

            return response;
        }

        private List<Client> GetClientListFromQueryResponse(List<object[]> queryResponse)
        {
            var response = new List<Client>();

            foreach (object[] row in queryResponse)
            {
                // create a new client and add it to the Client List for each row in the query results
                response.Add(new Client
                {
                    ClientID = int.Parse(row[0].ToString()),
                    DescriptionID = row[1].ToString(),
                    FirstName = row[2].ToString(),
                    MiddleName = row[3].ToString(),
                    LastName = row[4].ToString(),
                    PhoneNumber = row[5].ToString()
                });
            }

            return response;
        }
    }
}