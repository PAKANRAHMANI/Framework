using Framework.AspNetCore.Configurations;

namespace Framework.AspNetCore.MiddleWares;

public static class FrameworkMiddlewareExtensions
{
    public static IApplicationBuilder UseFrameworkExceptionMiddleware(this IApplicationBuilder builder, Action<ExceptionLogConfiguration> exceptionConfig)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (exceptionConfig == null)
        {
            throw new ArgumentNullException(nameof(exceptionConfig));
        }
        var exceptionLogConfiguration = ExceptionLogConfiguration.GetInstance();

        exceptionConfig.Invoke(exceptionLogConfiguration);

        return builder.UseMiddleware<ExceptionHandlerMiddleware>(exceptionLogConfiguration);
    }
}