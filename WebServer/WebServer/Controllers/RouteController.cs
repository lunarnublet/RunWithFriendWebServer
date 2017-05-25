using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebServer.Models;
using System.Data.Entity.Spatial;
using WebServer.Attributes;


namespace WebServer.Controllers
{
    [RoutePrefix("api")]
    public class RouteController : Controller
    {
        [Route("user")]
        [APIAuthorize]
        //[Authorize]
        public ActionResult GetUserRoutes(string userId)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var userID = User.Identity.GetUserId();

                if (userId != null && userID != userId)
                {
                    return RedirectToRoute(new
                    {
                        controller = "Authorization",
                        action = "Forbidden"
                    });
                }

                var routes = db.Routes.Where(x => x.UserId == userID);
                var arr = new JArray();

                foreach (var route in routes)
                {
                    arr.Add(new JObject(
                        new JProperty("id", route.Id),
                        new JProperty("name", route.Name),
                        new JProperty("polyline", ""),
                        new JProperty("distance", route.Distance)));
                }

                var jobject = new JObject(new JProperty("routes", arr));

                //var jobject = new JObject(new JProperty("routes",
                //    new JArray(from r in routes
                //               select new JObject(
                //                new JProperty("id", r.Id),
                //                new JProperty("name", r.Name),
                //                new JProperty("polyline", ""),
                //                new JProperty("distance", r.Distance)))));

                string json = jobject.ToString();

                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.Write(json);
            }

            return Content("");
        }
    }
}