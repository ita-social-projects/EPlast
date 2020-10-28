using EPlast.WebApi.CustomMiddlewares;
using EPlast.WebApi.WebSocketHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace EPlast.WebApi.Extensions
{
    public static class NotificationWebSocketMiddlewareExtension
    {
        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app,
                                                        PathString path,
                                                        BaseWebSocketHandler handler)
        {
            return app.Map(path, (_app) => _app.UseMiddleware<NotificationWebSocketMiddleware>(handler));
        }

    }
}
