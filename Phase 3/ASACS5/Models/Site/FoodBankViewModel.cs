using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASACS5.Models.Site
{
	public class FoodBankViewModel
	{
		public int SiteID { get; set; }
		public string SiteName { get; set; }

		public string StatusType { get; set; }
		public string StatusMessage { get; set; }

        public bool FoodBankExists;

	}
}