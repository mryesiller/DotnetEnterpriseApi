using System.Net;
using System.Text.Json;
using FluentValidation;

namespace EnterpriseAPI.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            case ValidationException validationException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Title = "Validasyon hatası";
                response.StatusCode = context.Response.StatusCode;
                response.Errors = validationException.Errors
                    .Select(e => e.ErrorMessage)
                    .ToArray();
                break;

            case KeyNotFoundException notFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Title = "Kaynak bulunamadı";
                response.StatusCode = context.Response.StatusCode;
                response.Detail = exception.Message;
                break;

            case UnauthorizedAccessException:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response.Title = "Yetkisiz erişim";
                response.StatusCode = context.Response.StatusCode;
                break;

            default:
                _logger.LogError(exception, "Beklenmeyen hata: {Message}", exception.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Title = "Sunucu hatası";
                response.StatusCode = context.Response.StatusCode;
                response.Detail = "İşleminiz sırasında bir hata oluştu";
                break;
        }

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}

public class ErrorResponse
{
    public string? Title { get; set; }
    public int StatusCode { get; set; }
    public string? Detail { get; set; }
    public string? TraceId { get; set; }
    public string[]? Errors { get; set; }
}