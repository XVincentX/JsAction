﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;

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

            string endJsCode = "";

            if (context.Cache.Get("endJsCode") == null)
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

                this.urlhelper = new UrlHelper(this.requestContext, RouteTable.Routes);
                var documentate = context.Request.QueryString["doc"] == "1";
                var methods = this.GetMethodsWith<JsActionAttribute>(false, SearchAsm);

                if (methods.Count() == 0)
                    return;

                var js = new StringBuilder();
                js.AppendFormat("/*Generated: {0}*/", DateTime.Now.ToString());
                if (!documentate)
                    js.Append("(function (a) { if (a.isFunction(String.prototype.format) === false) { String.prototype.format = function () { var a = this; var b = arguments.length; while (b--) { a = a.replace(new RegExp('\\\\{' + b + '\\\\}', 'gim'), arguments[b]) } return a } } if (a.isFunction(Date.prototype.toISOString) === false) { Date.prototype.toISOString = function () { var a = function (a, b) { a = a.toString(); for (var c = a.length; c < b; c++) { a = '0' + a } return a }; var b = this; return '{0}-{1}-{2}T{3}:{4}:{5}.{6}Z'.format(b.getUTCFullYear(), a(b.getUTCMonth() + 1, 2), a(b.getUTCDate(), 2), a(b.getUTCHours(), 2), a(b.getUTCMinutes(), 2), a(b.getUTCSeconds(), 2), a(b.getUTCMilliseconds(), 3)) } } var b = function (c, d, e, f) { if (a.isPlainObject(c)) { for (var g in c) { if (f === true || typeof c[g] !== 'undefined' && c[g] !== null) { b(c[g], d, e.length > 0 ? e + '.' + g : g, f) } } } else { if (a.isArray(c)) { a.each(c, function (a, c) { b(c, d, '{0}[{1}]'.format(e, a)) }); return } if (!a.isFunction(c)) { if (c instanceof Date) { d.push({ name: e, value: c.toISOString() }) } else { var h = typeof c; switch (h) { case 'boolean': case 'number': h = c; break; case 'object': if (f !== true) { return }; default: h = c || '' } d.push({ name: e, value: h }) } } } }; a.extend({ toDictionary: function (c, d, e) { c = a.isFunction(c) ? c.call() : c; if (arguments.length === 2 && typeof d === 'boolean') { e = d; d = '' } e = typeof e === 'boolean' ? e : false; var f = []; b(c, f, d || '', e); return f } }) })(jQuery);");

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

                    GenerateMethodCall(js, method, attribute, documentate);
                }

                js.Remove(js.Length - 1, 1).Append("};");

                if (alerts.IsValueCreated && !documentate)
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

                if (documentate)
                {
                    //ComplexTypeDecomposition(js);
                    JsAction.ext.JSBeautify j = new ext.JSBeautify(js.ToString(), new ext.JSBeautifyOptions());
                    endJsCode = j.GetResult();
                }
                else
                {
                    endJsCode = js.ToString();

                    context.Cache.Insert("endJsCode", endJsCode);
                }
            }
            else
            {
                endJsCode = context.Cache.Get("endJsCode").ToString();
            }

            TimeSpan refresh = new TimeSpan(365, 0, 0, 0, 0);
            context.Response.Cache.SetExpires(DateTime.Today.Add(refresh));
            context.Response.Cache.SetMaxAge(refresh);
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetValidUntilExpires(true);
            context.Response.Cache.SetETag(string.IsNullOrEmpty(context.Request.QueryString["data"]) ? "Not" : context.Request.QueryString["data"]);

            context.Response.ContentType = "application/javascript";
            context.Response.Charset = string.Empty;
            context.Response.Write(endJsCode);
            context.Response.End();

        }

        internal void ComplexTypeDecomposition(StringBuilder js)
        {
            //Works, but not used.
            if (this.ComplexTypeList.IsValueCreated == false)
                return;

            StringBuilder sb = new StringBuilder(30 * this.ComplexTypeList.Value.Count);

            foreach (var ctype in this.ComplexTypeList.Value)
            {
                var props = ctype.GetProperties().Select(p => p.Name);
                sb.AppendFormat("{0} = function({1}){{", ctype.Name, string.Join(",", props));
                foreach (var prop in props)
                {
                    sb.AppendFormat("this.{0}={0};", prop);
                }
                sb.Append('}');
            }
        }

        internal void DocumentateTheFunction(StringBuilder js, MethodInfo method)
        {
            try
            {
                var doc = ext.DocsByReflection.XMLFromMember(method);
                foreach (XmlNode node in doc.ChildNodes)
                {

                    if (node.Name == "param")
                    {
                        var typenode = node.OwnerDocument.CreateAttribute("type");
                        var thetype = method.GetParameters().Where(w => w.Name == node.Attributes.GetNamedItem("name").Value).First().ParameterType;
                        //                       if (!thetype.IsPrimitive)
                        //                           this.ComplexTypeList.Value.Add(thetype);

                        typenode.InnerText = thetype.Name;
                        node.Attributes.Append(typenode);
                    }
                }
                var strdoc = string.Concat(@"///", string.Concat(doc.InnerXml, "<param name=\"options\" type=\"ajaxSettings\">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param>").Replace(Environment.NewLine, string.Concat(Environment.NewLine, "///")), Environment.NewLine);
                js.Append(strdoc);
            }
            catch (JsAction.ext.DocsByReflectionException c)
            {
                js.AppendLine(string.Concat("//WARNING - JsAction:", c.Message, ": ", method.Name, ",", method.DeclaringType.Assembly.ToString(), ",", method.DeclaringType));
                return;
            }
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            this.requestContext = requestContext;
            return this;
        }

        internal void GenerateMethodCall(StringBuilder js, MethodInfo method, JsActionAttribute jsattribute, bool documentate)
        {

            const string GET = "GET";
            const string POST = "POST";

            var attributes = method.GetCustomAttributes(true);
            var ActionAttr = attributes.Where(attr => attr is ActionNameAttribute);

            string Action = (ActionAttr.Count() != 0) ? (ActionAttr.First() as ActionNameAttribute).Name : method.Name;
            string Controller = method.DeclaringType.Name.Substring(0, method.DeclaringType.Name.IndexOf("Controller"));

            string url = urlhelper.Action(Action, Controller);

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

            js.Append('{');
            if (documentate)
            {
                DocumentateTheFunction(js, method);
            }
            js.AppendFormat("var opts={{url:\"{0}\",async:{4},cache:{3},type:\"{1}\",data:$.toDictionary({{{2}}})}};", url, requestmethod, jsondata, jsattribute.CacheRequest == true ? "true" : "false", jsattribute.Async == true ? "true" : "false");
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
        private UrlHelper urlhelper;
        private Lazy<List<Type>> ComplexTypeList = new Lazy<List<Type>>(() => { return new List<Type>(4); }, false);
        #endregion

    }
}
