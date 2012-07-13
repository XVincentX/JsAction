using System;

[assembly: WebActivator.PreApplicationStartMethod(
    typeof($rootnamespace$.App_Start.RegJsWebApi), "PreStart")]

namespace $rootnamespace$.App_Start {
    public static class RegJsWebApi {
        public static void PreStart() {
            System.Web.Routing.RouteTable.Routes.Add("JsActionWebApiRoute", JsAction.JsActionWebApiRouteHandlerInstance.JsActionWebApiRoute);
        }
    }
}
