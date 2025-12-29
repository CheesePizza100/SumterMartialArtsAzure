using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace SumterMartialArtsAzure.Server.Api.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
            TraceId = context.TraceIdentifier
        };

        switch (exception)
        {
            //case ValidationException validationException:
            //    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            //    response.Title = "Validation Error";
            //    response.Errors = validationException.Errors
            //        .Select(e => new ErrorDetail
            //        {
            //            Field = e.PropertyName,
            //            Message = e.ErrorMessage
            //        })
            //        .ToList();

            //    _logger.LogWarning(
            //        validationException,
            //        "Validation error: {Errors}",
            //        string.Join("; ", validationException.Errors.Select(e => e.ErrorMessage))
            //    );
            //    break;

            case InvalidOperationException invalidOperationException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Title = "Business Rule Violation";
                response.Errors = new List<ErrorDetail>
                {
                    new ErrorDetail { Message = invalidOperationException.Message }
                };

                _logger.LogWarning(
                    invalidOperationException,
                    "Business rule violation: {Message}",
                    invalidOperationException.Message
                );
                break;

            case ArgumentException argumentException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Title = "Invalid Argument";
                response.Errors = new List<ErrorDetail>
                {
                    new ErrorDetail
                    {
                        Field = argumentException.ParamName,
                        Message = argumentException.Message
                    }
                };

                _logger.LogWarning(
                    argumentException,
                    "Argument error: {Message}",
                    argumentException.Message
                );
                break;

            case KeyNotFoundException keyNotFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Title = "Resource Not Found";
                response.Errors = new List<ErrorDetail>
                {
                    new ErrorDetail { Message = keyNotFoundException.Message }
                };

                _logger.LogWarning(
                    keyNotFoundException,
                    "Resource not found: {Message}",
                    keyNotFoundException.Message
                );
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Title = "Internal Server Error";
                response.Errors = new List<ErrorDetail>
                {
                    new ErrorDetail { Message = "An unexpected error occurred. Please try again later." }
                };

                _logger.LogError(
                    exception,
                    "Unhandled exception occurred: {Message}",
                    exception.Message
                );
                break;
        }

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }
}

public class ErrorResponse
{
    public string Title { get; set; } = string.Empty;
    public List<ErrorDetail> Errors { get; set; } = new();
    public string TraceId { get; set; } = string.Empty;
}

public class ErrorDetail
{
    public string? Field { get; set; }
    public string Message { get; set; } = string.Empty;
}

// Extension method for easy registration
public static class GlobalExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    }
}
