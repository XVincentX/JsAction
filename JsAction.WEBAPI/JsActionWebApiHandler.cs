using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Routing;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Web.Http.WebHost.Routing;
using System.Collections;


namespace JsAction
{
    class JsActionWebApiHandler : IJsActionHandler
    {

        protected override void GenerateMethodCall(StringBuilder js, System.Reflection.MethodInfo method, string Controller, bool documentate)
        {
            var jsattribute = method.DeclaringType.GetCustomAttributes(typeof(JsActionAttribute), false).First() as JsActionAttribute;

            if (groups.Count(str => string.IsNullOrEmpty(str) == false) > 0 && jsattribute.Groups.Split(',').Intersect(groups).Count() == 0)
                return;

            var pars = method.GetParameters();

            var parameters = string.Join(",", pars.Select(m => m.Name));
            if (parameters.Length > 0)
                parameters += ',';
            js.AppendFormat("{0}:function({1}options)", method.Name, parameters);

            pars.FirstOrDefault(m =>
            {
                if (m.ParameterType.IsGenericType)
                    ComplexTypeList.Value.Add(m.ParameterType.GetGenericArguments().First());
                else if (m.ParameterType != typeof(string) && m.ParameterType != typeof(DateTime) && m.ParameterType != typeof(DateTimeOffset) && m.ParameterType.IsPrimitive == false)
                    ComplexTypeList.Value.Add(m.ParameterType);
                return false;
            });

            js.Append('{');
            if (documentate)
            {
                //Method can be empty.
                JsActionUtils.DocumentateTheFunction(js, method);
                js.Append("},");
                return;

            }

            string url = RouteTable.Routes.Where(q => q is HttpWebRoute).First().GetVirtualPath(this.requestContext, new RouteValueDictionary(new { httproute = "", controller = Controller })).VirtualPath;

            StringBuilder jsondata = new StringBuilder();

            foreach (var parameter in method.GetParameters())
            {
                jsondata.AppendFormat("{0}:{0},", parameter.Name);
            }

            if (jsondata.Length > 0)
                jsondata.Remove(jsondata.Length - 1, 1);

            string requestmethod = string.Empty;

            if (method.Name.StartsWith("GET", StringComparison.InvariantCultureIgnoreCase))
                requestmethod = "GET";
            else if (method.Name.StartsWith("POST", StringComparison.InvariantCultureIgnoreCase))
                requestmethod = "POST";
            else if (method.Name.StartsWith("PUT", StringComparison.InvariantCultureIgnoreCase))
                requestmethod = "PUT";
            else if (method.Name.StartsWith("DELETE", StringComparison.InvariantCultureIgnoreCase))
                requestmethod = "DELETE";


            js.AppendFormat("var opts={{success:trd,url:\"{0}\",async:{4},cache:{3},type:\"{1}\",data:$.toDictionary({{{2}}})}};", url, requestmethod, jsondata, jsattribute.CacheRequest == true ? "true" : "false", jsattribute.Async == true ? "true" : "false");
            js.Append("jQuery.extend(opts,options);return jQuery.ajax(opts);},");
        }

        protected override IEnumerable<IGrouping<Type, System.Reflection.MethodInfo>> GetMethodsWith<TAttribute>(params System.Reflection.Assembly[] asm)
        {

            var methods =
           from ass in asm
           from t in ass.GetTypes()
           from m in t.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
           where t.IsDefined(typeof(TAttribute), false)
           && m.IsDefined(typeof(NonActionAttribute), false) == false

           && t.IsSubclassOf(typeof(ApiController))
           select m;

            return methods.GroupBy(m => m.DeclaringType);
        }

        protected override string cacheKey
        {
            get { return string.Concat("_API_", string.Join("_", this.qstring.Split(',').OrderBy(w => w))); }
        }

        protected override string InnerObject
        {
            get { return ".WebApi"; }
        }

    }
}