using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebServer.Controllers
{
    public class AuthorizationController : Controller
    {
        [Route("unauthorized")]
        public ActionResult Unauthorized()
        {
            Response.StatusCode = 401;
            return View("Error", model:"Error: 401");
        }

        [Route("forbidden")]
        public ActionResult Forbidden()
        {
            Response.StatusCode = 403;
            return View("Error", model:"Error: 403");
        }
    }
}