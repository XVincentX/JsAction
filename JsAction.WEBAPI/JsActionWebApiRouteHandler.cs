using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace JsAction
{
    public static class JsActionWebApiRouteHandlerInstance
    {
        public static readonly Route JsActionWebApiRoute = new Route("JsActionWebApi", new RouteValueDictionary(new { action = "JsActionWebApi", controller = "Foo" }), new JsAction.JsActionWebApiHandler());
    }
}
