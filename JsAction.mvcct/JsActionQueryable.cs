using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace JsAction.mvcct
{
    /// <summary>
    /// JsActionQueryable attribute can be used to mark Action Method and generate related Javascript.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false), ImmutableObject(true)]
    public sealed class JsActionQueryableAttribute : JsActionAttribute
    {

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), ReadOnly(true)]
        private new string MethodName { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), ReadOnly(true)]
        private new HttpSingleVerb Verb { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), ReadOnly(true)]
        private new string Groups { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), ReadOnly(true)]
        private new bool CacheRequest { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never), Browsable(false), ReadOnly(true)]
        private new bool Async { get; set; }
    }


}
