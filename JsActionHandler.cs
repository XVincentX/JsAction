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
    /// <summary>
    /// Http and Route handler for javascript file generation
    /// </summary>
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

        internal void GenerateMethodCall(StringBuilder js, MethodInfo method, JsActionAttribute jsattribute)
        {

            const string GET = "GET";
            const string POST = "POST";

            var attributes = method.GetCustomAttributes(true);
            var ActionAttr = attributes.Where(attr => attr is ActionNameAttribute);

            string Action = (ActionAttr.Count() != 0) ? (ActionAttr.First() as ActionNameAttribute).Name : method.Name;
            string Controller = method.DeclaringType.Name.Substring(0, method.DeclaringType.Name.IndexOf("Controller"));

            string url = UrlHelper.GenerateUrl("Default", Action, Controller, null, RouteTable.Routes, this.requestContext, true);


            bool post = false;
            bool get = false;

            if (jsattribute.Verb != HttpSingleVerb.None)
            {
                if (jsattribute.Verb == HttpSingleVerb.HttpPost)
                    post = true;
                else
                    get = true;
            }
            else
            {
                if (attributes.Count(attr => attr is AcceptVerbsAttribute) != 0)
                {
                    var verbattr = attributes.Where(attr => attr is AcceptVerbsAttribute).First();

                    foreach (var verb in (verbattr as AcceptVerbsAttribute).Verbs)
                    {
                        if (verb.ToUpper() == GET)
                            get = true;
                        else if (verb.ToUpper() == POST)
                            post = true;
                    }

                }
                else
                {
                    post = attributes.Count(attr => attr is HttpPostAttribute) > 0;
                    get = attributes.Count(attr => attr is HttpGetAttribute) > 0;

                }
            }

            if (post && get || (!post && !get))
            {
                if (Debugger.IsAttached)
                {
                    Debug.WriteLine("JsAction -- I found two HttpVerb attributes, i will use GET as preferred");
                    post = false;
                    get = true;
                }
                else
                    throw new Exception("There are two acceptable HttpVerbs but noone has been marked as preferred!");
            }

            string requestmethod = string.Empty;

            if (post)
                requestmethod = POST;
            else if (get)
                requestmethod = GET;

            StringBuilder jsondata = new StringBuilder();

            foreach (var parameter in method.GetParameters())
            {
                jsondata.AppendFormat("{0}:{0},", parameter.Name);
            }
            if (jsondata.Length > 0)
                jsondata.Remove(jsondata.Length - 1, 1);

            js.AppendFormat("{{var opts = {{url:\"{0}\",cache:{3},type: \"{1}\",data:{{{2}}}}};", url, requestmethod, jsondata, jsattribute.CacheRequest == true ? "true" : "false");
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
                   && m.IsDefined(typeof(NonActionAttribute), inherit) == false
                   select m;
        }
        private RequestContext requestContext;

    }
}
