﻿using System;
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
        /// <summary>
        /// Costructs a new hanlder for JsAction requests
        /// </summary>
        /// <param name="Asm">Optional assembly array in which search for JsAction attribute. If null is specified, only current one will be taken in consideration.</param>
        public JsActionHandler(params Assembly[] Asm)
        {
                this.SearchAsm = Asm;       
        }
        public bool IsReusable
        {
            get { return true; }
        }
        public void ProcessRequest(HttpContext context)
        {


            if (this.SearchAsm.Count() == 0)
            {
                var type = context.ApplicationInstance.GetType();
                while (type != null && type.Namespace == "ASP")
                {
                    type = type.BaseType;
                }

                var asm = type == null ? null : type.Assembly;
                this.SearchAsm = new Assembly[1];
                this.SearchAsm[0] = asm;
            }

           var methods = this.GetMethodsWith<JsActionAttribute>(false, SearchAsm);

            if (methods.Count() == 0)
                return;

            var js = new StringBuilder();
            js.Append("var JsActions = {");
            string[] groups = context.Request.QueryString["data"].Split(',');

            foreach (var method in methods)
            {
                JsActionAttribute attribute = method.GetCustomAttributes(typeof(JsActionAttribute), false).First() as JsActionAttribute;

                if (groups.Count(str => string.IsNullOrEmpty(str) == false) > 0 && attribute.Groups.Split(',').Intersect(groups).Count() == 0)
                    continue;

                js.AppendFormat("{0}:function(", string.IsNullOrEmpty(attribute.MethodName) ? method.Name : attribute.MethodName);

                foreach (var parameter in method.GetParameters())
                    js.AppendFormat("{0},", parameter.Name);

                js.Append("options)");

                GenerateMethodCall(js, method, attribute);
            }

            js.Remove(js.Length - 1, 1).Append("};");

            if (alerts.IsValueCreated)
            {
                alerts.Value.Remove(alerts.Value.Length - 1, 1);

                js.Append("jQuery(document).ready(function(){{jQuery('body').prepend('<div style=\"background-color:#FF8080;border:1px solid black;margin:auto auto auto auto;\"><h2>JsAction debug message</h2><p>The following methods can handle multiple HttpVerbs, but no preference was choosen, and <a href=\"#\">GET</a> was assumed: <br/><table style=\"border-style:dotted;\"><tr><th>Method Name</th><th>Controller</th><th>Action</th></tr>");
                foreach (var item in alerts.Value.ToString().Split(','))
                {
                    js.Append("<tr>");
                    foreach (var it in item.Split('_'))
                        js.AppendFormat("<td>{0}</td>", it);
                    js.Append("</tr>");
                }
                js.Append("</table></p><p>Note: This message will output only in debug mode. JsAction will throw an <b>Exception</b> when debugger is not attached.</p></div>');}});");

                alerts.Value.Clear();
            }

            context.Response.ContentType = "application/javascript";
            context.Response.Charset = string.Empty;
            context.Response.Write(js.ToString());
            context.Response.End();

        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            this.requestContext = requestContext;
            return this;
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
                    alerts.Value.AppendFormat("{2}_{1}_{0},", Action, Controller, jsattribute.MethodName != null ? jsattribute.MethodName : method.Name);
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

            js.AppendFormat("{{var opts={{url:\"{0}\",async:{4},cache:{3},type:\"{1}\",data:{{{2}}}}};", url, requestmethod, jsondata, jsattribute.CacheRequest == true ? "true" : "false", jsattribute.Async == true ? "true" : "false");
            js.Append("jQuery.extend(opts,options);return jQuery.ajax(opts);},");
        }

        internal IEnumerable<MethodInfo> GetMethodsWith<TAttribute>(bool inherit, params Assembly[] asm) where TAttribute : System.Attribute
        {
            return
                   from ass in asm
                   from t in ass.GetTypes()
                   from m in t.GetMethods()
                   where m.IsDefined(typeof(TAttribute), inherit)
                   && m.IsDefined(typeof(NonActionAttribute), inherit) == false
                   select m;
        }

        #region Private Members
        private RequestContext requestContext;
        private Lazy<StringBuilder> alerts = new Lazy<StringBuilder>(() => { return new StringBuilder(600); }, false);
        private Assembly[] SearchAsm;
        #endregion

    }
}
