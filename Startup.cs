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
        // 5.4 Inject configuration
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
            services.AddTransient<TestOptionsMiddleware>(); // 2.3 Register service (Inject TestOptionsMiddleware)
            services.AddSingleton<ProductNames>(); // 3.2 Register ProductNames service
            services.AddTransient<SendMailService>(); // 5.9 Register SendMailService

            // 5.3 Register AddOptions
            services.AddOptions();

            // 5.5 Get mailSettings from appsettings.json
            var mailSettings = _configuration.GetSection("MailSettings");

            // 5.6 Inject MailSettings into Configure with mailSettings
            services.Configure<MailSettings>(mailSettings);

            var testOptions = _configuration.GetSection("TestOptions");
            services.Configure<TestOptions>(testOptions);

            // 4.1 Register service session
            // services.AddDistributedMemoryCache();

            // Register service sql server cache
            services.AddDistributedSqlServerCache(option =>
            {
                option.ConnectionString = "Server=TUONG\\SQLEXPRESS;Database=webdb;Trusted_Connection=True;";
                option.SchemaName = "dbo";
                option.TableName = "Session";
            });

            // 4.3 Config session
            services.AddSession(options =>
            {
                options.Cookie.Name = "tuonghuynh";
                options.IdleTimeout = new TimeSpan(0, 30, 0); // time out 30 minutes
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            // 4.2 Use session middleware
            app.UseSession();

            // app.UseMiddleware<FirstMiddleware>(); // 1. using UseMiddleware
            //app.UseFirstMiddleware(); // 2. create extension method for app

            // 5. use middleware
            // app.UseMiddleware<SecondMiddleware>();
            //app.UseSecondMiddleware();

            //app.UseMiddleware<TestOptionsMiddleware>(); // 2.4 Use TestOptionsMiddleware

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
                    // var testOptions = context.RequestServices.GetService<IOptions<TestOptions>>().Value;

                    // var stringBuilder = new StringBuilder();
                    // stringBuilder.Append("Test Options\n");
                    // stringBuilder.Append($"opt_key1 = {testOptions.opt_key1}\n");
                    // stringBuilder.Append($"TestOptions.opt_key2.k1 = {testOptions.opt_key2.k1}\n");
                    // stringBuilder.Append($"TestOptions.opt_key2.k2 = {testOptions.opt_key2.k2}");

                    // await context.Response.WriteAsync(stringBuilder.ToString());
                });

                endpoints.MapGet("/Session", async context =>
                {
                    int? count;
                    count = context.Session.GetInt32("count");

                    if (count == null)
                    {
                        count = 0;
                    }
                    else
                    {
                        count += 1;
                    }

                    context.Session.SetInt32("count", count.Value);
                    await context.Response.WriteAsync($"Number access /Session: {count}");
                });

                endpoints.MapGet("/SendMail", async context =>
                {
                    var message = await MailUtils.MailUtils.SendMail("frogling5112@gmail.com", "frogling5112@gmail.com", "Test Send Mail", "Hello World!");
                    await context.Response.WriteAsync(message);
                });

                endpoints.MapGet("/SendGmail", async context =>
                {
                    var message = await MailUtils.MailUtils.SendGmail("frogling5112@gmail.com", "frogling5112@gmail.com", "Test Gmail Mail", "Hello World!", "frogling5112@gmail.com", "icawxqrpxheqripi");
                    await context.Response.WriteAsync(message);
                });

                endpoints.MapGet("/SendMailService", async context =>
                {
                    // 5.10 Get Service
                    var sendMailService = context.RequestServices.GetService<SendMailService>();
                    var message = await sendMailService.SendMail(new MailContent()
                    {
                        To = "huynhdieutuong@gmail.com",
                        Subject = "Test Send Mail Service",
                        Body = "<h1>Test Service</h1><br/><p>Hello <strong>Tuong</strong>, How are you?</p>"
                    });
                    await context.Response.WriteAsync(message);
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
