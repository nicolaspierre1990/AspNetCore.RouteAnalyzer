using AspNetCore.RouteAnalyzer.Inner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;

namespace AspNetCore.RouteAnalyzer
{
    public static class RouteAnalyzerServiceCollectionExtensions
    {
        public static IServiceCollection AddRouteAnalyzer(this IServiceCollection services)
        {
            services.AddSingleton<IRouteAnalyzer, RouteAnalyzerImpl>();
            return services;
        }
    }

    public static class RouteAnalyzerRouteBuilderExtensions
    {
        private static string AspNetCoreRouteAnalyzerRoute = "AspNetCore.RouteAnalyzer.Route";
        public static string RouteAnalyzerUrlPath { get; private set; } = "";

        public static IApplicationBuilder MapRouteAnalyzer(this IApplicationBuilder app,  string routeAnalyzerUrlPath)
        {        
            var aspnetCorErouteHandler = new RouteHandler(context =>
            {
                return Task.Run(() =>
                {
                    context.Response.Redirect("RouteAnalyzer_Main/ShowAllRoutes");
                });
            });

            var routeBuilder = new RouteBuilder(app, aspnetCorErouteHandler);

            routeBuilder.MapRoute(AspNetCoreRouteAnalyzerRoute, routeAnalyzerUrlPath);

            var routes = routeBuilder.Build();
            app.UseRouter(routes);

            return app;
        }
    }
}
