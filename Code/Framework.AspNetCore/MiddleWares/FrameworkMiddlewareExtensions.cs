using Framework.AspNetCore.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Framework.AspNetCore.MiddleWares
{
    public static class FrameworkMiddlewareExtensions
    {
        public static IApplicationBuilder UseFrameworkExceptionMiddleware(this IApplicationBuilder builder, IServiceCollection services, Action<ExceptionLogConfiguration> exceptionConfig)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (exceptionConfig == null)
            {
                throw new ArgumentNullException(nameof(exceptionConfig));
            }
            var exceptionLogConfiguration = new ExceptionLogConfiguration();

            exceptionConfig.Invoke(exceptionLogConfiguration);

            services.AddSingleton(exceptionLogConfiguration);

            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
