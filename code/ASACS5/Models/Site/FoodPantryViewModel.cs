using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASACS5.Models.Site
{
	public class FoodPantryViewModel
	{
		public int SiteID { get; set; }
		public string SiteName { get; set; }

		public string StatusType { get; set; }
		public string StatusMessage { get; set; }

		[DisplayName("Hours of Operation")]
		[Required(ErrorMessage = "Hours of Operation is required")]
		[MaxLength(1000, ErrorMessage = "Max length is 1000 characters")]
		public string HoursOfOperation { get; set; }

		[DisplayName("Conditions For Use")]
		[MaxLength(1000, ErrorMessage = "Max length is 1000 characters")]
		public string ConditionsForUse { get; set; }

	}
}