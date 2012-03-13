using System;

[assembly: WebActivator.PreApplicationStartMethod(
    typeof($rootnamespace$.App_Start.MySuperPackage), "PreStart")]

namespace $rootnamespace$.App_Start {
    public static class MySuperPackage {
        public static void PreStart() {
            System.Web.Routing.RouteTable.Add("JsActionWebApiRoute", JsAction.JsActionWebApiRouteHandlerInstance.JsActionWebApiRoute);
        }
    }
}
