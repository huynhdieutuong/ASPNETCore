using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ASPNETCore
{
    public class FirstMiddleware
    {
        private readonly RequestDelegate _next;
        // RequestDelegate ~ async (HttpContext context) => {}
        public FirstMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // HttpContext go through Middleware in pipeline
        public async Task InvokeAsync(HttpContext context)
        {
            System.Console.WriteLine($"URL: {context.Request.Path}");
            context.Items.Add("DataFirstMiddleware", $"<p>URL: {context.Request.Path}</p>"); // 2. transfer data to SecondMiddleware to avoid exception
            // await context.Response.WriteAsync($"<p>URL: {context.Request.Path}</p>"); // 1. if write content before add Headers at SecondMiddleware, throw exception
            await _next(context);
        }
    }
}