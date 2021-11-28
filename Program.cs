using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ASPNETCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine("Start App");

            // 1. Create IHostBuilder
            IHostBuilder builder = Host.CreateDefaultBuilder(args);

            // 2. Configure, Register Services (ConfigureWebHostDefaults)
            builder.ConfigureWebHostDefaults((IWebHostBuilder webBuilder) =>
            {
                // Custom Host (Register Service)
                webBuilder.UseStartup<MyStartUp>();

                // Rename static folder to "public" (default name "wwwroot")
                webBuilder.UseWebRoot("public");
            });

            // 3. IHostBuilder.Build() => Host(IHost)
            IHost host = builder.Build();

            // 4. Host.Run();
            host.Run();
        }
    }
}
