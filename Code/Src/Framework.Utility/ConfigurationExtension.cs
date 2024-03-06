using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Framework.Utility;

/// <summary>
/// Log configuration extensions
/// </summary>
public static class ConfigurationExtension
{
    /// <summary>
    /// Registers the configuration of desired type with Singleton lifeTime and returns it.
    /// </summary>
    /// <typeparam name="T">desired input type</typeparam>
    /// <param name="configuration">application configurations</param>
    /// <param name="services">serviceCollection</param>
    /// <returns>expected configuration</returns>
    public static T GetRegisteredConfigWithSingleton<T>(this IConfiguration configuration, IServiceCollection services) where T : class, new()
    {
        var configName = typeof(T).Name;

        return configuration.GetRegisteredConfigWithSingleton<T>(services, configName);
    }

    /// <summary>
    /// Registers the configuration with specified section name and desired type with Singleton lifeTime and returns it.
    /// </summary>
    /// <typeparam name="T">desired input type</typeparam>
    /// <param name="configuration">application configurations</param>
    /// <param name="services">serviceCollection</param>
    /// <param name="configName">desired configuration section name</param>
    /// <returns>expected configuration</returns>
    public static T GetRegisteredConfigWithSingleton<T>(this IConfiguration configuration, IServiceCollection services, string configName) where T : class, new()
    {
        var config = configuration.GetConfig<T>(configName);

        services.AddSingleton(_ => config);

        return config;
    }
    /// <summary>
    /// Tries to registers the configuration of desired type with Singleton lifeTime and returns it.
    /// </summary>
    /// <typeparam name="T">desired input type</typeparam>
    /// <param name="configuration">application configurations</param>
    /// <param name="services">serviceCollection</param>
    /// <returns>expected configuration</returns>
    public static T TryGetRegisteredConfigWithSingleton<T>(this IConfiguration configuration, IServiceCollection services) where T : class, new()
    {
        var configName = typeof(T).Name;

        return configuration.TryGetRegisteredConfigWithSingleton<T>(services, configName);
    }


    /// <summary>
    /// Tries to registers the configuration of desired type with Singleton lifeTime.
    /// </summary>
    /// <typeparam name="T">desired input type</typeparam>
    /// <param name="configuration">application configurations</param>
    /// <param name="services">serviceCollection</param>
    /// <returns></returns>
    public static void TryRegisterConfigWithSingleton<T>(this IConfiguration configuration, IServiceCollection services) where T : class, new()
    {
        var configName = typeof(T).Name;

        var config = configuration.GetConfig<T>(configName);

        services.TryAddSingleton(_ => config);
    }
    /// <summary>
    /// Tries to registers the configuration of desired type with Scoped lifeTime.
    /// </summary>
    /// <typeparam name="T">desired input type</typeparam>
    /// <param name="configuration">application configurations</param>
    /// <param name="services">serviceCollection</param>
    /// <returns></returns>
    public static void TryRegisterConfigWithScoped<T>(this IConfiguration configuration, IServiceCollection services) where T : class, new()
    {
        var configName = typeof(T).Name;

        var config = configuration.GetConfig<T>(configName);

        services.TryAddScoped(_ => config);
    }
    /// <summary>
    /// Registers the configuration of desired type with Transient lifeTime.
    /// </summary>
    /// <typeparam name="T">desired input type</typeparam>
    /// <param name="configuration">application configurations</param>
    /// <param name="services">serviceCollection</param>
    /// <returns></returns>
    public static void RegisterConfigWithTransient<T>(this IConfiguration configuration, IServiceCollection services) where T : class, new()
    {
        var configName = typeof(T).Name;

        var config = configuration.GetConfig<T>(configName);

        services.AddTransient(_ => config);
    }

    /// <summary>
    /// Registers the configuration of desired type with Singleton lifeTime.
    /// </summary>
    /// <typeparam name="T">desired input type</typeparam>
    /// <param name="configuration">application configurations</param>
    /// <param name="services">serviceCollection</param>
    /// <returns></returns>
    public static void RegisterConfigWithSingleton<T>(this IConfiguration configuration, IServiceCollection services) where T : class, new()
    {
        var configName = typeof(T).Name;

        var config = configuration.GetConfig<T>(configName);

        services.AddSingleton(_ => config);
    }
    /// <summary>
    /// Registers the configuration of desired type with Scoped lifeTime.
    /// </summary>
    /// <typeparam name="T">desired input type</typeparam>
    /// <param name="configuration">application configurations</param>
    /// <param name="services">serviceCollection</param>
    /// <returns></returns>
    public static void RegisterConfigWithScoped<T>(this IConfiguration configuration, IServiceCollection services) where T : class, new()
    {
        var configName = typeof(T).Name;

        var config = configuration.GetConfig<T>(configName);

        services.AddScoped(_ => config);
    }
    /// <summary>
    /// Tries to registers the configuration of desired type with Transient lifeTime.
    /// </summary>
    /// <typeparam name="T">desired input type</typeparam>
    /// <param name="configuration">application configurations</param>
    /// <param name="services">serviceCollection</param>
    /// <returns></returns>
    public static void TryRegisterConfigWithTransient<T>(this IConfiguration configuration, IServiceCollection services) where T : class, new()
    {
        var configName = typeof(T).Name;

        var config = configuration.GetConfig<T>(configName);

        services.TryAddTransient(_ => config);
    }
    /// <summary>
    /// Tries to registers the configuration of desired type with Singleton lifeTime and returns it.
    /// </summary>
    /// <typeparam name="T">desired input type</typeparam>
    /// <param name="configuration">application configurations</param>
    /// <param name="services">serviceCollection</param>
    /// <param name="configName">desired configuration section name</param>
    /// <returns>expected configuration</returns>
    public static T TryGetRegisteredConfigWithSingleton<T>(this IConfiguration configuration, IServiceCollection services, string configName) where T : class, new()
    {
        var config = configuration.GetConfig<T>(configName);

        services.TryAddSingleton(_ => config);

        return config;
    }

    /// <summary>
    /// Registers the configuration of desired type with Scoped lifeTime and returns it.
    /// </summary>
    /// <typeparam name="T">desired input type</typeparam>
    /// <param name="configuration">application configurations</param>
    /// <param name="services">serviceCollection</param>
    /// <returns>expected configuration</returns>
    public static T GetRegisteredConfigWithScoped<T>(this IConfiguration configuration, IServiceCollection services) where T : class, new()
    {
        var configName = typeof(T).Name;

        return configuration.GetRegisteredConfigWithScoped<T>(services, configName);
    }
    /// <summary>
    /// Registers the configuration with specified section name and desired type with Scoped lifeTime and returns it.
    /// </summary>
    /// <typeparam name="T">desired input type</typeparam>
    /// <param name="configuration">application configurations</param>
    /// <param name="services">serviceCollection</param>
    /// <param name="configName">desired configuration section name</param>
    /// <returns>expected configuration</returns>
    public static T GetRegisteredConfigWithScoped<T>(this IConfiguration configuration, IServiceCollection services, string configName) where T : class, new()
    {
        var config = configuration.GetConfig<T>(configName);

        services.AddScoped(_ => config);

        return config;
    }
    /// <summary>
    /// Tries to registers the configuration of desired type with Scoped lifeTime and returns it.
    /// </summary>
    /// <typeparam name="T">desired input type</typeparam>
    /// <param name="configuration">application configurations</param>
    /// <param name="services">serviceCollection</param>
    /// <returns>expected configuration</returns>
    public static T TryGetRegisteredConfigWithScoped<T>(this IConfiguration configuration, IServiceCollection services) where T : class, new()
    {
        var configName = typeof(T).Name;

        return configuration.TryGetRegisteredConfigWithScoped<T>(services, configName);
    }
    /// <summary>
    /// Tries to registers the configuration of desired type with Scoped lifeTime and returns it.
    /// </summary>
    /// <typeparam name="T">desired input type</typeparam>
    /// <param name="configuration">application configurations</param>
    /// <param name="services">serviceCollection</param>
    /// <param name="configName">desired configuration section name</param>
    /// <returns>expected configuration</returns>
    public static T TryGetRegisteredConfigWithScoped<T>(this IConfiguration configuration, IServiceCollection services, string configName) where T : class, new()
    {
        var config = configuration.GetConfig<T>(configName);

        services.TryAddScoped(_ => config);

        return config;
    }

    /// <summary>
    /// Registers the configuration of desired type with Transient lifeTime and returns it.
    /// </summary>
    /// <typeparam name="T">desired input type</typeparam>
    /// <param name="configuration">application configurations</param>
    /// <param name="services">serviceCollection</param>
    /// <returns>expected configuration</returns>
    public static T GetRegisteredConfigWithTransient<T>(this IConfiguration configuration, IServiceCollection services) where T : class, new()
    {
        var configName = typeof(T).Name;

        return configuration.GetRegisteredConfigWithTransient<T>(services, configName);
    }
    /// <summary>
    /// Registers the configuration with specified section name and desired type with Transient lifeTime and returns it.
    /// </summary>
    /// <typeparam name="T">desired input type</typeparam>
    /// <param name="configuration">application configurations</param>
    /// <param name="services">serviceCollection</param>
    /// <param name="configName">desired configuration section name</param>
    /// <returns>expected configuration</returns>
    public static T GetRegisteredConfigWithTransient<T>(this IConfiguration configuration, IServiceCollection services, string configName) where T : class, new()
    {
        var config = configuration.GetConfig<T>(configName);

        services.AddTransient(_ => config);

        return config;
    }
    /// <summary>
    /// Tries to registers the configuration of desired type with Transient lifeTime and returns it.
    /// </summary>
    /// <typeparam name="T">desired input type</typeparam>
    /// <param name="configuration">application configurations</param>
    /// <param name="services">serviceCollection</param>
    /// <returns>expected configuration</returns>
    public static T TryGetRegisteredConfigWithTransient<T>(this IConfiguration configuration, IServiceCollection services) where T : class, new()
    {
        var configName = typeof(T).Name;

        return configuration.TryGetRegisteredConfigWithTransient<T>(services, configName);
    }
    /// <summary>
    /// Tries to registers the configuration of desired type with Transient lifeTime and returns it.
    /// </summary>
    /// <typeparam name="T">desired input type</typeparam>
    /// <param name="configuration">application configurations</param>
    /// <param name="services">serviceCollection</param>
    /// <param name="configName">desired configuration section name</param>
    /// <returns>expected configuration</returns>
    public static T TryGetRegisteredConfigWithTransient<T>(this IConfiguration configuration, IServiceCollection services, string configName) where T : class, new()
    {
        var config = configuration.GetConfig<T>(configName);

        services.TryAddTransient(_ => config);

        return config;
    }
    /// <summary>
    /// Get desired type with specified section name
    /// </summary>
    /// <typeparam name="T">desired input type</typeparam>
    /// <param name="configuration">application configurations</param>
    /// <param name="configName">desired configuration section name</param>
    /// <returns>expected configuration</returns>

    public static T GetConfig<T>(this IConfiguration configuration, string configName) where T : class, new()
    {
        var section = configuration.GetSection(configName);

        var config = section.Get<T>();

        if (config is null)
            ArgumentNullException.ThrowIfNull($"no such configuration part with name: {configName} was found in app.setting file.");

        return config;
    }
    /// <summary>
    /// Get desired type
    /// </summary>
    /// <typeparam name="T">desired input type</typeparam>
    /// <param name="configuration">application configurations</param>
    /// <returns>expected configuration</returns>
    public static T GetConfig<T>(this IConfiguration configuration) where T : class, new()
    {
        var configName = typeof(T).Name;

        return configuration.GetConfig<T>(configName);
    }
}