using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using WebService.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebService.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {

        [Authorize]
        [Route("routes")]
        public IHttpActionResult Get()
        {
            var userId = User.Identity.GetUserId();

            using (var db = ApplicationDbContext.Create())
            {
                var routes = db.Routes.Where(x => x.UserId == userId);

                var arr = new JArray();

                foreach(var route in routes)
                {
                    arr.Add(new JObject(new JProperty("id", route.Id), new JProperty("name", route.Name), 
                        new JProperty("polyline", route.Polyline), new JProperty("distance", route.Distance)));
                }

                var obj = new JObject(new JProperty("routes", arr));

                return Ok(obj);
            }

            //return BadRequest();
        }

        [Authorize]
        [Route("routes/{id}")]
        public IHttpActionResult Get(int id)
        {
            var userId = User.Identity.GetUserId();

            using (var db = ApplicationDbContext.Create())
            {
                var route = db.Routes.Where(x => x.UserId == userId && x.Id == id).FirstOrDefault();

                if (route == null)
                {
                    return BadRequest("No route found");
                }
                else if (route != null && route.UserId != userId)
                {
                    return Unauthorized();
                }

                var obj = new JObject(new JProperty("id", route.Id), new JProperty("name", route.Name),
                    new JProperty("polyline", route.Polyline), new JProperty("distance", route.Distance));

                return Ok(obj);
            }
            //return BadRequest();
        }
    }
}
