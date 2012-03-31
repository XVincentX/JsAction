using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections;

namespace JsAction
{
    /// <summary>
    /// Http and Route handler for javascript file generation
    /// </summary>
    public class JsActionHandler : IJsActionHandler
    {

        protected override void GenerateMethodCall(StringBuilder js, MethodInfo method, string Controller, bool documentate)
        {

            var attributes = method.GetCustomAttributes(true);
            var ActionAttr = attributes.Where(attr => attr is ActionNameAttribute);
            var jsattribute = attributes.Where(attr => attr is JsActionAttribute).First() as JsActionAttribute;

            if (groups.Count(str => string.IsNullOrEmpty(str) == false) > 0 && jsattribute.Groups.Split(',').Intersect(groups).Count() == 0)
                return;

            var pars = method.GetParameters();

            var parameters = string.Join(",", pars.Select(m => m.Name));
            if (parameters.Length > 0)
                parameters += ',';

            pars.FirstOrDefault(m =>
            {
                if (m.ParameterType.IsGenericType)
                    ComplexTypeList.Value.Add(m.ParameterType.GetGenericArguments().First());
                else if (m.ParameterType != typeof(string) && m.ParameterType.IsPrimitive == false)
                    ComplexTypeList.Value.Add(m.ParameterType);
                return false;
            });

            string methodName = method.Name;

            if (string.IsNullOrEmpty(jsattribute.MethodName) == false)
                methodName = jsattribute.MethodName;
            else if (ActionAttr.Count() > 0)
                methodName = (ActionAttr.First() as ActionNameAttribute).Name;

            js.AppendFormat("{0}:function({1}options)", methodName, parameters);

            js.Append('{');
            if (documentate)
            {
                //Method can be empty.
                JsActionUtils.DocumentateTheFunction(js, method);
                js.Append("},");
                return;

            }

            const string GET = "GET";
            const string POST = "POST";

            string Action = (ActionAttr.Count() != 0) ? (ActionAttr.First() as ActionNameAttribute).Name : method.Name;
            string url = RouteTable.Routes.GetVirtualPath(this.requestContext, new RouteValueDictionary(new { controller = Controller, action = Action })).VirtualPath;

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

            js.AppendFormat("var opts={{success:trd,url:\"{0}\",async:{4},cache:{3},type:\"{1}\",data:$.toDictionary({{{2}}})}};", url, requestmethod, jsondata, jsattribute.CacheRequest == true ? "true" : "false", jsattribute.Async == true ? "true" : "false");
            js.Append("jQuery.extend(opts,options);return jQuery.ajax(opts);},");
        }
        protected override IEnumerable<IGrouping<Type, MethodInfo>> GetMethodsWith<TAttribute>(params Assembly[] asm)
        {
            var methods =
                   from ass in asm
                   from t in ass.GetTypes()
                   from m in t.GetMethods()
                   where m.IsDefined(typeof(TAttribute), false)
                   && m.IsDefined(typeof(NonActionAttribute), false) == false
                   select m;

            return methods.GroupBy(m => m.DeclaringType);

        }
        protected override string cacheKey
        {
            get { return string.Join("_", this.qstring.Split(',').OrderBy(w => w)); }
        }
        protected override string InnerObject
        {
            get { return string.Empty; }
        }
    }
}
