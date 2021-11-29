using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class SecondMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.Request.Path == "/xxx.html")
        {
            context.Response.Headers.Add("SecondMiddleware", "You can not access");

            // 3. Write data from First Middleware
            var dataFromFirstMiddleware = context.Items["DataFirstMiddleware"];
            if (dataFromFirstMiddleware != null) await context.Response.WriteAsync((string)dataFromFirstMiddleware);

            await context.Response.WriteAsync("You can not access this url");
        }
        else
        {
            context.Response.Headers.Add("SecondMiddleware", "Successful access");
            await next(context);
        }
    }
}