using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASACS5.Models.Requests
{
    public class MyRequestsViewModel
    {
        public int SiteID { get; set; }
        public List<Request> Requests { get; set; }

    }
}