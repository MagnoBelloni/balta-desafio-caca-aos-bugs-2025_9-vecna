using BugStore.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace BugStore.Api.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;

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
            _logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);

            var response = context.Response;
            response.ContentType = "application/json";

            int statusCode;
            string message;

            if (exception is CustomAppException appEx)
            {
                statusCode = appEx.StatusCode;
                message = appEx.Message;
            }
            else
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
                message = "Ocorreu um erro inesperado. Tente novamente mais tarde.";
            }

            response.StatusCode = statusCode;

            var result = JsonSerializer.Serialize(new
            {
                error = message,
                statusCode
            });

            await response.WriteAsync(result);
        }
    }
}
