using ASACS5.Models.Logs;
using ASACS5.Models.Waitlists;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASACS5.Models.Site
{
    public class SelectClientViewModel
    {
        public int SiteID { get; set; }
        public string SiteName { get; set; }

        public string StatusType { get; set; }
        public string StatusMessage { get; set; }

        public int ClientID { get; set; }
        public int MaleBunks { get; set; }
        public int FemaleBunks { get; set; }
        public int MixedBunks { get; set; }
        public string selectedBunk { get; set; }
        public Boolean HasShelter { get; set; }
        public Boolean HasFoodPantry { get; set; }
        public Boolean HasSoupKitchen { get; set; }
        
        public List<Log> Logs { get; set; }
        public List<Waitlist> Waitlist { get; set; }
        public Boolean AddClientToWaitlistAllowed { get; set; }
        public string ShelterServiceName { get; set; }
        public string FoodPantryServiceName { get; set; }
        public string SoupKitchenServiceName { get; set; }
        public Boolean BunksAvailable { get; set; }
        

        [DisplayName("Descriptive ID")]
        [Required(ErrorMessage = "Descriptive ID is required")]
        [MaxLength(1000, ErrorMessage = "Max length is 250 characters")]
        public string DescriptiveID { get; set; }
        public string oldDescriptiveID { get; set; }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(1000, ErrorMessage = "Max length is 250 characters")]
        public string FirstName { get; set; }
        public string oldFirstName { get; set; }

        [DisplayName("Middle Name")]
        [MaxLength(1000, ErrorMessage = "Max length is 250 characters")]
        public string MiddleName { get; set; }
        public string oldMiddleName { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(1000, ErrorMessage = "Max length is 250 characters")]
        public string LastName { get; set; }
        public string oldLastName { get; set; }

        [DisplayName("Phone Number")]
        [MaxLength(10, ErrorMessage = "Max length is 80 characters")]
        public string PhoneNumber { get; set; }
        public string oldPhoneNumber { get; set; }

        [DisplayName("Log Entry")]
        [MaxLength(1000, ErrorMessage = "Max length is 250 characters")]
        public string LogEntry { get; set; }

        [DisplayName("Additional Notes for Bunk Check-In")]
        [MaxLength(1000, ErrorMessage = "Max length is 250 characters")]
        public string ShelterLogEntry { get; set; }

        [DisplayName("Additional Notes for Food Pantry Check-In")]
        [MaxLength(1000, ErrorMessage = "Max length is 250 characters")]
        public string FoodPantryLogEntry { get; set; }

        [DisplayName("Additional Notes for Soup Kitchen Check-In")]
        [MaxLength(1000, ErrorMessage = "Max length is 250 characters")]
        public string SoupKitchenLogEntry { get; set; }

        [DisplayName("Provide food from food pantry")]
        public Boolean FoodPantryCheckIn { get; set; }

        [DisplayName("Provide meal from soup kitchen")]
        public Boolean SoupKitchenCheckIn { get; set; }
    }
}