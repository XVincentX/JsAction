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
        /// <summary>
        /// Object .ctor
        /// </summary>
        public JsActionAttribute()
        {
            Verb = HttpSingleVerb.None;
            CacheRequest = true;
            Async = true;
            Groups = string.Empty;
            this._methodname = null;
        }

        /// <summary>
        /// Gets or sets Javascript function name to generate. If null, Action name will be used.
        /// </summary>
        public string MethodName
        {
            get { return _methodname; }
            set
            {
                if (!JsVariableConventionMatching(value))
                    throw new Exception("Function name does not respect standard variables name convention.");

                _methodname = value;
            }
        }

        /// <summary>
        /// Gets or sets Http Verb to use in Ajax request. Must be specified if multiple Verbs are accepted for same Action Method.
        /// </summary>
        public HttpSingleVerb Verb { get; set; }

        /// <summary>
        /// Gets or sets group names for function, separated by comma values
        /// </summary>
        public string Groups { get; set; }

        /// <summary>
        /// Gets or sets if cache option parameter should be set.
        /// </summary>
        public bool CacheRequest { get; set; }

        /// <summary>
        /// Gets or sets if request should be async or not.
        /// </summary>
        public bool Async { get; set; }

        /// <summary>
        /// Method to check compliance with standard JS variable convention
        /// </summary>
        /// <param name="value">Value to validate</param>
        /// <returns>Bool value, if valid or not.</returns>
        internal bool JsVariableConventionMatching(string value)
        {
            if (string.IsNullOrEmpty(value))
                return true;
            return (System.Text.RegularExpressions.Regex.IsMatch(value, @"^[\p{L}\p{Nl}$_][\p{L}\p{Nl}$\p{Mn}\p{Mc}\p{Nd}\p{Pc}]*$"));
        }

        /// <summary>
        /// Internal method name string
        /// </summary>
        private string _methodname;

    }

}
