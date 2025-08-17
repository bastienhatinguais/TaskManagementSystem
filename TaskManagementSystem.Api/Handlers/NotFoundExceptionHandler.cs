using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Api.Exceptions;

namespace TaskManagementSystem.Api.Handlers;

public class NotFoundExceptionHandler(
    ILogger<NotFoundExceptionHandler> logger,
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException) return false;

        logger.LogWarning(exception, "Resource not found");

        var problem = new ProblemDetails
        {
            Title = "Resource not found.",
            Detail = exception.Message,
            Status = StatusCodes.Status404NotFound,
            Instance = httpContext.Request.Path
        };

        await problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problem,
            Exception = exception
        });

        return true;
    }
}