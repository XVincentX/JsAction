using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Mvc_Examples.Models;
using MVCControlsToolkit.Controller;
using MVCControlsToolkit.Linq;
using System.Transactions;


namespace Mvc_Examples.Controllers
{
    public class ToDoController : ApiController
    {
        [Queryable]
        [JsAction.mvcct.JsqSelectMethod]
        public IQueryable<ToDoView> Get()
        {
            return new HttpSafeQuery<ToDoView>(this.Request, ToDoViewModel.GetToDoQueryable(), true);
        }

        // GET /api/todo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST /api/todo
        [JsAction.mvcct.JsqUpdateMethod]
        public HttpResponseMessage Post(UpdataViewModel model)
        {
            //uncomment to experiment server side error handling
            //ModelState.AddModelError("ToDoCS.Modified[0].Name", "Fake error");
            //ModelState.AddModelError("TaskCS.Modified[0].Name", "Fake error1");
            int[] ToDoKeys = new int[0];
            int[] TaskKeys = new int[0]; ;
            if (ModelState.IsValid)
            {
                try
                {
                    using (var t = new TransactionScope())
                    {
                        if (model != null && model.ToDoCS != null)
                        {
                            ToDoKeys = ToDoViewModel.UpdatePage(model.ToDoCS.Inserted, model.ToDoCS.Modified, model.ToDoCS.Deleted);

                            //imports the external keys of the newly created father entities into their children
                            if (model.TaskCS != null) model.TaskCS.ImportExternals(ToDoKeys, m => m.FatherId);
                            //here the same for other children collections
                        }
                        if (model != null && model.TaskCS != null)
                        {
                            TaskKeys = ToDoViewModel.UpdatePageTasks(model.TaskCS.Inserted, model.TaskCS.Modified, model.TaskCS.Deleted);
                        }
                        t.Complete();
                    }

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return
                        Request.CreateResponse(System.Net.HttpStatusCode.InternalServerError,
                            new ApiServerErrors<int>(ModelState, new ApiKeyInfos<int>[0]));
                }

            }

            // if keys have different simple types such as one is int, and one other is string, use ApiServerErrors<object>
            return new ApiServerErrors<int>(ModelState,
                new ApiKeyInfos<int>[] {
                    new ApiKeyInfos<int>{destinationExpression="ToDoCS", keys=ToDoKeys},
                    new ApiKeyInfos<int>{destinationExpression="TaskCS", keys=TaskKeys}
                }).Wrap(this.Request);
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
