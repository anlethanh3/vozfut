using System.Net;
using System.Text.Json;
using FootballManager.Domain.Exceptions;

namespace FootballManager.WebApi.Providers
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this._next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            if (exception.GetType() == typeof(DomainExeption))
            {
                var ex = exception as DomainExeption;
                var json = JsonSerializer.Serialize(new
                {
                    message = ex?.Message,
                    errors = ex?.Content,
                    traceId = string.Empty
                });

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(json, System.Text.Encoding.UTF8);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var json = JsonSerializer.Serialize(new
                {
                    message = exception.Message,
                    exception = exception.GetType().ToString(),
                    stackTrace = exception.StackTrace
                });

                await context.Response.WriteAsync(json, System.Text.Encoding.UTF8);
            }
        }
    }
}
