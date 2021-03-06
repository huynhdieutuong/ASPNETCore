using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

public class TestOptionsMiddleware : IMiddleware // 2.1 Create Middleware base IMiddleware
{
    TestOptions _testOptions { get; set; }
    ProductNames productNames { get; set; }

    // 3.3 Inject ProductNames service into TestOptionsMiddleware
    public TestOptionsMiddleware(IOptions<TestOptions> options, ProductNames product) // 2.5 get _testOptions by Inject IOptions<TestOptions>
    {
        _testOptions = options.Value;
        productNames = product;
    }
    public async Task InvokeAsync(HttpContext context, RequestDelegate next) // 2.2 Implement InvokeAsync
    {
        await context.Response.WriteAsync("Show options in TestOptionsMiddleware\n");

        // 2.6 Read _testOptions
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Test Options\n");
        stringBuilder.Append($"opt_key1 = {_testOptions.opt_key1}\n");
        stringBuilder.Append($"TestOptions.opt_key2.k1 = {_testOptions.opt_key2.k1}\n");
        stringBuilder.Append($"TestOptions.opt_key2.k2 = {_testOptions.opt_key2.k2}\n");

        // 3.4 Read productNames
        foreach (var prName in productNames.GetNames())
        {
            stringBuilder.Append(prName + "\n");
        }

        await context.Response.WriteAsync(stringBuilder.ToString());

        await next(context);
    }
}