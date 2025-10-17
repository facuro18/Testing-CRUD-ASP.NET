using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Testing_CRUD.middlewares;

/// <summary>
/// Middleware that logs detailed information about HTTP requests and responses.
///
/// HOW IT WORKS:
/// 1. ASP.NET Core instantiates this middleware when app.UseMiddleware<RequestLoggingMiddleware>() is called
/// 2. For each HTTP request, ASP.NET Core calls InvokeAsync(HttpContext context)
/// 3. The middleware logs request details, calls the next middleware, then logs response details
/// 4. The 'next' parameter represents the next middleware in the pipeline
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next; // This is the next middleware in the pipeline
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = DateTime.UtcNow;

        // ===== REQUEST LOGGING (BEFORE calling next middleware) =====
        await LogRequestDetails(context);

        // ===== CALL NEXT MIDDLEWARE =====
        // This passes the request to the next middleware in the pipeline
        // (could be authentication, routing, controller, etc.)
        await _next(context);

        // ===== RESPONSE LOGGING (AFTER next middleware returns) =====
        // await LogResponseDetails(context, startTime);
    }

    private async Task LogRequestDetails(HttpContext context)
    {
        // Extract IP address from various headers
        var ipAddress = ExtractClientIP(context);

        // Extract request details
        var userAgent = context.Request.Headers.UserAgent.FirstOrDefault() ?? "Unknown";
        var queryString = context.Request.QueryString.ToString();
        // var contentType = context.Request.ContentType ?? "None";
        // var contentLength = context.Request.ContentLength ?? 0;

        // Log basic request info
        _logger.LogInformation("=== REQUEST START ===");
        _logger.LogInformation(
            "Method: {Method} | Path: {Path} | QueryString: {QueryString}",
            context.Request.Method,
            context.Request.Path,
            queryString
        );
        _logger.LogInformation(
            "IP: {IPAddress} | User-Agent: {UserAgent}",
            ipAddress ?? "Unknown",
            userAgent
        );
        // _logger.LogInformation(
        //     "Content-Type: {ContentType} | Content-Length: {ContentLength}",
        //     contentType,
        //     contentLength
        // );

        // Log request body for POST/PUT/PATCH requests
        if (ShouldLogRequestBody(context.Request.Method))
        {
            var requestBody = await ReadRequestBodyAsync(context);
            if (!string.IsNullOrEmpty(requestBody))
            {
                _logger.LogInformation("Request Body: {RequestBody}", requestBody);
            }
        }

        // Log all headers
        // _logger.LogDebug(
        //     "Request Headers: {Headers}",
        //     string.Join(", ", context.Request.Headers.Select(h => $"{h.Key}={h.Value}"))
        // );
    }

    private Task LogResponseDetails(HttpContext context, DateTime startTime)
    {
        var endTime = DateTime.UtcNow;
        var duration = (endTime - startTime).TotalMilliseconds;
        var statusCode = context.Response.StatusCode;

        _logger.LogInformation("=== RESPONSE ===");
        _logger.LogInformation(
            "Status: {StatusCode} | Duration: {Duration}ms",
            statusCode,
            duration.ToString("F2")
        );
        _logger.LogInformation("=== REQUEST END ===");
        return Task.CompletedTask;
    }

    private string ExtractClientIP(HttpContext context)
    {
        var ipAddress = context.Connection.RemoteIpAddress?.ToString();

        if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            ipAddress = context
                .Request.Headers["X-Forwarded-For"]
                .FirstOrDefault()
                ?.Split(',')
                .FirstOrDefault()
                ?.Trim();
        }
        else if (context.Request.Headers.ContainsKey("X-Real-IP"))
        {
            ipAddress = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        }

        return ipAddress ?? "Unknown";
    }

    private bool ShouldLogRequestBody(string method)
    {
        return method.Equals("POST", StringComparison.OrdinalIgnoreCase)
            || method.Equals("PUT", StringComparison.OrdinalIgnoreCase)
            || method.Equals("PATCH", StringComparison.OrdinalIgnoreCase);
    }

    private async Task<string> ReadRequestBodyAsync(HttpContext context)
    {
        try
        {
            // Enable buffering to allow reading the request body multiple times
            context.Request.EnableBuffering();

            // Reset the position to the beginning
            context.Request.Body.Position = 0;

            // Read the request body
            using var reader = new StreamReader(
                context.Request.Body,
                Encoding.UTF8,
                leaveOpen: true
            );
            var body = await reader.ReadToEndAsync();

            // Reset the position again for the next middleware
            context.Request.Body.Position = 0;

            // Limit body size for logging (prevent huge logs)
            return body.Length > 4096 ? body.Substring(0, 4096) + "... [TRUNCATED]" : body;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to read request body");
            return "[ERROR READING BODY]";
        }
    }
}
