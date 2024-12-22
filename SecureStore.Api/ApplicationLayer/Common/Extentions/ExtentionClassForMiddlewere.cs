using SecureStore.Api.ApiLayer.Minddlewares;

namespace SecureStore.Api.ApplicationLayer.Common.Extentions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
