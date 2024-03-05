using Microsoft.Extensions.DependencyInjection;

namespace Framework.Sentry;

/// <summary>
/// Capture Exception by sentry
/// </summary>
public static class SentryExtensions
{
    public static IServiceCollection RegisterSentryService(this IServiceCollection services, Action<SentrySettings> options)
    {
        var sentrySettings = new SentrySettings();

        options.Invoke(sentrySettings);

        services.AddSingleton(_ => sentrySettings);
        
        services.AddSingleton<ISentryService, SentryService>();

        return services;
    }
}