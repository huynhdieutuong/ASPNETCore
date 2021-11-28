using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASPNETCore
{
    public class MyStartUp
    {
        // 1. Register Services (using Dependency Injection) 
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddSingleton
        }

        // 2. Build pipeline (middlewares)
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 2.3 EndpointRoutingMiddleware
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                // GET /
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("This is Home Page.");
                });

                // GET /about
                endpoints.MapGet("/about", async context =>
                {
                    await context.Response.WriteAsync("This is About Page.");
                });

                // GET /contact
                endpoints.MapGet("/contact", async context =>
                {
                    await context.Response.WriteAsync("This is Contact Page.");
                });
            });

            // 2.2 Terminate Middleware (the end middleware for /abc endpoint & /abc/xyz...)
            app.Map("/abc", app1 =>
            {
                app1.Run(async (context) =>
                {
                    await context.Response.WriteAsync("This is the end middleware ABC");
                });
            });

            // 2.1 Terminate Middleware (the end middleware for all endpoints, except /abc endpoint)
            // app.Run(async (HttpContext context) =>
            // {
            //     await context.Response.WriteAsync("This is the end middleware, if it has middlewares below, then middlewares below will not execute");
            // });
            // Use StatusCodePages middleware instead of app.Run (2.1) for 404 Page
            app.UseStatusCodePages();
        }
    }
}