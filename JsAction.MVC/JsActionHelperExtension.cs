using System.Web;
using System.Web.Mvc;
using System;
using System.ComponentModel;

namespace JsAction
{
    /// <summary>
    /// JsAction Helper extension
    /// </summary>
    public static class JsActionHelperExtension
    {
        /// <summary>
        /// Writes into view the needed script reference for JsAction.
        /// </summary>
        /// <param name="helper">static html helper</param>
        /// <param name="Groups">Groups name to import</param>
        /// <returns>Html String that represents script reference</returns>
        [Obsolete("Consider to use @JsActionHelper.MVCScript, placed in System.Web.Mvc namespace", false)]
        public static MvcHtmlString JsScript(this HtmlHelper helper, params string[] Groups)
        {
            return MvcHtmlString.Create(string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", string.Format(VirtualPathUtility.ToAbsolute("~/JsAction?data={0}"), string.Join(",", Groups))));
        }
    }
}

namespace System.Web.Mvc
{
    /// <summary>
    /// JsAction Helper
    /// </summary>
    public static class JsActionHelperExtension
    {
        /// <summary>
        /// Writes into view the needed script reference for JsAction.
        /// </summary>
        /// <param name="Groups">Groups name to import</param>
        /// <returns>Html String that represents script reference</returns>
        public static MvcHtmlString JsActionScript(this HtmlHelper helper, params string[] Groups)
        {
            return MvcHtmlString.Create(string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", string.Format(VirtualPathUtility.ToAbsolute("~/JsAction?data={0}"), string.Join(",", Groups))));
        }

        public static MvcHtmlString JsActionQueryableScript(this HtmlHelper helper)
        {
            return MvcHtmlString.Create(string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", VirtualPathUtility.ToAbsolute("~/JsActionQueryable?data=")));
        }
    }
}