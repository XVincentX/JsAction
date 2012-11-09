using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web.Routing;
using System.Web;
using System.Xml;

namespace JsAction
{
    public abstract class JsActionHandler : IHttpHandler, IRouteHandler
    {

        /// <summary>
        /// Is the request reusable?
        /// </summary>
        public bool IsReusable
        {
            get { return true; }
        }
        /// <summary>
        /// Processes the request
        /// </summary>
        /// <param name="context">context</param>
        public virtual void ProcessRequest(HttpContext context)
        {

            string endJsCode = "";
            this.qstring = context.Request.QueryString["data"];
            var documentate = !string.IsNullOrEmpty(context.Request.QueryString["doc"]);
            if (context.Cache.Get(cacheKey) == null || documentate == true)
            {

                var type = context.ApplicationInstance.GetType();

                while (type != null && type.Namespace == "ASP")
                    type = type.BaseType;

                var asm = type == null ? null : type.Assembly;

                this.SearchAsm.Value.Add(asm);

                var groupedMethods = this.GetMethodsWith(SearchAsm.Value.ToArray());

                if (groupedMethods.Count() == 0)
                    return;

                var js = new StringBuilder();
                if (!documentate)
                    js.Append(@"if(typeof trd=='undefined'){function trd(a){for(var b in a){if(typeof a[b]=='object')trd(a[b]);if(typeof a[b]=='string'&&a[b].indexOf('/Date')==0){var c=a[b].replace(/\/Date\((-?\d+)\)\//,'$1');a[b]=new Date(parseInt(c))}}}}").Append("(function(a){if(a.isFunction(String.prototype.format)===false){String.prototype.format=function(){var a=this;varb=arguments.length;while(b--){a=a.replace(new RegExp('\\\\{'+b+'\\\\}','gim'),arguments[b])}return a}}if(a.isFunction(Date.prototype.toISOString)===false){Date.prototype.toISOString=function(){var a=function(a, b){a=a.toString();for(var c=a.length;c<b;c++){a='0'+a}return a};var b=this;return '{0}-{1}-{2}T{3}:{4}:{5}.{6}Z'.format(b.getUTCFullYear(), a(b.getUTCMonth() + 1, 2), a(b.getUTCDate(), 2), a(b.getUTCHours(), 2), a(b.getUTCMinutes(), 2), a(b.getUTCSeconds(), 2), a(b.getUTCMilliseconds(), 3)) } } var b = function (c, d, e, f) { if (a.isPlainObject(c)) { for (var g in c) { if (f === true || typeof c[g] !== 'undefined' && c[g] !== null) { b(c[g], d, e.length > 0 ? e + '.' + g : g, f) } } } else { if (a.isArray(c)) { a.each(c, function (a, c) { b(c, d, '{0}[{1}]'.format(e, a)) }); return } if (!a.isFunction(c)) { if (c instanceof Date) { d.push({ name: e, value: c.toISOString() }) } else { var h = typeof c; switch (h) { case 'boolean': case 'number': h = c; break; case 'object': if (f !== true) { return }; default: h = c || '' } d.push({ name: e, value: h }) } } } }; a.extend({ toDictionary: function (c, d, e) { c = a.isFunction(c) ? c.call() : c; if (arguments.length === 2 && typeof d === 'boolean'){e=d;d=''}e=typeof e==='boolean'?e:false;var f=[];b(c,f,d||'',e);return f}})})(jQuery);");

                js.Append("if (typeof JsActions=='undefined'){var JsActions={};JsActions.WebApi={};}");

                this.groups = context.Request.QueryString["data"].Split(',');

                foreach (var controller in groupedMethods)
                {
                    string ControllerName = controller.Key.Name.Substring(0, controller.Key.Name.IndexOf("Controller"));
                    js.AppendFormat("JsActions{1}.{0}={{", ControllerName, InnerObject);
                    foreach (var method in controller)
                        GenerateMethodCall(js, method, ControllerName, documentate);

                    js.Remove(js.Length - 1, 1).Append("};");
                }

                this.ComplexTypeDecomposition(js, ComplexTypeList, documentate);

                if (alerts.IsValueCreated && !documentate)
                {
                    alerts.Value.Remove(alerts.Value.Length - 1, 1);

                    js.Append("jQuery(document).ready(function(){{jQuery('body').prepend('<div style=\"background-color:#FF8080;border:1px solid black;margin:auto auto auto auto;\"><h2>JsAction debug message</h2><p>The following groupedMethods can handle multiple HttpVerbs, but no preference was choosen, and <a href=\"#\">GET</a> was assumed: <br/><table style=\"border-style:dotted;\"><tr><th>Method Name</th><th>Controller</th><th>Action</th></tr>");
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
                    JsAction.ext.JSBeautify j = new ext.JSBeautify(js.ToString(), new ext.JSBeautifyOptions());
                    endJsCode = j.GetResult();
                    context.Response.Cache.SetCacheability(HttpCacheability.NoCache); //regenerate a new fresh file every time.
                }
                else
                {
                    endJsCode = js.ToString();
                    context.Cache.Insert(cacheKey, endJsCode);
                }
            }
            else
            {
                endJsCode = context.Cache.Get(cacheKey).ToString();
            }

            context.Response.ContentType = "application/javascript";
            context.Response.Charset = string.Empty;
            context.Response.Write(endJsCode);
            context.Response.End();

        }
        /// <summary>
        /// Gets http handler to manage request
        /// </summary>
        /// <param name="requestContext">request context</param>
        /// <returns>HttpHandler</returns>
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            this.requestContext = requestContext;
            return this;
        }

        /// <summary>
        /// Register externals assembly to keep in mind when generating javascript
        /// </summary>
        /// <param name="asm">Assembly to add</param>
        public void RegisterExternalAssembly(params Assembly[] asm)
        {
            this.SearchAsm.Value.AddRange(asm);
        }

        protected void DocumentateTheFunction(StringBuilder js, MethodInfo method)
        {

            const string ajaxOptionParam = "<param name=\"options\" type=\"ajaxSettings\">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param><returns type=\"jqXHR\"/>";
            try
            {
                var doc = ext.DocsByReflection.XMLFromMember(method);
                foreach (XmlNode node in doc.ChildNodes)
                {

                    if (node.Name == "param")
                    {
                        var typenode = node.OwnerDocument.CreateAttribute("type");
                        var thetype = method.GetParameters().Where(w => w.Name == node.Attributes.GetNamedItem("name").Value).First().ParameterType;

                        if (thetype.Name.Contains("`1"))
                        {
                            var GenericType = thetype.GetGenericArguments()[0].Name;
                            typenode.InnerText = string.Concat(GenericType, thetype.Name.Substring(0, thetype.Name.Length - 2));
                        }

                        node.Attributes.Append(typenode);
                    }
                }
                var strdoc = string.Concat(@"///", string.Concat(doc.InnerXml, ajaxOptionParam).Replace(Environment.NewLine, string.Concat(Environment.NewLine, "///")), Environment.NewLine);
                js.Append(strdoc);
            }
            catch (JsAction.ext.DocsByReflectionException)
            {
                //There is no documentation. We will create it on the fly.
                foreach (var parameter in method.GetParameters())
                {
                    string paramType = parameter.ParameterType.Name;
                    if (paramType.Contains("`1"))
                    {
                        var GenericType = parameter.ParameterType.GetGenericArguments()[0].Name;
                        paramType = string.Concat(GenericType, paramType.Substring(0, paramType.Length - 2));
                    }

                    js.AppendFormat("///<param name=\"{0}\" type = \"{1}\"></param>{2}", parameter.Name, paramType, Environment.NewLine);

                }
                js.AppendLine("///" + ajaxOptionParam);
            }

        }

        /// <summary>
        /// Stub. Complex type decomposition for better intellisense
        /// </summary>
        /// <param name="js">js stringBuilder</param>
        /// <param name="ComplexTypeList">Complex list to scan</param>
        protected void ComplexTypeDecomposition(StringBuilder js, Lazy<List<Type>> ComplexTypeList, bool documentate)
        {

            if (!ComplexTypeList.IsValueCreated)
                return;

            var list = ComplexTypeList.Value.Distinct();
            int w = list.Count();
            StringBuilder tmp = new StringBuilder(50);

            for (int i = 0; i < w; i++)
            {

                tmp.Clear().EnsureCapacity(50);
                var props = list.ElementAt(i).GetProperties();
                js.AppendFormat("if (typeof {0} == 'undefined') {{function {0}({1}){{", list.ElementAt(i).Name, string.Join(",", props.Select(p => p.Name)));

                foreach (var prop in props)
                {
                    if (prop.PropertyType.IsClass && !prop.PropertyType.IsPrimitive && prop.PropertyType != typeof(string))
                    {
                        ComplexTypeList.Value.Add(prop.PropertyType.GetType());
                        w++;
                    }
                    else
                    {
                        if (documentate)
                            js.AppendFormat("///<param name=\"{0}\" type = \"{1}\"></param>{2}", prop.Name, prop.PropertyType, Environment.NewLine);
                        tmp.AppendFormat("this.{0}={0};", prop.Name);
                    }
                }
                js.Append(tmp).Append("}}");
            }

        }

        /// <summary>
        /// Gets type for IQueryable
        /// </summary>
        /// <param name="seqType">Sequence type</param>
        /// <returns>Real type reference</returns>
        protected Type FindIEnumerable(Type seqType)
        {
            if (seqType == null || seqType == typeof(string))
                return null;

            if (seqType.IsArray)
                return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());

            if (seqType.IsGenericType)
            {
                foreach (Type arg in seqType.GetGenericArguments())
                {
                    Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
                    if (ienum.IsAssignableFrom(seqType))
                    {
                        return ienum;
                    }
                }
            }

            Type[] ifaces = seqType.GetInterfaces();
            if (ifaces != null && ifaces.Length > 0)
            {
                foreach (Type iface in ifaces)
                {
                    Type ienum = FindIEnumerable(iface);
                    if (ienum != null) return ienum;
                }
            }

            if (seqType.BaseType != null && seqType.BaseType != typeof(object))
            {
                return FindIEnumerable(seqType.BaseType);
            }

            return null;
        }

        #region Abstract Part
        protected abstract void GenerateMethodCall(StringBuilder js, MethodInfo method, string Controller, bool documentate);
        protected abstract IEnumerable<IGrouping<Type, MethodInfo>> GetMethodsWith(params Assembly[] asm);
        protected abstract string cacheKey { get; }
        protected abstract string InnerObject { get; }
        #endregion

        #region Protected Members
        protected RequestContext requestContext;
        protected Lazy<StringBuilder> alerts = new Lazy<StringBuilder>(() => { return new StringBuilder(200); }, false);
        protected Lazy<List<Assembly>> SearchAsm = new Lazy<List<Assembly>>(() => { return new List<Assembly>(3); }, false);
        protected Lazy<List<Type>> ComplexTypeList = new Lazy<List<Type>>(() => { return new List<Type>(4); }, false);
        protected string[] groups;
        protected string qstring { get; set; }
        #endregion


    }
}
