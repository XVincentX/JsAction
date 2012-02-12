using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JsAction;

namespace TestApp.Controllers
{

    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }

        [JsAction(Async = false), HttpGet]
        public int GetInt() { return 1; }
        [JsAction(Async = false), HttpGet]
        public float GetFloat() { return 3.3f; }
        [JsAction(Async = false), HttpGet]
        public string GetString() { return "This is a test string"; }
        [JsAction(Async = false), HttpGet]
        public JsonResult JsonObject() { return Json(new { theint = 0, thefloat = 3.3f, thestring = "This is a string" }); }

    }
}
