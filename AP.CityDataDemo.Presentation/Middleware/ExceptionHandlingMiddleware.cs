using System.Text.Json;
using AP.CityDataDemo.Application.Exceptions;
using FluentValidation;

namespace AP.CityDataDemo.Presentation.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var res = new ErrorResponseInfo();
            res.Message = ex.Message;
            
            switch (ex)
            {
                case NotFoundException notFoundEx:
                    _logger.LogWarning(notFoundEx, "Resource not found");
                    res.StatusCode = StatusCodes.Status404NotFound;
                    break;
                case ValidationException validationEx:
                    _logger.LogWarning(validationEx, "Validation failed");
                    var message = validationEx.Message?.Length > 0 ? validationEx.Message : "One or more validation errors occurred.";     
                    res.StatusCode = StatusCodes.Status400BadRequest;
                    res.Message = message;
                    break;
                case TransactionFailedException txnEx:
                    _logger.LogError(txnEx, "Transaction failed");
                    res.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
                default:
                    _logger.LogError(ex, "Unhandled exception");
                    res.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            context.Response.StatusCode = res.StatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(res));
        }
    }
}

public class ErrorResponseInfo
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
}
