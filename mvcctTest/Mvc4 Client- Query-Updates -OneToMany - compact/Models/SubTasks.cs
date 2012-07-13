using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MVCControlsToolkit.DataAnnotations;

namespace Mvc_Examples.Models
{
    [MetadataType(typeof(MetaSubTasks))]
    public partial class SubTasks
    {
    }

    public class MetaSubTasks
    {
        [Required]
        [Display(Prompt = "Sub-task name")]
        public object Name {get; set;}
        [Range(0, 90, ErrorMessage="Acceptable Working Days Range from 0 to 90")]
        [Display(Prompt="N° of days")]
        [Format(Postfix=" days", ClientFormat="n0")]
        public object WorkingDays { get; set; }
    }
}