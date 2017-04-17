using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASACS5.Models.Requests
{
    public class RequestsIndexViewModel
    {
        public int SiteID { get; set; }
        public string SiteName { get; set; }

        public string SortByChoice { get; set; }
        public string SortByOrder { get; set; }

        public IEnumerable<SelectListItem> SortByChoiceOptions { get; set; }
        public IEnumerable<SelectListItem> SortByOrderOptions { get; set; }

        public List<Request> Requests { get; set; }

    }
}