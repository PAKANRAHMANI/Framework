using Microsoft.Extensions.DependencyInjection;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Framework.Core.Logging;
using System;

namespace Framework.RateLimit;

public static class RateLimitExtensions
    {
        public static IServiceCollection ConfigureRateLimit(this IServiceCollection services,
            Action<RateLimitSetting> options, ILogger logger)
        {
            var rateLimitConfig = new RateLimitSetting();

            options.Invoke(rateLimitConfig);

            services.AddRateLimiter(options =>
            {
                options.OnRejected = async (context, token) =>
                {

                    context.HttpContext.Response.StatusCode = rateLimitConfig.StatusCode;

                    await context.HttpContext.Response.WriteAsync(ConstantMessages.RATE_LIMIT_MESSAGE, token);

                    WriteMessage("Request rejected because too many requests received in OnRejected.", nameof(ConfigureRateLimit), logger);
                };

                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                {
                    WriteMessage("Request rejected because too many requests received in GlobalLimiter.", nameof(ConfigureRateLimit), logger);

                    var ipAddress = httpContext.Request.Headers["X-Forwarded-For"];

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: ipAddress.ToString(),
                        partition => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = rateLimitConfig.PermitLimit,
                            Window = TimeSpan.FromSeconds(rateLimitConfig.Window),
                            QueueLimit = rateLimitConfig.QueueLimit,
                            QueueProcessingOrder = (QueueProcessingOrder)rateLimitConfig.QueueProcessingOrder
                        });
                });
            });

        return services;
    }

        private static void WriteMessage(string message, string methodName, ILogger _logger)
        {
            _logger.Write(
                "class: {className} | method: {methodName} | log-event: {logEvent},message: {@message}.",
                LogLevel.Information,
                nameof(RateLimitExtensions), methodName, "Getting Token", message);
        }
}

