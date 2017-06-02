using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Configuration;

namespace WebService.Controllers
{
    using Microsoft.AspNet.Identity;
    using Models;

    [RoutePrefix("routes")]
    public class MapsController : Controller
    {
        [HttpGet]
        [Authorize]
        [Route("")]
        public ActionResult Routes()
        {
            using (var db = ApplicationDbContext.Create())
            {
                var model = db.Routes.Where(x => x.User.UserName == User.Identity.Name).ToList();
                //var model = db.Routes.Where(x => true == true).ToList();
                return View(model: model);
            }
        }


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
            ValidateRoute(route);

            if (ModelState.IsValid)
            {
                using (var db = ApplicationDbContext.Create())
                {
                    var user = db.Users.Single(x => x.UserName == User.Identity.Name);

                    route.User = user;
                    db.Routes.Add(route);

                    db.SaveChanges();
                }

                return RedirectToAction("Routes", "Maps");
            }

            return View(model: route);
        }

        [HttpGet]
        [Authorize]
        [Route("edit")]
        public ActionResult EditRoute(int id)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var route = db.Routes.SingleOrDefault(x => x.Id == id);
                route = route == null ? new Route() : route;
                return View(model: route);
            }
        }
        [HttpPost]
        [Authorize]
        [Route("edit")]
        public ActionResult EditRoute(Route route)
        {
            ValidateRoute(route);

            if (ModelState.IsValid)
            {
                using (var db = ApplicationDbContext.Create())
                {
                    var routeToUpdate = db.Routes.Include("User").SingleOrDefault(x => x.Id == route.Id);

                    if (routeToUpdate == null)
                    {
                        return View(model: routeToUpdate);
                    }

                    if (User.Identity.Name != routeToUpdate.User.UserName)
                    {
                        // TODO: error page
                        return View(model: routeToUpdate);
                    }

                    routeToUpdate = db.Routes.Attach(routeToUpdate);
                    var entry = db.Entry(routeToUpdate);

                    routeToUpdate.Name = route.Name;
                    routeToUpdate.Origin = route.Origin;
                    routeToUpdate.Destination = route.Destination;
                    routeToUpdate.IsLoopRoute = route.IsLoopRoute;
                    routeToUpdate.Distance = route.Distance;

                    entry.State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();
                }

                return RedirectToAction("Routes", "Maps");
            }

            return View(model: route);
        }

        [Authorize]
        [Route("delete")]
        public ActionResult DeleteRoute(int id)
        {
            using (var db = ApplicationDbContext.Create())
            {
                var route = db.Routes.Include("User").SingleOrDefault(x => x.Id == id);

                if (route != null)
                {
                    if (route.User.UserName == User.Identity.Name)
                    {
                        db.Routes.Remove(route);
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Routes");
        }


        private void ValidateRoute(Route route)
        {
            ModelState.Clear();
            {
                var latLng = route.Origin.Split(',');

                if (latLng.Length != 2)
                {
                    ModelState.AddModelError("Origin", "Not a valid Latitude Longitude");
                }
                else
                {
                    decimal parse = 0;
                    if (!decimal.TryParse(latLng[0], out parse))
                    {
                        ModelState.AddModelError("Origin", "Not a valid Latitude Longitude");
                    }
                    else
                    {
                        if (!decimal.TryParse(latLng[1], out parse))
                        {
                            ModelState.AddModelError("Origin", "Not a valid Latitude Longitude");
                        }
                    }
                }

            }
            {
                var latLng = route.Destination.Split(',');

                if (latLng.Length != 2)
                {
                    ModelState.AddModelError("Origin", "Not a valid Latitude Longitude");
                }
                else
                {
                    decimal parse = 0;
                    if (!decimal.TryParse(latLng[0], out parse))
                    {
                        ModelState.AddModelError("Origin", "Not a valid Latitude Longitude");
                    }
                    else
                    {
                        if (!decimal.TryParse(latLng[1], out parse))
                        {
                            ModelState.AddModelError("Origin", "Not a valid Latitude Longitude");
                        }
                    }
                }

            }

        }
    }
}