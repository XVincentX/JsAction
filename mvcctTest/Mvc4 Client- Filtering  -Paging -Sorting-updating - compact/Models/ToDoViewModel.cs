using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using MVCControlsToolkit.Linq;
using MVCControlsToolkit.Controller;
using System.Web.Script.Serialization;

namespace Mvc_Examples.Models
{
    public class ToDoViewModel
    {
        
        [ScriptIgnore]
        public List<KeyValuePair<LambdaExpression, OrderType>> ToDoOrder { get; set; }
        [ScriptIgnore]
        public System.Linq.Expressions.Expression<Func<ToDoView, bool>> ToDoFilter { get; set; }
        public ToDoPage DataPage { get; set; }
        public static IQueryable<ToDoView> GetToDoQueryable()
        {
            SiteDbEntities context = new SiteDbEntities();
            return context.ToDo.Select(item =>
                        new ToDoView() { Name = item.Name, Description = item.Description, DueDate = item.DueDate, id = item.id });

        }
        public static List<ToDoView> GetToDoPage(int pageDim, out int totalPages, ref List<KeyValuePair<LambdaExpression, OrderType>> order, int page = 1,
            System.Linq.Expressions.Expression<Func<ToDoView, bool>> filter=null)
        {
            List<ToDoView> result;
            if (order == null) 
            {
                order = new List<KeyValuePair<LambdaExpression, OrderType>>();
               
            }
            if (order.Count == 0)//paging require ordering! Therefore we always need to add a default oredering
            {
                Expression<Func<ToDoView, DateTime>> defaultOrder = m => m.DueDate;

                order.Add(new KeyValuePair<LambdaExpression, OrderType>(defaultOrder, OrderType.Descending));
            }
            using (SiteDbEntities context = new SiteDbEntities())
            {
                int rowCount;
                if (filter == null) rowCount = context.ToDo.Count();
                else rowCount = (from x in context.ToDo select new ToDoView(){Name = x.Name, Description = x.Description, DueDate = x.DueDate, id = x.id}).Where(filter).Count();
                if (rowCount == 0)
                {
                    totalPages=0;
                    return new List<ToDoView>();
                }
                totalPages = rowCount / pageDim;
                if (rowCount % pageDim > 0) totalPages++;
                if (page > totalPages) page = totalPages;
                if (page < 1) page = 1;
                int toSkip = (page-1) * pageDim;

                if (filter == null)
                {
                    result = context.ToDo.Select(item =>
                        new ToDoView() { Name = item.Name, Description = item.Description, DueDate = item.DueDate, id = item.id }).ApplyOrder(order)
                        .Skip(toSkip).Take(pageDim).ToList();
                }
                else
                {
                    result = context.ToDo.Select(item =>
                        new ToDoView() { Name = item.Name, Description = item.Description, DueDate = item.DueDate, id = item.id }).Where(filter).ApplyOrder(order)
                        .Skip(toSkip).Take(pageDim).ToList();
                }
                
            }
            return result;
        }
        public static int[] UpdatePage(List<ToDoView> itemsI, List<ToDoView> itemsM, List<int> toDelete)
        {
            if (itemsM == null && itemsI == null && toDelete == null) return new int[0];
            List<ToDo> inserted = new List<ToDo>();
            using (SiteDbEntities context = new SiteDbEntities())
            {
                bool aChange = false;
                if (itemsI != null)
                {
                    foreach (ToDoView item in itemsI)
                    {
                        ToDo curr = new ToDo() { Name = item.Name, Description = item.Description, DueDate = item.DueDate };
                        aChange = true;
                        context.ToDo.AddObject(curr);
                        inserted.Add(curr);
                    }
                }
                if (itemsM != null)
                {
                    foreach (ToDoView item in itemsM)
                    {
                        ToDo curr = new ToDo() { Name = item.Name, Description = item.Description, DueDate = item.DueDate, id = item.id.Value };
                        context.ToDo.Attach(curr);
                        context.ObjectStateManager.ChangeObjectState(curr, System.Data.EntityState.Modified);
                        aChange = true;
                    }
                }
                if (toDelete != null)
                {
                    foreach (int key in toDelete)
                    {
                        ToDo curr = new ToDo() { id = key };
                        context.ToDo.Attach(curr);
                        context.ObjectStateManager.ChangeObjectState(curr, System.Data.EntityState.Deleted);
                        aChange = true;
                    }
                }
                if (aChange)
                {
                    try
                    {
                        context.SaveChanges();

                    }
                    catch
                    {
                    }
                    return inserted.Select(m => m.id).ToArray();
                }
                else return new int[0];
            }
        }
    
    }
    
}