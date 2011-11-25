using System;
using System.Web.Mvc;

namespace JsAction
{
    /// <summary>
    /// JsActionAttribute can be used to mark Action Method and generate related Javascript
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class JsActionAttribute : Attribute
    {
        public JsActionAttribute()
        {
        }

        /// <summary>
        /// Gets or sets Javascript function name to generate. If null, Action name will be used.
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// Gets or sets Http Verb to use in Ajax request. Must be specified if multiple Verbs are accepted for same Action Method.
        /// </summary>
        public HttpSingleVerb Verb { get; set; }
    }

}
