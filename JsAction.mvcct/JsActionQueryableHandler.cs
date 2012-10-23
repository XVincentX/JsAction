using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Collections;

namespace JsAction.mvcct
{
    /// <summary>
    /// JsAction queryable handler
    /// </summary>
    public class JsActionQueryableHandler : JsActionHandler
    {
        /// <summary>
        /// Methodcall generation
        /// </summary>
        /// <param name="js">StringBuilder</param>
        /// <param name="method">Method</param>
        /// <param name="Controller">Controller</param>
        /// <param name="documentate">Documentate funziont</param>
        protected override void GenerateMethodCall(StringBuilder js, System.Reflection.MethodInfo method, string Controller, bool documentate)
        {
            var jsattribute = method.GetCustomAttributes(true).Where(attr => attr is JsqSelectMethodAttribute || attr is JsqUpdateMethodAttribute).First();

            var pars = method.GetParameters();

            string UrlToMethod = string.Empty;
            UrlToMethod = GenerateUrlToMethod(method, Controller, UrlToMethod);

            if (string.IsNullOrEmpty(UrlToMethod))
                throw new Exception("Unable to generate method Url");

            #region Select Method
            if (jsattribute.GetType() == typeof(JsqSelectMethodAttribute))
            {
                if (method.ReturnType.GetGenericTypeDefinition() != typeof(IQueryable<>))
                    throw new Exception("JsqSelectMethod Attribute should be applied only on IQueryable<> returning methods");

                var parameters = string.Join(",", pars.Select(m => m.Name).Union(new string[] { "fop", "options", "negate" }));
                if (parameters.Length > 0)
                    parameters += ',';

                pars.FirstOrDefault(m =>
                {
                    if (m.ParameterType.IsGenericType)
                        ComplexTypeList.Value.Add(m.ParameterType.GetGenericArguments().First());
                    else if (this.FindIEnumerable(m.ParameterType) == null && m.ParameterType != typeof(DateTime) && m.ParameterType != typeof(DateTimeOffset) && m.ParameterType.IsPrimitive == false)
                        ComplexTypeList.Value.Add(m.ParameterType);
                    return false;
                });

                js.AppendFormat("{0}:function({1}){{", method.Name, parameters.Substring(0, parameters.Length - 1));

                if (documentate)
                {
                    js.AppendFormat("///<summary>Queries the \"{0}.{2}\" ApiController method.</summary>{1}///<param name=\"fop\" type=\"LogicalOperator\">The top level logical operator used to combine the filter conditions</param>{1}///<param name=\"options\" type=\"Object\">Options; see documentation</param>{1}///<param name=\"negate\" type=\"Boolean\">If true the filter is negated</param>{1}///<returns type=\"mvcct.oDataQueryable\">oDataQueryable</returns>}},", Controller, Environment.NewLine, method.Name);
                    return;
                }

                js.AppendFormat("return mvcct.oDataQueryable('{0}',fop,options,negate);}},", UrlToMethod);
            }
            #endregion
            #region Update Method
            else //JsqUpdateMethodAttribute
            {
                foreach (var param in method.GetParameters())
                {
                    if (param.IsDefined(typeof(JsqChangesetAttribute), false))
                        SingleChangeset(js, method, UrlToMethod, documentate);
                    else if (this.FindIEnumerable(param.ParameterType) == null &&
                            param.ParameterType != typeof(DateTime) &&
                            param.ParameterType != typeof(DateTimeOffset) &&
                            param.ParameterType.IsPrimitive == false)
                        MultipleChangeset(js, param.ParameterType, UrlToMethod, documentate, string.Empty);
                }
            }
            #endregion
        }
        private string GenerateUrlToMethod(System.Reflection.MethodInfo method, string Controller, string UrlToMethod)
        {
            if (method.DeclaringType == typeof(Controller))
                UrlToMethod = RouteTable.Routes.GetVirtualPath(this.requestContext, new RouteValueDictionary(new { controller = Controller, action = method.Name })).VirtualPath;
            else //ApiController
                UrlToMethod = RouteTable.Routes.GetVirtualPath(this.requestContext, new RouteValueDictionary(new { httproute = "", controller = Controller })).VirtualPath;
            return UrlToMethod;
        }
        private void MultipleChangeset(StringBuilder js, Type param, string url, bool documentate, string currentNavigationProp)
        {

            ComplexTypeList.Value.Add(param);

            foreach (var prop in param.GetProperties())
            {
                if (prop.GetCustomAttributes(false).Where(a => a.GetType() == typeof(JsqChangesetAttribute)).Count() > 0)
                    SingleChangeset(js, prop, url, documentate, currentNavigationProp);
                else
                    if (prop.PropertyType.IsClass && prop.PropertyType.IsAssignableFrom(typeof(IEnumerable)) == false)
                        MultipleChangeset(js, prop.PropertyType, url, documentate, string.Concat(currentNavigationProp, string.Format("{0}.", prop.Name)));
            }
        }
        private void SingleChangeset(StringBuilder js, MethodInfo method, string url, bool documentate)
        {
            try
            {
                var par = method.GetParameters().Where(p => p.IsDefined(typeof(JsqChangesetAttribute), false)).Single();
                var attr = par.GetCustomAttributes(true).Where(attri => attri.GetType() == typeof(JsqChangesetAttribute)).Single() as JsqChangesetAttribute;
                var viewmodel = this.FindIEnumerable(par.ParameterType.GetProperties().Where(w => w.Name == attr.InsertedEntities).Single().PropertyType).GetGenericArguments().Single();
                string key = string.Empty;

                var props = viewmodel.GetProperties();
                foreach (var prop in props)
                {
                    if (prop.IsDefined(typeof(KeyAttribute), false) || prop.Name.EndsWith("id", StringComparison.InvariantCultureIgnoreCase))
                    {
                        key = prop.Name;
                        break;
                    }
                }

                if (documentate)
                    js.AppendFormat("{0}:function(sourceViewModel,sourceExpression,options){{///<summary>Handles the '{1}' collection, on the client and the post of its change set to the server.</summary>{2}///<param name='sourceViewModel' mayBeNull='false' optional='false' type='String' parameterArray='false' integer='false' domElement='false'>The Client Side ViewModel that hosts the collection.</param>{2}///<param name='sourceExpression' mayBeNull='false' optional='false' type='String' parameterArray='false' integer='false' domElement='false'>The property of the ViewModel that hosts the collection as a string expression.</param>{2}///<param name='key' mayBeNull='true' optional='true' type='String' parameterArray='false' integer='false' domElement='false'>[OPTIONAL] Overrides the object key retrieved automatically.</param>{2}///<param name='options' mayBeNull='true' optional='true' type='Object' parameterArray='false' integer='false' domElement='false'>Options (see documentation).</param>{2}///<returns type='mvcct.updatesManager' integer='false' domElement='false' mayBeNull='false'>updatesManager</returns>{2}}},", method.Name, viewmodel.Name, Environment.NewLine);
                else
                    js.AppendFormat("{0}:function(sourceViewModel,sourceExpression,options){{return mvcct.updatesManager('{1}',sourceViewModel,sourceExpression,'{2}',null,null,jQuery.extend({{updater: {{u: '{3}', i: '{4}', d: '{5}', f: '{6}'}}}},options)); }},", method.Name, url, key, attr.ModifiedEntities, attr.InsertedEntities, attr.DeletedEntities, attr.FatherReferencesEntities);


            }
            catch (Exception ex)
            {
                Type exType = ex.GetType();
                if (exType == typeof(ArgumentNullException) || exType == typeof(NullReferenceException))
                    throw new Exception(string.Format("JsqUpdateMethod attribute was placed on {0} method, but no Changeset/Multiple changeset attribute has been placed on parameters or its properties!", method.Name));
                else
                    throw;
            }


        }
        private void SingleChangeset(StringBuilder js, PropertyInfo prop, string url, bool documentate, string currentNavigationProp)
        {
            try
            {
                var attr = prop.GetCustomAttributes(true).Where(attri => attri.GetType() == typeof(JsqChangesetAttribute)).Single() as JsqChangesetAttribute;
                var viewmodel = this.FindIEnumerable(prop.PropertyType.GetProperties().Where(w => w.Name == attr.ModifiedEntities).Single().PropertyType).GetGenericArguments().Single();
                string key = string.Empty;

                var props = viewmodel.GetProperties();
                foreach (var prp in prop.PropertyType.GetProperties())
                {
                    if (prp.IsDefined(typeof(KeyAttribute), false) || prp.Name.EndsWith("id", StringComparison.InvariantCultureIgnoreCase))
                    {
                        key = prp.Name;
                        break;
                    }
                }

                if (documentate)
                    js.AppendFormat("{0}:function(sourceViewModel,sourceExpression,options){{///<summary>Handles the '{1}' collection, on the client and the post of its change set to the server.</summary>{2}///<param name='sourceViewModel' mayBeNull='false' optional='false' type='String' parameterArray='false' integer='false' domElement='false'>The Client Side ViewModel that hosts the collection.</param>{2}///<param name='sourceExpression' mayBeNull='false' optional='false' type='String' parameterArray='false' integer='false' domElement='false'>The property of the ViewModel that hosts the collection as a string expression.</param>{2}///<param name='key' mayBeNull='true' optional='true' type='String' parameterArray='false' integer='false' domElement='false'>[OPTIONAL] Overrides the object key retrieved automatically.</param>{2}///<param name='options' mayBeNull='true' optional='true' type='Object' parameterArray='false' integer='false' domElement='false'>Options (see documentation).</param>{2}///<returns type='mvcct.updatesManager' integer='false' domElement='false' mayBeNull='false'>updatesManager</returns>{2}}},", prop.Name, viewmodel.Name, Environment.NewLine);
                else
                    js.AppendFormat("{0}:function(sourceViewModel,sourceExpression,options){{return mvcct.updatesManager('{1}',sourceViewModel,sourceExpression,'{2}',null,'{7}',jQuery.extend({{updater: {{u: '{3}', i: '{4}', d: '{5}', f: '{6}'}}}},options)); }},", prop.Name, url, key, attr.ModifiedEntities, attr.InsertedEntities, attr.DeletedEntities, attr.FatherReferencesEntities, currentNavigationProp);


            }
            catch (Exception ex)
            {
                Type exType = ex.GetType();
                if (exType == typeof(ArgumentNullException) || exType == typeof(NullReferenceException))
                    throw new Exception(string.Format("JsqUpdateMethod attribute was placed on {0} method, but no Changeset/Multiple changeset attribute has been placed on parameters or its properties!", prop.Name));
                else
                    throw;
            }


        }

        /// <summary>
        /// Get methods on which attribute are set
        /// </summary>
        /// <param name="asm">External assembly</param>
        /// <returns>Methods</returns>
        protected override IEnumerable<IGrouping<Type, System.Reflection.MethodInfo>> GetMethodsWith(params System.Reflection.Assembly[] asm)
        {
            var methods =
            from ass in asm
            from t in ass.GetTypes()
            from m in t.GetMethods()
            where m.Name.StartsWith("GET", StringComparison.InvariantCultureIgnoreCase)
            && (m.IsDefined(typeof(JsqSelectMethodAttribute), false) == true
            && m.ReturnType.IsGenericType == true)
            || m.IsDefined(typeof(JsqUpdateMethodAttribute), false)
            select m;


            return methods.GroupBy(m => m.DeclaringType);
        }

        /// <summary>
        /// Processes current request
        /// </summary>
        /// <param name="context">HttpContext to handle with</param>
        public override void ProcessRequest(System.Web.HttpContext context)
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

                js.Append("if (typeof JsActions=='undefined'){var JsActions={};}JsActions.JsQueryables={};");

                foreach (var controller in groupedMethods)
                {
                    string ControllerName = controller.Key.Name.Substring(0, controller.Key.Name.IndexOf("Controller"));
                    js.AppendFormat("JsActions{1}.{0}={{", ControllerName, InnerObject);
                    foreach (var method in controller)
                        GenerateMethodCall(js, method, ControllerName, documentate);

                    js.Remove(js.Length - 1, 1).Append("};");
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
    }
}
