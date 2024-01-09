using System.Threading.Tasks;
using Framework.Core;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Framework.AspNetCore.MiddleWares;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ICurrentUser _currentUser;

    public LoggingMiddleware(RequestDelegate next, ICurrentUser currentUser)
    {
        _next = next;
        _currentUser = currentUser;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var userId = _currentUser.GetUserIdFromNameIdentifier<string>();

        LogContext.PushProperty("UserIP", context.Connection.RemoteIpAddress?.ToString());

        LogContext.PushProperty("UserId", userId);

        await _next.Invoke(context);

    }
}