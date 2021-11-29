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
            await context.Response.WriteAsync($"<p>URL: {context.Request.Path}</p>");
            await _next(context);
        }
    }
}