using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestPackageProject.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        [JsAction.JsAction]
        public ActionResult Index()
        {
            return View();
        }

        [JsAction.JsAction]
        public JsonResult oas()
        {
            return Json(new { a = 3 });
        }

    }
}
