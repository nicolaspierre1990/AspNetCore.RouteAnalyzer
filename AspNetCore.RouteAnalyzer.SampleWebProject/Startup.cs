using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace AspNetCore.RouteAnalyzer.SampleWebProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private IRouteAnalyzer m_routeAnalyzer;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddRouting();
            services.AddRouteAnalyzer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            IApplicationLifetime applicationLifetime,
            IRouteAnalyzer routeAnalyzer
        )
        {
            m_routeAnalyzer = routeAnalyzer;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                //routes.MapRouteAnalyzer("/routes", app.ApplicationServices);
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.MapRouteAnalyzer("/routes");

            // Lifetime events
            applicationLifetime.ApplicationStarted.Register(OnStarted);
            applicationLifetime.ApplicationStopping.Register(OnStopping);
            applicationLifetime.ApplicationStopped.Register(OnStopped);
        }

        void OnStarted()
        {
            var infos = m_routeAnalyzer.GetAllRouteInformations();
            Debug.WriteLine("======== ALL ROUTE INFORMATION ========");
            foreach(var info in infos)
            {
                Debug.WriteLine(info.ToString());
            }
            Debug.WriteLine("");
            Debug.WriteLine("");
        }

        void OnStopping()
        {
        }

        void OnStopped()
        {
        }
    }
}
