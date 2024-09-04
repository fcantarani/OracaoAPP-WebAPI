using OracaoApp.API.Extensions;

namespace OracaoApp.API.Middlewares;

public class LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var state = new Dictionary<string, object>
        {
            {"ip", context.GetIp()}
        };

        using (logger.BeginScope(state))
        {
            await next(context);
        }
    }
}
