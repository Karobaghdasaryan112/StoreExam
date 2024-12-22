using SecureStore.Api.DomainLayer.Exceptions;
using System.Net;
using System.Text.Json;

namespace SecureStore.Api.ApiLayer.Minddlewares
{
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidateException ex)
            {
                await HandleExceptionAsync(ex.Message, context, (int)HttpStatusCode.BadRequest, ex.StackTrace);
            }
            catch (DatabaseException ex)
            {
                await HandleExceptionAsync(ex.Message, context, (int)HttpStatusCode.InternalServerError, ex.StackTrace);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync("An unexpected error occurred.", context, (int)HttpStatusCode.InternalServerError, ex.StackTrace);
            }
        }
        private async Task HandleExceptionAsync(string ExeMsg, HttpContext context, int StatusCode, string StackTrace)
        {
            var response = new ErrorResponse
            {
                StatusCode = StatusCode,
                Message = ExeMsg,
            };
            context.Response.StatusCode = StatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
