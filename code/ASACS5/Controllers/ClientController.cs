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
using ASACS5.Models.Waitlists;

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
                int? SiteID = Session["SiteID"] as int?;

                if (SiteID.HasValue) vm.SiteID = SiteID.Value;

                vm.SiteName = Session["SiteName"] as string;
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

                    sql = String.Format(
                        "INSERT INTO clientlogentry (ClientID, SiteName, Description) " +
                        "VALUES ({0}, '{1}', '{2}'); ",
                        clientID, vm.SiteName, "Profile created"
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

                // Ugly way to build a query search for client based on Description ID, First Name and Last Name.
                // Query did not work well with null parameters so I had to ignore parameters if null.
                string querystring = "SELECT * FROM client WHERE ";
                Boolean parametersNotNull = false;
                Boolean firstParameter = false; 
                if (vm.DescriptiveID != null)
                {
                    firstParameter = true; 
                    querystring += String.Format("DescriptiveID='{0}' ", vm.DescriptiveID);
                    parametersNotNull = true;
                }
                if (vm.FirstName != null)
                {
                    if (firstParameter)
                    {
                        querystring += "AND ";
                    } else
                    {
                        firstParameter = true;
                    }
                    querystring += String.Format("FirstName LIKE '%{0}%' ", vm.FirstName);
                    parametersNotNull = true;
                }
                if (vm.LastName != null)
                {
                    if (firstParameter)
                    {
                        querystring += "AND ";
                    }
                    querystring += String.Format("LastName LIKE '%{0}%' ", vm.LastName);
                    parametersNotNull = true;
                }
                querystring += ";";
                List<object[]> queryResult = null;
                if (parametersNotNull)
                {
                    queryResult = SqlHelper.ExecuteMultiSelect(querystring, 6);
                }
                // If less than 5 results found in query, display list. If not, display appropiate messages. 
                if (queryResult == null)
                {
                    vm.StatusMessage = "No results found. Try search again.";
                }
                else if (queryResult.Count < 5)
                {
                    
                    // set up the sql query
                    string sql = String.Format(querystring);

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
            int? SiteID = Session["SiteID"] as int?;
            if (SiteID.HasValue) vm.SiteID = SiteID.Value;
            string sql = String.Format(
                "SELECT DescriptiveID, FirstName, MiddleName, LastName, PhoneNumber " +
                "FROM client WHERE ClientID = {0}; ", vm.selectedClient.ToString());
            SelectClientViewModel om = new SelectClientViewModel();
            // run the sql against the db
            object[] result = SqlHelper.ExecuteSingleSelect(sql, 5);

            
            // To auto generate log based on changes in values the old values have to be saved for later comparison.
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
                       "SELECT DateTimeStamp, ServiceName, SiteName, Description " +
                       "FROM clientlogentry WHERE ClientID = {0} ", om.ClientID);

                // run the sql against the db
                List<object[]> result2 = SqlHelper.ExecuteMultiSelect(sql2, 4);

                // if we got a result, populate the view model fields
                if (result2 != null)
                {
                    om.Logs = GetLogListFromQueryResponse(result2);
                }

                string sql3 = String.Format(
                       "SELECT ClientID, SiteID, Ranking " +
                       "FROM waitlist WHERE ClientID = {0} ", om.ClientID);

                // run the sql against the db
                List<object[]> result3 = SqlHelper.ExecuteMultiSelect(sql3, 3);

                if (result3 != null)
                {
                    om.Waitlist = GetWaitListFromQueryResponse(result3);
                }
                
                object queryResult = null;
                queryResult = SqlHelper.ExecuteScalar(String.Format("SELECT COUNT(ClientID) FROM waitlist WHERE SiteID = {0} AND ClientID = {1} ", SiteID, om.ClientID));

                // Add to waitlist will only appear if client is not currently on waitlist for site
                if (queryResult.ToString() == "0")
                {
                    om.AddClientToWaitlistAllowed = true;
                } else
                {
                    om.AddClientToWaitlistAllowed = false;
                }
            }
            

            //Get Available bunks 
            string sql4 = String.Format(
                       "SELECT MaleBunksAvailable, FemaleBunksAVailable, MixedBunksAvailable " +
                       "FROM shelter WHERE SiteID = {0} ", vm.SiteID);

            result = SqlHelper.ExecuteSingleSelect(sql4, 3);
            if (result != null)
            {
                om.MaleBunks = int.Parse(result[0].ToString());
                om.FemaleBunks = int.Parse(result[1].ToString());
                om.MixedBunks = int.Parse(result[2].ToString());
            }
                return View(om);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateClient(SelectClientViewModel vm)
        {

            int? SiteID = Session["SiteID"] as int?;
            

            if (SiteID.HasValue) vm.SiteID = SiteID.Value;

            vm.SiteName = Session["SiteName"] as string;

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
            Boolean updateHappened = false;
            if (vm.oldDescriptiveID != vm.DescriptiveID)
            {
                updateHappened = true;
                mod_string += String.Format("DescriptiveID changed from {0} to {1}; ", vm.oldDescriptiveID, vm.DescriptiveID);
            }
            if (vm.oldFirstName != vm.FirstName)
            {
                updateHappened = true;
                mod_string += String.Format("First name changed from {0} to {1}; ",vm.oldFirstName,vm.FirstName);
            }
            if (vm.oldMiddleName != vm.MiddleName)
            {
                updateHappened = true;
                mod_string += String.Format("Middle name changed from {0} to {1}; ", vm.oldMiddleName, vm.MiddleName);
            }
            if (vm.oldLastName != vm.LastName)
            {
                updateHappened = true;
                mod_string += String.Format("Last name changed from {0} to {1}; ", vm.oldLastName, vm.LastName);
            }
            if (vm.oldPhoneNumber != vm.PhoneNumber)
            {
                updateHappened = true;
                mod_string += String.Format("Phone Number changed from {0} to {1}; ", vm.oldPhoneNumber, vm.PhoneNumber);
            }

            // Log auto generated message for changing of values
            if (updateHappened)
            {
                sql = String.Format(
                        "INSERT INTO clientlogentry (ClientID, SiteName, Description) " +
                        "VALUES ({0}, '{1}', '{2}'); ",
                        vm.ClientID, vm.SiteName, mod_string
                    );

                SqlHelper.ExecuteNonQuery(sql);
            }
            
            //Add manual log entry if available
            if (vm.LogEntry != null)
            {
                sql = String.Format(
                    "INSERT INTO clientlogentry (ClientID, SiteName, Description) " +
                    "VALUES ({0}, '{1}', '{2}'); ",
                    vm.ClientID, vm.SiteName, vm.LogEntry
                );

                SqlHelper.ExecuteNonQuery(sql);
            }

            UpdateClientViewModel om = new UpdateClientViewModel();

            // Get log list with new entry to display
            sql = String.Format(
                    "SELECT DateTimeStamp, ServiceName, SiteName, Description " +
                       "FROM clientlogentry WHERE ClientID = {0} ", vm.ClientID);

            List<object[]> result = SqlHelper.ExecuteMultiSelect(sql, 4);
            if (result != null)
            {
                om.Logs = GetLogListFromQueryResponse(result);
            }

            
            if (vm.selectedBunk != null)
            {
                // If Check-in radio button is selected, decrement from bunklist
                sql = String.Format(
               "UPDATE shelter SET {0} = {0} - 1 " +
               "WHERE SiteID = {1}; ",
               vm.selectedBunk, vm.SiteID);

                SqlHelper.ExecuteNonQuery(sql);

                // Add logging for checkin-in
                string checkInLogEntry = "Checked into ";
                if (vm.selectedBunk == "MaleBunksAvailable")
                {
                    checkInLogEntry += "male bunk";

                }
                else if (vm.selectedBunk == "FemaleBunksAvailable")
                {
                    checkInLogEntry += "female bunk";
                }
                else
                {
                    checkInLogEntry += "mixed bunk";
                }

                sql = String.Format(
                    "INSERT INTO clientlogentry (ClientID, SiteName, Description) " +
                    "VALUES ({0}, '{1}', '{2}'); ",
                    vm.ClientID, vm.SiteName, checkInLogEntry
                );

                SqlHelper.ExecuteNonQuery(sql);
            }


            om.StatusMessage = "Succesfully updated!";
           
            return View(om);

        }

        
        public ActionResult AddManualLogIndex(SelectClientViewModel vm)
        {
            Log om = new Log();
            om.ClientID = vm.ClientID;
            return View(om);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToWaitlist(SelectClientViewModel vm)
        {
            // Get max rating for current site
            int? SiteID = Session["SiteID"] as int?;
            string queryResult = null;
            string sql = String.Format("SELECT MAX(Ranking) FROM waitlist WHERE SiteID = {0}; ", SiteID);
            queryResult = SqlHelper.ExecuteScalar(sql).ToString();
            int ranking = 1;
            if (queryResult != "")
            {
                ranking = int.Parse(SqlHelper.ExecuteScalar(sql).ToString())+1;
            } 
            // Place client on waitlist with next ranking. 
            sql = String.Format(
                        "INSERT INTO waitlist (ClientID, SiteID, Ranking) " +
                        "VALUES ({0},{1}, {2}); ",
                        vm.ClientID, SiteID, ranking
                    );

            SqlHelper.ExecuteNonQuery(sql);
            AddToWaitlistViewModel om = new AddToWaitlistViewModel();
            
            sql = String.Format(
                       "SELECT ClientID, SiteID, Ranking " +
                       "FROM waitlist WHERE ClientID = {0} ", vm.ClientID);

            List<object[]> result = SqlHelper.ExecuteMultiSelect(sql, 3);
            if (result != null)
            {
                om.Waitlist = GetWaitListFromQueryResponse(result);
            }
            om.StatusMessage = "Succesfully added to waitlist!";
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
                    DateTimeStamp = row[0].ToString(),
                    ServiceName = row[1].ToString(),
                    SiteName = row[2].ToString(),
                    Description = row[3].ToString(),
                });
            }

            return response;
        }

        private List<Waitlist> GetWaitListFromQueryResponse(List<object[]> queryResponse)
        {
            var response = new List<Waitlist>();

            foreach (object[] row in queryResponse)
            {
                response.Add(new Waitlist
                {
                    ClientID = int.Parse(row[0].ToString()),
                    SiteID = int.Parse(row[1].ToString()),
                    Ranking = int.Parse(row[2].ToString()),
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