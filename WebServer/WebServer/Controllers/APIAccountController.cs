using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;

namespace WebServer.Controllers
{
    [RoutePrefix("api")]
    public class APIAccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public APIAccountController()
        {
        }

        public APIAccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        [Route("register")]
        public ActionResult Register(string userName, string password)
        {
            if (userName == null || password == null)
            {
                Response.StatusCode = 400;
                return Content("nope");
            }

            return Content("yep");
        }

        [Route("login")]
        public async Task<ActionResult> Login(string userName, string password)
        {
            var result = await SignInManager.PasswordSignInAsync(userName, password, true, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    Response.StatusCode = 200;
                    break;
                case SignInStatus.LockedOut:
                    return View("Lockout");
                //case SignInStatus.RequiresVerification:
                //    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                    Response.StatusCode = 500;
                    break;
                default:
                    Response.StatusCode = 400;
                    break;
            }

            return Content("");
        }
    }
}