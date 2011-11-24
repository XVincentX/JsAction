using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics;

namespace JsAction
{
    public class JsActionHandler : IHttpHandler, IRouteHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var js = new StringBuilder();
            js.Append("var JsActions = {");

            var methods = this.GetMethodsWith<JsActionAttribute>(false);
            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttributes(typeof(JsActionAttribute), false).First() as JsActionAttribute;

                js.AppendFormat("{0}:function(", string.IsNullOrEmpty(attribute.MethodName) ? method.Name : attribute.MethodName);

                foreach (var parameter in method.GetParameters())
                {
                    js.AppendFormat("{0},", parameter.Name);
                }

                js.Append("options)");

                GenerateMethodCall(js, method, attribute);
            }

            js.Append('}');

            context.Response.ContentType = "application/javascript";
            context.Response.Charset = string.Empty;
            context.Response.Write(js.ToString());
            context.Response.End();

        }

        internal void GenerateMethodCall(StringBuilder js, MethodInfo method, JsActionAttribute attribute)
        {

            const string GET = "GET";
            const string POST = "POST";

            var attributes = method.GetCustomAttributes(true);
            var ActionAttr = attributes.Where(attr => attr is ActionNameAttribute);

            string Action = (ActionAttr.Count() != 0) ? (ActionAttr.First() as ActionNameAttribute).Name : method.Name;
            string Controller = method.DeclaringType.Name.Substring(0, method.DeclaringType.Name.IndexOf("Controller"));

            string url = UrlHelper.GenerateUrl("Default", Action, Controller, null, RouteTable.Routes, this.requestContext, true);


            string requestmethod = attribute.Verbs == null ? attributes.Count(attr => attr is HttpPostAttribute) != 0 ? POST : GET : attribute.Verbs.Value == HttpSingleVerb.HttpGet ? GET : POST;
            StringBuilder jsondata = new StringBuilder();

            foreach (var parameter in method.GetParameters())
            {
                jsondata.AppendFormat("{0}:{0},", parameter.Name);
            }
            if (jsondata.Length > 0)
                jsondata.Remove(jsondata.Length - 1, 1);

            var AcceptVerbs = attributes.Where(attr => attr is AcceptVerbsAttribute);
            if (AcceptVerbs.Count() != 0)
            {
                var verbs = AcceptVerbs.First() as AcceptVerbsAttribute;
                if (verbs.Verbs.Count() > 1)
                {
                    if (Debugger.IsAttached)
                    {
                        Debug.WriteLine("JsAction -- 2 accept verbs. I will suppose you prefer GET");
                        requestmethod = GET;
                    }
                    else
                        throw new Exception("2 accept verbs. Don't know which one to choose!");
                }
            }
            js.AppendFormat("{{var opts = {{url:\"{0}\",method: \"{1}\",data:{{{2}}}}};", url, requestmethod, jsondata);
            js.Append("$.extend(opts,options); return $.ajax(opts);}");
        }




        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            this.requestContext = requestContext;
            return this;
        }


        internal IEnumerable<MethodInfo> GetMethodsWith<TAttribute>(bool inherit) where TAttribute : System.Attribute
        {
            return from a in AppDomain.CurrentDomain.GetAssemblies()
                   from t in a.GetTypes()
                   from m in t.GetMethods()
                   where m.IsDefined(typeof(TAttribute), inherit)
                   select m;
        }
        private RequestContext requestContext;

    }
}
