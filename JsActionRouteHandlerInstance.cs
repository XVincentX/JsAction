using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace JsAction
{
    public static class JsActionRouteHandlerInstance
    {
        /// <summary>
        /// Static singleton Route to add
        /// </summary>
        public static readonly Route JsActionRoute = new Route("JsAction", new RouteValueDictionary(new { action = "JsAction", controller = "Foo" }), new JsAction.JsActionHandler());


    }
}
