using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MVCControlsToolkit.DataAnnotations;

namespace Mvc_Examples.Models
{
    [MetadataType(typeof(MetaSubTasks))]
    public class SubTasksView
    {
        public string Name {get; set;}

        public int WorkingDays { get; set; }
    }
}