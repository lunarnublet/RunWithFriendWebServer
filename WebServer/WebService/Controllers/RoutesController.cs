using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebService.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebService.Controllers
{
    [Authorize]
    [RoutePrefix("api")]
    public class RoutesController : ApiController
    {
        [HttpGet]
        [Route("routes")]
        public IHttpActionResult Get()
        {
            var userId = User.Identity.GetUserId();

            using (var db = ApplicationDbContext.Create())
            {
                var routes = db.Routes.Where(x => x.UserId == userId);

                var arr = new JArray();

                foreach (var route in routes)
                {
                    arr.Add(new JObject(new JProperty("id", route.Id), new JProperty("name", route.Name),
                        new JProperty("origin", route.Origin), new JProperty("destination", route.Destination),
                        new JProperty("distance", route.Distance), new JProperty("is_loop_route", route.IsLoopRoute)));
                }

                var obj = new JObject(new JProperty("routes", arr));

                return Ok(obj);
            }
        }

        [HttpGet]
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
                    new JProperty("origin", route.Origin), new JProperty("destination", route.Destination),
                        new JProperty("distance", route.Distance), new JProperty("is_loop_route", route.IsLoopRoute));

                return Ok(obj);
            }
            //return BadRequest();
        }

        [Route("routes")]
        [HttpPost]
        public IHttpActionResult Post([FromBody] RouteBinding routeBinding)
        {
            if (routeBinding == null)
            {
                return BadRequest("No route specified");
            }

            var userId = User.Identity.GetUserId();

            using (var db = ApplicationDbContext.Create())
            {
                var user = db.Users.Single(x => x.Id == userId);

                if (user.Id != userId)
                {
                    return BadRequest("Invalid user");
                }

                var route = new Route()
                {
                    Name = routeBinding.name,
                    Origin = routeBinding.origin,
                    Destination = routeBinding.destination,
                    Distance = routeBinding.distance,
                    IsLoopRoute = routeBinding.is_loop_route,
                    User = user
                };

                db.Routes.Add(route);

                db.SaveChanges();
                return Created("Routes", new RouteResultModel()
                {
                    id = route.Id,
                    name = route.Name,
                    origin = route.Origin,
                    destination = route.Destination,
                    is_loop_route = route.IsLoopRoute,
                    distance = route.Distance
                });
            }
        }

        [Route("routes/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var userId = User.Identity.GetUserId();

            using (var db = ApplicationDbContext.Create())
            {
                var user = db.Users.Single(x => x.Id == userId);

                if (userId != user.Id)
                {
                    return BadRequest("Invalid user");
                }

                var routeToDelete = db.Routes.SingleOrDefault(x => x.Id == id);

                if (routeToDelete == null)
                {
                    return BadRequest("Route does not exist");
                }

                db.Routes.Remove(routeToDelete);

                db.SaveChanges();
                return Ok("Route deleted");
            }
        }

        [Route("routes/{id}")]
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody] RouteBinding routeBinding)
        {
            if (routeBinding == null)
            {
                return BadRequest("No route specified");
            }

            var userId = User.Identity.GetUserId();

            using (var db = ApplicationDbContext.Create())
            {
                var user = db.Users.Single(x => x.Id == userId);

                if (userId != user.Id)
                {
                    return BadRequest("Invalid user");
                }

                var routeToUpdate = db.Routes.SingleOrDefault(x => x.Id == id);

                if (routeToUpdate == null)
                {
                    return BadRequest("Route does not exist");
                }

                var route = db.Routes.Attach(routeToUpdate);
                var entry = db.Entry(routeToUpdate);

                route.Name = routeBinding.name;
                route.Origin = routeBinding.origin;
                route.Destination = routeBinding.destination;
                route.IsLoopRoute = routeBinding.is_loop_route;
                route.Distance = routeBinding.distance;

                entry.State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();
                return Ok("Route updated");
            }
        }
    }

}
