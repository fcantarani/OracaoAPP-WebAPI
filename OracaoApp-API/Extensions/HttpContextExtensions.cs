using Newtonsoft.Json;

namespace OracaoApp.API.Extensions;

public static class HttpContextExtensions
{
    public static string GetIp(this HttpContext context)
    {
        if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            var xforwarded = context.Request.Headers.Where(x => x.Key == "X-Forwarded-For").FirstOrDefault();
            return xforwarded.Value!;
        }

        var ip = context.Connection.RemoteIpAddress?.ToString();
        return ip ?? "";
    }

    public static string GetFingerprint(this HttpContext context)
    {
        var ip = context.GetIp();
        var language = string.Join(",", [.. context.Request.Headers.AcceptLanguage]);
        var location = string.Join(",", [.. context.Request.Headers.Location]);
        var origin = string.Join(",", [.. context.Request.Headers.Origin]);
        var userAgent = string.Join(",", [.. context.Request.Headers.UserAgent]);
        var pragma = string.Join(",", [.. context.Request.Headers.Pragma]);

        return JsonConvert.SerializeObject(new
        {
            ip,
            language,
            location,
            origin,
            userAgent,
            pragma,
        });
    }
}
