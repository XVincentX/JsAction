using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml;

namespace JsAction
{
    public static class JsActionUtils
    {
        /// <summary>
        /// Generates documentation for current method
        /// </summary>
        /// <param name="js">js Stringbuilder</param>
        /// <param name="method">Method to analyze</param>
        public static void DocumentateTheFunction(StringBuilder js, MethodInfo method)
        {

            const string ajaxOptionParam = "<param name=\"options\" type=\"ajaxSettings\">[OPTIONAL] AjaxOptions partial object; it will be mergend with the one sent to .ajax jQuery function</param>";
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
        public static void ComplexTypeDecomposition(StringBuilder js, Lazy<List<Type>> ComplexTypeList)
        {
            //Works, but not used.
            if (ComplexTypeList.IsValueCreated == false)
                return;

            StringBuilder sb = new StringBuilder(30 * ComplexTypeList.Value.Count);

            foreach (var ctype in ComplexTypeList.Value)
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
    }
}
