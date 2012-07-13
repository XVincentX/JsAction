using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc_Examples.Models;
using MVCControlsToolkit.Controls;
using MVCControlsToolkit.DataAnnotations;
using MVCControlsToolkit.Controls.DataFilter;
using MVCControlsToolkit.Linq;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;

namespace Mvc_Examples.Controls
{

    public class ToDoGeneralFilter :
        IFilterDescription<ToDoView>
    { 
        [Display(Prompt = "chars the name of item starts with")]
        public string Name { get; set; }
        public bool NameEnabled { get; set; }
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }
        public bool FromDateEnabled { get; set; }
        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }
        public bool ToDateEnabled { get; set; }
        public FilterCondition NameContition { get; set; }
        public ToDoGeneralFilter()
        {
            NameContition=FilterCondition.StartsWith;
        }
        public System.Linq.Expressions.Expression<Func<ToDoView, bool>> GetExpression()
        {
            System.Linq.Expressions.Expression<Func<ToDoView, bool>> res = new FilterBuilder<ToDoView>(FilterLogicalOperator.And)
                .Add(NameEnabled && (!string.IsNullOrWhiteSpace(Name)), NameContition, m => m.Name, Name)
                .Add(FromDateEnabled, m => m.DueDate >= FromDate)
                .Add(ToDateEnabled, m => m.DueDate <= ToDate)
                .Get();
            return res;
        }
    }
    public class ToDoTwoDateRangesFilter :
        IFilterDescription<ToDoView>
    {
        [Display(Prompt = "chars the name of item starts with")]
        public string Name { get; set; }
        public bool NameEnabled { get; set; }
        public FilterCondition NameContition { get; set; }
        [Display(Name = "From Date 1")]
        public DateTime FromDate1 { get; set; }
        public bool FromDate1Enabled { get; set; }
        [Display(Name = "To Date 1")]
        public DateTime ToDate1 { get; set; }
        public bool ToDate1Enabled { get; set; }

        [Display(Name = "From Date 2")]
        public DateTime FromDate2 { get; set; }
        public bool FromDate2Enabled { get; set; }
        [Display(Name = "To Date 2")]
        public DateTime ToDate2 { get; set; }
        public bool ToDate2Enabled { get; set; }

        public ToDoTwoDateRangesFilter()
        {
            NameContition = FilterCondition.StartsWith;
        }
        public System.Linq.Expressions.Expression<Func<ToDoView, bool>> GetExpression()
        {
            System.Linq.Expressions.Expression<Func<ToDoView, bool>> res = 
                new FilterBuilder<ToDoView>(FilterLogicalOperator.And)
                .Add(NameEnabled && (!string.IsNullOrWhiteSpace(Name)), NameContition, m => m.Name, Name)
                .Open(true, FilterLogicalOperator.Or)
                    .Open(true, FilterLogicalOperator.And)
                        .Add(FromDate1Enabled, m => m.DueDate >= FromDate1)
                        .Add(ToDate1Enabled, m => m.DueDate <= ToDate1)
                    .Close(true)
                    .Open(true, FilterLogicalOperator.And)
                        .Add(FromDate2Enabled, m => m.DueDate >= FromDate2)
                        .Add(ToDate2Enabled, m => m.DueDate <= ToDate2)
                    .Close(true)
                .Close(true)
                .Get();
            return res;
        }
    }
    public class ToDoByDateFilter :
        IFilterDescription<ToDoView>
    {
        
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }
        public bool FromDateEnabled { get; set; }
        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }
        public bool ToDateEnabled { get; set; }
        public System.Linq.Expressions.Expression<Func<ToDoView, bool>> GetExpression()
        {
            System.Linq.Expressions.Expression<Func<ToDoView, bool>> res = new FilterBuilder<ToDoView>()
                .Add(FromDateEnabled, m => m.DueDate >= FromDate)
                .Add(ToDateEnabled, m => m.DueDate <= ToDate)
                .Get();
            return res;
        }
    }
}