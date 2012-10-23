using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Web.Mvc
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// Writes a script block in the current view using IDisposable pattern
        /// </summary>
        /// <param name="helper"></param>
        /// <returns>JSAScriptBlock to dispose in order to close script block</returns>
        public static JSAScriptBlock JsScriptBlock(this HtmlHelper helper)
        {
            TagBuilder tagbuilder = new TagBuilder("script");
            tagbuilder.MergeAttribute("type", "text/javascript");
            tagbuilder.MergeAttribute("data-jsaction", "jsaction");

            helper.ViewContext.Writer.Write(tagbuilder.ToString(TagRenderMode.StartTag));
            helper.ViewContext.Writer.Write(@"if (typeof JsActions=='undefined'){var JsActions={};JsActions.WebApi={};}");
            return new JSAScriptBlock(helper.ViewContext);
        }


        /// <summary>
        /// Creates a JSAction Ajax call for specified controller method.
        /// </summary>
        /// <typeparam name="TController">Controller with methods to create.</typeparam>
        /// <param name="helper">Current html helper</param>
        /// <param name="expressions">Lambda expression for controller creation.</param>
        public static void JsScriptForMethod<TController>(this HtmlHelper helper, params Expression<Func<TController, object>>[] expressions) where TController : Controller
        {
            
            StringBuilder sb = new StringBuilder(150);
            string ControllerName = typeof(TController).Name.Substring(0, typeof(TController).Name.IndexOf("Controller"));
            sb.AppendFormat(@"if (typeof JsActions.{0}=='undefined'){{JsActions.{0}={{}}}}", ControllerName);

            foreach (var expression in expressions)
            {
                if (expression.Body is MethodCallExpression == false)
                    throw new ArgumentException("JsScript for method should be called only on controller methods!", "expression.Body", null);

                var method = (expression.Body as MethodCallExpression).Method;

                sb.AppendFormat("if(JsActions.{0}.{1}=='undefined'){{JsActions.{0}.{1}=function(){{}}}}", ControllerName, method.Name);

                helper.ViewContext.Writer.Write(sb.ToString());

            }
        }
        public static void JsScriptForController<TController>(this HtmlHelper helper) where TController : Controller
        {
        }
    }

    public class JSAScriptBlock : IDisposable
    {

        private readonly ViewContext _viewContext;
        private bool _disposed;

        public JSAScriptBlock(ViewContext context)
        {
            this._viewContext = context;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                _viewContext.Writer.Write("</script>");
            }
        }

        public void EndScriptBlock()
        {
            Dispose(true);
        }

    }
}

