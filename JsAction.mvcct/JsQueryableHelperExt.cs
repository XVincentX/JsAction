using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace JsAction.mvcct
{
    /// <summary>
    /// Static helper class ext
    /// </summary>
    public static class JsQueryableHelperExt
    {
        /// <summary>
        /// Injects script reference to JsQueryables
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <returns>href script</returns>
        public static MvcHtmlString JsActionQueryableScript(this HtmlHelper helper)
        {
            return MvcHtmlString.Create(string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", VirtualPathUtility.ToAbsolute("~/JsActionQueryable?data=")));
        }
    }
}
