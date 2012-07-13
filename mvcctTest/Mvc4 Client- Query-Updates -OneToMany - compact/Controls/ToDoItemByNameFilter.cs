using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc_Examples.Models;
using MVCControlsToolkit.Controls;
using MVCControlsToolkit.DataAnnotations;
using MVCControlsToolkit.Controls.DataFilter;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;

namespace Mvc_Examples.Controls
{
    public class ToDoItemByNameFilter: 
        IFilterDescription<ToDoView>
    {
        [Required]
        [Display(Prompt="chars the name of item starts with")]
        public string Name {get; set;}
        public System.Linq.Expressions.Expression<Func<ToDoView, bool>> GetExpression()
        {
            System.Linq.Expressions.Expression<Func<ToDoView, bool>> res = null;
            Name=Name.Trim();
            if (!string.IsNullOrEmpty(Name))
            {
                Name=Name.Trim();
                res= m => (m.Name.StartsWith(Name));
                
            }
            return res;
        }
    }
}