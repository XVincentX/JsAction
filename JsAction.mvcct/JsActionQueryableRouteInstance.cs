using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace JsAction.mvcct
{
    /// <summary>
    /// Default Routehandler instance for Queryable extension
    /// </summary>
    public class JsActionQueryableRouteInstance
    {
        /// <summary>
        /// JsAction Queryable route
        /// </summary>
        public static readonly Route JsActionQueryableRoute = new Route("JsActionQueryable", new RouteValueDictionary(new { action = "JsActionQueryable", controller = "Foo" }), new JsAction.mvcct.JsActionQueryableHandler());
    }
}
