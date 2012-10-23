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
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class JsqSelectMethodAttribute  :   Attribute
    {
    }
}
