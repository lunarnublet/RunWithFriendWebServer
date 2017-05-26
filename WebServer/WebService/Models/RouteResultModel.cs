using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models
{
    public class RouteModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string polyline { get; set; }
        public decimal distance { get; set; }
    }
}