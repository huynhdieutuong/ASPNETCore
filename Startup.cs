using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ASPNETCore
{
    public class Startup
    {
        IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<SecondMiddleware>(); // 4. Inject SecondMiddleware

            services.AddOptions();
            var testOptions = _configuration.GetSection("TestOptions");
            services.Configure<TestOptions>(testOptions);
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
                    var menu = HtmlHelper.MenuTop(
                        HtmlHelper.DefaultMenuTopItems(),
                        context.Request
                    );

                    var info = RequestProcess.RequestInfo(context.Request).HtmlTag("div", "container");

                    var html = HtmlHelper.HtmlDocument("HELLO", menu + info);

                    await context.Response.WriteAsync(html);
                });

                endpoints.MapGet("/Encoding", async context =>
                {
                    await context.Response.WriteAsync("Encoding");
                });

                // /Cookies/write
                // /Cookies/read
                // {*action} is a dynamic, can have value or empty
                endpoints.MapGet("/Cookies/{*action}", async context =>
                {
                    var menu = HtmlHelper.MenuTop(
                        HtmlHelper.DefaultMenuTopItems(),
                        context.Request
                    );

                    var action = context.GetRouteValue("action") ?? "read"; // if no action (/Cookies), set default "read" (/Cookies/read)
                    string message = null;
                    if (action.ToString() == "write")
                    {
                        var options = new CookieOptions()
                        {
                            Path = "/",
                            Expires = DateTime.Now.AddDays(1)
                        };
                        context.Response.Cookies.Append("ProductId", "4354366546", options);
                        message = "Cookie has been written";
                    }
                    else
                    {
                        var listCookies = context.Request.Cookies.Select(header => $"{header.Key}: {header.Value}".HtmlTag("li"));
                        message = string.Join("", listCookies).HtmlTag("ul");
                    }

                    var guide = "<a class=\"btn btn-danger\" href=\"/Cookies/read\">Read Cookie</a><a class=\"btn btn-success\" href=\"/Cookies/write\">Write Cookie</a>";
                    guide = guide.HtmlTag("div", "container mt-4");
                    message = message.HtmlTag("div", "container alert alert-danger");

                    var html = HtmlHelper.HtmlDocument($"Cookies: {action}", menu + guide + message);

                    await context.Response.WriteAsync(html);
                });

                endpoints.MapGet("/Json", async context =>
                {
                    context.Response.ContentType = "application/json";

                    var product = new
                    {
                        ProductName = "Apple watch 6",
                        Price = 800,
                        Date = new DateTime(2020, 11, 30)
                    };

                    var json = JsonConvert.SerializeObject(product);
                    await context.Response.WriteAsync(json);
                });

                endpoints.MapMethods("/Form", new string[] { "POST", "GET" }, async context =>
                  {
                      var menu = HtmlHelper.MenuTop(
                          HtmlHelper.DefaultMenuTopItems(),
                          context.Request
                      );

                      var formHtml = RequestProcess.ProcessForm(context.Request).HtmlTag("div", "container");

                      var html = HtmlHelper.HtmlDocument("HELLO", menu + formHtml);

                      await context.Response.WriteAsync(html);
                  });

                endpoints.MapGet("/ShowOptions", async context =>
                {
                    var testOptions = context.RequestServices.GetService<IOptions<TestOptions>>().Value;

                    var stringBuilder = new StringBuilder();
                    stringBuilder.Append("Test Options\n");
                    stringBuilder.Append($"opt_key1 = {testOptions.opt_key1}\n");
                    stringBuilder.Append($"TestOptions.opt_key2.k1 = {testOptions.opt_key2.k1}\n");
                    stringBuilder.Append($"TestOptions.opt_key2.k2 = {testOptions.opt_key2.k2}");

                    await context.Response.WriteAsync(stringBuilder.ToString());
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
