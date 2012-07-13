using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCControlsToolkit.Controller;

namespace Mvc_Examples.Models
{
    public class UpdataViewModel
    {
        public Updater<ToDoView, Int32> ToDoCS { get; set; }
        public ChildUpdater<SubTasksView, Int32> TaskCS { get; set; }
    }
}