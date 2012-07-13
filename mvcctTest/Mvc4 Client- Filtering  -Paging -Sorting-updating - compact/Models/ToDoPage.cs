using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_Examples.Models
{
    public class ToDoPage
    {
        public int TotalPages { get; set; }
        public int CurrPage { get; set; }
        public int PrevPage { get; set; }
        public List<ToDoView> ToDoList { get; set; }
        public List<int> Deleted { get; set; }
    }
}