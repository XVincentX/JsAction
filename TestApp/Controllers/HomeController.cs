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
        public int GetInt() { return 1;}

        [JsAction(Async = false), HttpGet]
        public float GetFloat() { return 3.3f; }

        [JsAction(Async = false), HttpGet]
        public string GetString() { return "This is a test string"; }

        [JsAction(Async = false), HttpPost]
        public JsonResult JsonObject() { return Json(new { theint = 3, thefloat = 3.3f, thestring = "This is a string" }); }

        [JsAction(Async = false), HttpPost]
        public JsonResult ComplexObjectSendAndReturn(Complex obj) { return Json(obj); }

        [JsAction(Async = false), HttpGet]
        public ActionResult JsCode()
        {
            return JavaScript("ok(true,'Javascript code from server side');");
        }

        [JsAction(Async = false, CacheRequest = true), HttpGet]
        public long CachedTicks()
        {
            return DateTime.Now.Ticks;
        }

        [JsAction(Async = false, CacheRequest = false), HttpGet]
        public long NonChachedTicks()
        {
            return DateTime.Now.Ticks;
        }

        [JsAction(Async = false, MethodName = "smpl10"), HttpGet]
        public string ogjipogjheioufhiuohadsiufhodsiughughidfshgodfsuoghdiuoh()
        {
            return System.Reflection.MethodInfo.GetCurrentMethod().Name;
        }

        [JsAction(Async = true), HttpGet]
        public string AsyncMethod()
        {
            return "I runned using async jQuery feature";
        }

        [JsAction(Async = false), HttpGet, NonAction]
        public string NotMappedMethod()
        {
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listComplex"></param>
        /// <returns></returns>
        [JsAction(Async = false), HttpGet]
        public JsonResult ListOfComplex(List<Complex> listComplex)
        {
            return Json(listComplex, JsonRequestBehavior.AllowGet);
        }

        [JsAction(Async = false), HttpGet]
        public JsonResult DateTimeObj(DateTime dt)
        {
            return Json(new { i = 3, dt = DateTime.Now, wz = new { i = 7, dt = DateTime.Now.AddYears(10) } }, JsonRequestBehavior.AllowGet);
        }
    }
}
