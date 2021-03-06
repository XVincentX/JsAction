﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MVCControlsToolkit.DataAnnotations;

namespace Mvc_Examples.Models
{
    [MetadataType(typeof(MetaToDo))]
    public partial class ToDoViewBase
    {
        public int? id { get; set; }
       
        public string Name { get; set; }
       
        public string Description { set; get; }

        

        public DateTime DueDate {set;get;}

    }

    public partial class ToDoView: ToDoViewBase
    {
        public IEnumerable<SubTasksView> Tasks { get; set; }
    }
    public partial class ToDoViewDetail:ToDoViewBase
    {
        

        [DateRange(SMinimum = "Today-8M", SMaximum = "Today+6M")]
        new public DateTime DueDate { set { } get { return DateTime.Today; } }
        public ToDoView DetailOf { get; set; }
        public List<SubTasksView> Tasks { get; set; }
           
    }
}