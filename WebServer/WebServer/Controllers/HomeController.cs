using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebServer.Models;

namespace WebServer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //using (var db = ApplicationDbContext.Create())
            //{
            //    db.Routes.Add(new Route()
            //    {
            //        User = db.Users.First(),
            //        Name = "Cool Route",
            //        Polyline = DbGeometry.LineFromText("LINESTRING (60.170880 24.942795, 60.170879 24.942796, 60.170877 24.942796)", DbGeometry.DefaultCoordinateSystemId),
            //        Distance = .2224m
            //    });
            //    db.SaveChanges();
            //}

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}