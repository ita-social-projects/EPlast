using EPlast.WebApi.CustomExceptionMiddleware;
using Microsoft.AspNetCore.Builder;

namespace EPlast.WebApi.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}