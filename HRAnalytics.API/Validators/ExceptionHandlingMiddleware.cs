using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HRAnalytics.API.Validators
{
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
                _logger.LogError(ex, "Bir hata oluştu");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = new ProblemDetails();

            switch (exception)
            {
                case ValidationException validationEx:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Status = (int)HttpStatusCode.BadRequest;
                    response.Title = "Validation failed";
                    response.Detail = string.Join(", ", validationEx.Errors.Select(x => x.ErrorMessage));
                    break;

                case KeyNotFoundException notFoundEx:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Status = (int)HttpStatusCode.NotFound;
                    response.Title = "Not Found";
                    response.Detail = notFoundEx.Message;
                    break;

                case UnauthorizedAccessException unauthorizedEx:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Status = (int)HttpStatusCode.Unauthorized;
                    response.Title = "Unauthorized";
                    response.Detail = unauthorizedEx.Message;
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Status = (int)HttpStatusCode.InternalServerError;
                    response.Title = "Server Error";
                    response.Detail = "An internal server error has occurred.";
                    break;
            }

            await context.Response.WriteAsJsonAsync(response);
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}

