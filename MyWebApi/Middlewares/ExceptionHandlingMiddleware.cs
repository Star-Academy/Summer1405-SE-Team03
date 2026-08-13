using MyWebApi.Exceptions;

namespace MyWebApi.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = StatusCodes.Status500InternalServerError;
        var message = "An unexpected error occurred.";

        switch (exception)
        {
            case BadRequestException badRequestException:
                statusCode = StatusCodes.Status400BadRequest;
                message = badRequestException.Message;
                break;

            case InvalidDatabaseException invalidDatabaseException:
                statusCode = StatusCodes.Status400BadRequest;
                message = invalidDatabaseException.Message;
                break;

            case ResourceNotFoundException notFoundException:
                statusCode = StatusCodes.Status404NotFound;
                message = notFoundException.Message;
                break;

            case DuplicateResourceException conflictException:
                statusCode = StatusCodes.Status409Conflict;
                message = conflictException.Message;
                break;

            case ArgumentException argumentException:
                statusCode = StatusCodes.Status400BadRequest;
                message = argumentException.Message;
                break;

            default:
                _logger.LogError(exception, "Unhandled exception occurred.");
                break;
        }

        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(new
        {
            error = message
        });
    }
}