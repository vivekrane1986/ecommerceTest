using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Domain.Exceptions;

namespace ProductCatalog.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ValidationException e)
        {
            var errorDetails = new ProblemDetails()
            {
                Status = StatusCodes.Status400BadRequest,
                Detail = e.Message,
                Title = "Validation Failed",
                Type = "ValidationFailure"
            };

            if (e.Errors is not null)
            {
                errorDetails.Extensions["errors"] = e.Errors;
            }
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsJsonAsync(errorDetails);
        }
        catch(NoDataFoundException ex)
        {
            _logger.LogError(ex.Message);
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            await httpContext.Response.WriteAsJsonAsync(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(ex.Message);
        }
    }
}
