using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace MemeSource.Filters
{
    public class MyFilter : Attribute, IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            Log.Information("OnResultExecuting");
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            Log.Information("OnResultExecuted");
        }
    }

    public class AsyncFilter : Attribute, IAsyncResultFilter
    {

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            await context.HttpContext.Response.WriteAsync("async in  \r\n");

            var resultContext = await next();

            await context.HttpContext.Response.WriteAsync("async out  \r\n");
        }
    }
}
