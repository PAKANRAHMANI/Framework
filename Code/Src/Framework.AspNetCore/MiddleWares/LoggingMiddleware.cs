using Framework.Core;
using Serilog.Context;

namespace Framework.AspNetCore.MiddleWares;

public class LoggingMiddleware(RequestDelegate next, ICurrentUser currentUser)
{
    public async Task InvokeAsync(HttpContext context)
    {
        LogContext.PushProperty("UserIP", currentUser.GetUserIp());

        LogContext.PushProperty("UserId", currentUser.GetUserIdFromNameIdentifier<string>());

        await next.Invoke(context);

    }
}