using ASPNETCore;
using Microsoft.AspNetCore.Builder;

public static class UseFirstMiddlewareMethod
{
    public static void UseFirstMiddleware(this IApplicationBuilder app) // 2. create extension method for app
    {
        app.UseMiddleware<FirstMiddleware>();
    }

    public static void UseSecondMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<SecondMiddleware>();
    }
}