using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class LogDownstreamPathMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LogDownstreamPathMiddleware> _logger;

    public LogDownstreamPathMiddleware(RequestDelegate next, ILogger<LogDownstreamPathMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        // Log the current request path
        _logger.LogInformation("Request Path: {Path}", context.Request.Path);

        // Log the downstream path template if available
        if (context.Items.TryGetValue("DownstreamPathTemplate", out var downstreamrequest))
        {
            _logger.LogInformation("Downstream Path Template: {PathTemplate}", downstreamrequest);
        }

        // Continue processing the request
        await _next(context);
    }
}
