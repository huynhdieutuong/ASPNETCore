using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

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
        public void Configure(IApplicationBuilder applicationBuilder, IWebHostEnvironment env)
        {

        }
    }
}