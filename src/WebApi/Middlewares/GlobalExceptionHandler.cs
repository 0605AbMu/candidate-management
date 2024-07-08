using System.Net;
using CM.WebApi.Exceptions.Common;
using Serilog;

namespace CM.WebApi.Middlewares;

public class GlobalExceptionHandler : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
#pragma warning disable CA1031
        catch (Exception e)
#pragma warning restore CA1031
        {
            if (e is ApiException apiException)
            {
                context.Response.StatusCode = (int)apiException.StatusCode;
                await context.Response.WriteAsJsonAsync(new
                {
                    Error = apiException.Message, StatusCode = apiException.StatusCode
                });
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(new
                {
                    Error = e.Message, StatusCode = (int)HttpStatusCode.InternalServerError
                });

                Log.Error("Exception: {0}", e);
            }
        }
        finally
        {
        }
    }
}
