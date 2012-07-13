using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web
{
    /// <summary>
    /// Static helper class for JsAction WebApi
    /// </summary>
    public static class JsActionHelper
    {
        /// <summary>
        /// Creates script refence for JsAction WEBAPI
        /// </summary>
        /// <param name="Groups">Optional groups</param>
        /// <returns></returns>
        public static HtmlString WebApiScript(params string[] Groups)
        {
            return new HtmlString(string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", string.Format(VirtualPathUtility.ToAbsolute("~/JsActionWebApi?data={0}"), string.Join(",", Groups))));

        }
    }
}
