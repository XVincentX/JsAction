using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Mvc_Examples.Models;
using MVCControlsToolkit.Controller;
using MVCControlsToolkit.Linq;

namespace Mvc_Examples.Controllers
{
    public class ToDoController : ApiController
    {
        // GET /api/todo
        [Queryable]
        [JsAction.mvcct.JsqSelectMethod]
        public IQueryable<ToDoView> Get()
        {
            return new HttpSafeQuery<ToDoView>(this.Request, ToDoViewModel.GetToDoQueryable());
        }

        // GET /api/todo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST /api/todo
        public HttpResponseMessage Post(Updater<ToDoView, int> model)
        {
            //uncomment to experiment server side error handling
            //ModelState.AddModelError("Modified[0].Name", "Fake error");
            int[] insertedKeys;
            if (ModelState.IsValid)
            {
                insertedKeys = ToDoViewModel.UpdatePage(model.Inserted, model.Modified, model.Deleted);

            }
            else
            {
                insertedKeys = new int[0];
            }
            return (new ApiServerErrors<int>(ModelState, insertedKeys)).Wrap(this.Request);
        }

        // PUT /api/todo/5
        public void Put(int id, string value)
        {
        }

        // DELETE /api/todo/5
        public void Delete(int id)
        {
        }
    }
}
