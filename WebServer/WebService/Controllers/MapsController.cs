using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;

namespace WebService.Controllers
{
    using Models;

    [RoutePrefix("map")]
    public class MapsController : Controller
    {
        [HttpGet]
        [Authorize]
        [Route("new")]
        public ActionResult NewRoute()
        {
            return View(model: new Route());
        }

        [HttpPost]
        [Authorize]
        [Route("new")]
        public ActionResult NewRoute(Route route)
        {
            return View(model: route);
        }
    }
}