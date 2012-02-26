using System.Web;
using System.Web.Mvc;

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
        public static MvcHtmlString JsScript(this HtmlHelper helper, params string[] Groups)
        {
            return MvcHtmlString.Create(string.Format("<script type=\"text/javascript\" src=\"{0}\"></script>", string.Format(VirtualPathUtility.ToAbsolute("~/JsAction?data={0}"), string.Join(",", Groups))));
        }
    }
}
