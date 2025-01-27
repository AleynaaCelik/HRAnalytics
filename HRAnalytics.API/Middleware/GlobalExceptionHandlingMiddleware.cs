using HRAnalytics.API.Response;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace HRAnalytics.API.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
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

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var apiResponse = new ApiResponse<object>();

            switch (exception)
            {
                case FluentValidation.ValidationException validationEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    apiResponse = ApiResponse<object>.FailureResult(
                        "Validation error",
                        validationEx.Errors.Select(e => e.ErrorMessage));
                    Log.Warning(exception, "Validation error occurred");
                    break;

                case ValidationException validationEx: // Custom ValidationException
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    apiResponse = ApiResponse<object>.FailureResult(
                        validationEx.Message,
                        new[] { validationEx.Message });
                    Log.Warning(exception, "Validation error occurred");
                    break;

                    // ... diğer case'ler aynı kalacak
            }

            await response.WriteAsJsonAsync(apiResponse);
        }

       
    }
}

