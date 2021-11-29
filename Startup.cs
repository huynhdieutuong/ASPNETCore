using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASPNETCore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<SecondMiddleware>(); // 4. Inject SecondMiddleware
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            // app.UseMiddleware<FirstMiddleware>(); // 1. using UseMiddleware
            app.UseFirstMiddleware(); // 2. create extension method for app

            // 5. use middleware
            // app.UseMiddleware<SecondMiddleware>();
            app.UseSecondMiddleware();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    var menu = HtmlHelper.MenuTop(
                        HtmlHelper.DefaultMenuTopItems(),
                        context.Request
                    );

                    var html = HtmlHelper.HtmlDocument("HELLO", menu + HtmlHelper.HtmlTrangchu());

                    await context.Response.WriteAsync(html);
                });

                endpoints.MapGet("/RequestInfo", async context =>
                {
                    await context.Response.WriteAsync("RequestInfo");
                });

                endpoints.MapGet("/Encoding", async context =>
                {
                    await context.Response.WriteAsync("Encoding");
                });

                endpoints.MapGet("/Cookies", async context =>
                {
                    await context.Response.WriteAsync("Cookies");
                });

                endpoints.MapGet("/Json", async context =>
                {
                    await context.Response.WriteAsync("Json");
                });

                endpoints.MapGet("/Form", async context =>
                {
                    await context.Response.WriteAsync("Form");
                });
            });

            // Terminate Middleware M1
            app.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("Page not found!");
            });
        }
    }
}
