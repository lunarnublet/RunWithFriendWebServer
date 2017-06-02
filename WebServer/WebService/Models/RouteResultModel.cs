using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebService.Models
{
    public class RouteResultModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string origin { get; set; }
        public string destination { get; set; }
        public bool is_loop_route { get; set; }
        public decimal distance { get; set; }
    }
}