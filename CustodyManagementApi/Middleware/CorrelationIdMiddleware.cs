using Microsoft.Extensions.Primitives;

namespace CustodyManagementApi.Middleware;

public class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
{
    public const string HeaderName = "X-Correlation-ID";

    public async Task Invoke(HttpContext context)
    {
        var correlationId = GetOrCreateCorrelationId(context.Request.Headers);

        context.TraceIdentifier = correlationId;
        context.Response.Headers[HeaderName] = correlationId;

        using (logger.BeginScope(new Dictionary<string, object>
               {
                   ["CorrelationId"] = correlationId
               }))
        {
            logger.LogInformation("Handling request {Method} {Path}", context.Request.Method, context.Request.Path);
            await next(context);
            logger.LogInformation("Completed request with status {StatusCode}", context.Response.StatusCode);
        }
    }

    private static string GetOrCreateCorrelationId(IHeaderDictionary headers)
    {
        if (headers.TryGetValue(HeaderName, out StringValues values) && !StringValues.IsNullOrEmpty(values))
        {
            return values.ToString();
        }

        return Guid.NewGuid().ToString("N");
    }
}
