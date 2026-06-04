using EWallet.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EWallet.Common.Web;
//ref: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-10.0
//ref: https://dev.to/libintombaby/global-exception-handling-in-aspnet-core-the-complete-guide-3cpc
//ref: https://codewithmukesh.com/blog/global-exception-handling-in-aspnet-core/
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IProblemDetailsService _problemDetailsService;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IProblemDetailsService problemDetailsService)
    {
        _logger = logger;
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var (statusCode, title) = MapException(exception);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Instance = httpContext.Request.Path,
            Detail = exception.Message
        };

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }

    private static (int StatusCode, string Title) MapException(Exception exception)
    {
        return exception switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, "The requested resource was not found."),
            ValidationException => (StatusCodes.Status400BadRequest, "One or more validation errors occurred."),
            ArgumentNullException => (StatusCodes.Status400BadRequest, "A required argument was null."),
            ArgumentException => (StatusCodes.Status400BadRequest, "An argument was invalid."),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };
    }
}