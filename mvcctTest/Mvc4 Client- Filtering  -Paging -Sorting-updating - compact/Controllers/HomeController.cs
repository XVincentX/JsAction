using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Mvc_Examples.Models;
using MVCControlsToolkit.Controller;
using MVCControlsToolkit.Linq;

namespace Mvc_Examples.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public const int PageDim=5;//in actual application this should be put in a config file
        public ActionResult Index()
        {
            //define default ordering. It is necessary for paging.
            List<KeyValuePair<LambdaExpression, OrderType>> order = new List<KeyValuePair<LambdaExpression,OrderType>>();
            Expression<Func<ToDoView, DateTime>> defaultOrder = m => m.DueDate;
            order.Add(new KeyValuePair<LambdaExpression, OrderType>(defaultOrder, OrderType.Descending));

            ToDoViewModel result = new ToDoViewModel()
                {
                    DataPage = new ToDoPage {
                        ToDoList=new List<ToDoView>()},
                    ToDoOrder=order

                };
            return this.ClientBlockView(result, "ClientToDoView");
        }

        

        public ActionResult IndexEdit()
        {
            //define default ordering. It is necessary for paging.
            List<KeyValuePair<LambdaExpression, OrderType>> order = new List<KeyValuePair<LambdaExpression, OrderType>>();
            Expression<Func<ToDoView, DateTime>> defaultOrder = m => m.DueDate;
            order.Add(new KeyValuePair<LambdaExpression, OrderType>(defaultOrder, OrderType.Descending));

            ToDoViewModel result = new ToDoViewModel()
            {
                DataPage = new ToDoPage
                {
                    ToDoList = new List<ToDoView>()
                },
                ToDoOrder = order

            };
            return this.ClientBlockView(result, "ClientToDoView", "root");
        }

        
        

        public ActionResult About()
        {
            return View();
        }
    }
}
