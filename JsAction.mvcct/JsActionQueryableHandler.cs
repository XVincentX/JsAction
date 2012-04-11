using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace JsAction.mvcct
{
    /// <summary>
    /// theclass
    /// </summary>
    public class JsActionQueryableHandler : IJsActionHandler
    {
        /// <summary>
        /// Methodcall generation
        /// </summary>
        /// <param name="js"></param>
        /// <param name="method"></param>
        /// <param name="Controller"></param>
        /// <param name="documentate"></param>
        protected override void GenerateMethodCall(StringBuilder js, System.Reflection.MethodInfo method, string Controller, bool documentate)
        {
            var attributes = method.GetCustomAttributes(true);
            var ActionAttr = attributes.Where(attr => attr is ActionNameAttribute);
            var jsattribute = method.DeclaringType.GetCustomAttributes(true).Where(attr => attr is JsActionQueryableAttribute).First() as JsActionQueryableAttribute;

            var pars = method.GetParameters();

            var parameters = string.Join(",", pars.Select(m => m.Name).Union(new string[] { "fop", "options", "negate" }));
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

            if (ActionAttr.Count() > 0)
                methodName = (ActionAttr.First() as ActionNameAttribute).Name;

            js.AppendFormat("{0}:function({1}){{", methodName, parameters.Substring(0, parameters.Length - 1));

            if (documentate)
            {
                //Method can be empty.
                JsActionUtils.DocumentateTheFunction(js, method);
                js.Append("},");
                return;
            }

            string Action = (ActionAttr.Count() != 0) ? (ActionAttr.First() as ActionNameAttribute).Name : method.Name;
            string url = string.Empty;

            if (method.DeclaringType == typeof(Controller))
                url = RouteTable.Routes.GetVirtualPath(this.requestContext, new RouteValueDictionary(new { controller = Controller, action = Action })).VirtualPath;
            else
                url = RouteTable.Routes.GetVirtualPath(this.requestContext, new RouteValueDictionary(new { httproute = "", controller = Controller })).VirtualPath;

            js.AppendFormat(" return mvcct.oDataQueryable('{0}',fop,options,negate);}},", url);
        }

        /// <summary>
        /// Getmethodswith
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="asm"></param>
        /// <returns></returns>
        protected override IEnumerable<IGrouping<Type, System.Reflection.MethodInfo>> GetMethodsWith<TAttribute>(params System.Reflection.Assembly[] asm)
        {
            var methods =
            from ass in asm
            from t in ass.GetTypes()
            where t.IsDefined(typeof(JsActionQueryableAttribute), false)
            from m in t.GetMethods()
            where m.Name.StartsWith("GET", StringComparison.InvariantCultureIgnoreCase)
            && m.ReturnType.IsGenericType == true
            && m.ReturnType.GetGenericTypeDefinition() == typeof(IQueryable<>)
            select m;

            return methods.GroupBy(m => m.DeclaringType);
        }


        /// <summary>
        /// CacheKey
        /// </summary>
        protected override string cacheKey
        {
            get { return "_JSQUERY_"; }
        }

        /// <summary>
        /// InnerObject
        /// </summary>
        protected override string InnerObject
        {
            get { return ".JsQueryables"; }
        }

        protected override bool InjectHelperScript
        {
            get
            {
                return false;
            }
        }
    }
}
