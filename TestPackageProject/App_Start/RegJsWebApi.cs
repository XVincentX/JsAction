using System;

[assembly: WebActivator.PreApplicationStartMethod(
    typeof(TestPackageProject.App_Start.RegJsWebApi), "PreStart")]

namespace TestPackageProject.App_Start {
    public static class RegJsWebApi {
        public static void PreStart() {
            System.Web.Routing.RouteTable.Routes.Add("JsActionWebApiRoute", JsAction.JsActionWebApiRouteHandlerInstance.JsActionWebApiRoute);
        }
    }
}
