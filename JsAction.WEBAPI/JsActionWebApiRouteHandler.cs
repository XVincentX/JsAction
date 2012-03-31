using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace JsAction
{
    /// <summary>
    /// Container static class
    /// </summary>
    public static class JsActionWebApiRouteHandlerInstance
    {
        /// <summary>
        /// Predefined JsAction WebApi route
        /// </summary>
        public static readonly Route JsActionWebApiRoute = new Route("JsActionWebApi", new RouteValueDictionary(new { action = "JsActionWebApi", controller = "Foo" }), new JsAction.JsActionWebApiHandler());
    }
}
