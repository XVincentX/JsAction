using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace JsAction
{
        public static class JsActionRouteHandlerInstance
        {
            public static readonly Route JsActionRoute = new Route("JsAction/", new JsAction.JsActionHandler());
        }
}
