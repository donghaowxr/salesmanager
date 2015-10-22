using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace salesmanager.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (Session["username"] == null)
            {
                //return Redirect("/Login/Index");

                filterContext.HttpContext.Response.Redirect("/UserLogin/UserLogin");
            }
        }

    }
}
