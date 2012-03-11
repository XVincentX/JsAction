using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsAction
{
    /// <summary>
    /// Enumeration that rapresents supported HttpVerbs for JsAction library. Ignored when using WebApi support.
    /// </summary>
    /// <remarks>Ignored when using WebApi support</remarks>
    public enum HttpSingleVerb : short
    {
        /// <summary>
        /// No HttpVerb has been selected
        /// </summary>
        None = 0,
        /// <summary>
        /// GET Http verb
        /// </summary>
        HttpGet = 1,
        /// <summary>
        /// POST Http verb
        /// </summary>
        HttpPost = 2
    }
}
