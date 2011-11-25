using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace JsAction
{
    public static class JsActionHelperExtension
    {
        /// <summary>
        /// Writes into view the needed script reference for JsAction.
        /// </summary>
        /// <param name="helper">static html helper</param>
        /// <returns>Html String that represents script reference</returns>
        public static MvcHtmlString JsScript(this HtmlHelper helper)
        {
            return MvcHtmlString.Create(string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", VirtualPathUtility.ToAbsolute("~/JsAction/")));
        }
    }
}
