using System;

[assembly: WebActivator.PreApplicationStartMethod(
    typeof(TestPackageProject.App_Start.RegJsAction), "PreStart")]

namespace TestPackageProject.App_Start {
    public static class RegJsAction {
        public static void PreStart() {
            System.Web.Routing.RouteTable.Routes.Add("JsActionRoute", JsAction.JsActionRouteHandlerInstance.JsActionRoute);
        }
    }
}
