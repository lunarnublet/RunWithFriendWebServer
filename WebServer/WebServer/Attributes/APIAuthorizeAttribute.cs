using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebServer.Attributes
{
    public class APIAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                HttpContext.Current.Response.StatusCode = 403;
                filterContext.HttpContext.Response.StatusCode = 403;
            }
            else
            {
                HttpContext.Current.Response.StatusCode = 401;
                filterContext.HttpContext.Response.StatusCode = 401;
            }
        }
    }
}