using Clinic.Api.middlewares;

namespace Clinic.Api.Extensions
{
    public static class RequestContextMiddlewareExtensionos
    {
        public static IApplicationBuilder UseRequestContextLogging(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestContextMiddleware>();
            return app;
        }
    }
}
