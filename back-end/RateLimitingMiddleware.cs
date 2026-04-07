public class RateLimitingMiddleware
{
    private static Dictionary<string, DateTime> _requestTimestamps = new();
    private readonly RequestDelegate _next;
    public RateLimitingMiddleware(RequestDelegate next) => _next = next;
    public async Task InvokeAsync(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString();
        if (ip != null && _requestTimestamps.TryGetValue(ip, out var lastRequest))
        {
            if ((DateTime.UtcNow - lastRequest).TotalSeconds < 1)
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsync("Rate limit exceeded.");
                return;
            }
        }
        _requestTimestamps[ip] = DateTime.UtcNow;
        await _next(context);
    }
}