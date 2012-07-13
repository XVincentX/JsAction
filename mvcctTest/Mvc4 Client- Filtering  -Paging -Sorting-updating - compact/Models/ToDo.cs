using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MVCControlsToolkit.DataAnnotations;

namespace Mvc_Examples.Models
{
    [MetadataType(typeof(MetaToDo))]
    public partial class ToDo
    {

    }
    public class MetaToDo
    {
        
        [Required, CanSort, Display(Name="Name")]
        public object Name { get; set; }
        [Required, Display(ShortName = "Description")]
        public object Description { get; set; }
        [CanSort, Display(ShortName = "Due Date"), Format(DataFormatString="{0:D}")]
        public object DueDate { get; set; }
    }
}