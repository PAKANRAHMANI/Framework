using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace Framework.AspNetCore.MiddleWares
{
    public static class FrameworkMiddlewareExtensions
    {
        public static IApplicationBuilder UseFrameworkExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
