using System;
using System.Web.Mvc;

namespace JsAction
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class JsActionAttribute : Attribute
    {
        public JsActionAttribute()
        {
        }

        public string MethodName { get; set; }
        public HttpSingleVerb? Verbs { get; set; }
    }

}
