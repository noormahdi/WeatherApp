namespace WeatherApp.Web.Middleware;

public class ErrorSimulatorMiddleware
{
    private readonly RequestDelegate _next;
    private static int _requestCount = 0;
    private static readonly object _lock = new object();
    public ErrorSimulatorMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.Equals("/api/Weather"))
        {
            bool shouldReturn503 = false;

            lock (_lock)
            {
                _requestCount++;
                if (_requestCount % 5 == 0)
                {
                    shouldReturn503 = true;
                }
            }

            if (shouldReturn503)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                return;
            } 
        }

        await _next(context);
    }
}
