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
        public static JSAScriptBlock JsScriptBlock(this HtmlHelper helper)
        {
            TagBuilder tagbuilder = new TagBuilder("script");
            tagbuilder.MergeAttribute("type", "text/javascript");
            tagbuilder.MergeAttribute("data-jsaction", "jsaction");

            helper.ViewContext.Writer.Write(tagbuilder.ToString(TagRenderMode.StartTag));
            return new JSAScriptBlock(helper.ViewContext);
        }


        public static void JsScriptForMethod<TController>(this HtmlHelper helper, Expression<Func<TController, object>> expression) where TController : Controller
        {
            JsScriptForMethod<TController>(helper, new Expression<Func<TController, object>>[] { expression });
        }
        public static void JsScriptForMethod<TController>(this HtmlHelper helper, params Expression<Func<TController, object>>[] expressions) where TController : Controller
        {

            foreach (var expression in expressions)
            {
                if (expression.Body is MethodCallExpression == false)
                    throw new ArgumentException("JsScript for method should be called only on controller methods!", "expression.Body", null);

                var method = (expression.Body as MethodCallExpression).Method;

                helper.ViewContext.Writer.Write("function gerbone(){}");

            }
        }
        public static void JsScriptForController<TController>(this HtmlHelper helper) where TController : Controller
        {
            JsScriptForController<TController>(helper, null);
        }
        public static void JsScriptForController<TController>(this HtmlHelper helper, params Expression<Func<TController, object>>[] Exceptions) where TController : Controller
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

