using Framework.Authentication.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Framework.Authentication;
public static class IdpTokenManagementExtension
{
    public static IServiceCollection RegisterTokenConfiguration(this IServiceCollection services, Action<TokenManagementConfiguration> options)
    {
        var tokenManagementConfiguration = new TokenManagementConfiguration();

        options.Invoke(tokenManagementConfiguration);

        services.AddScoped(_ => tokenManagementConfiguration);

        services.AddScoped<ITokenManagement, TokenManagement>();

        services.AddHttpClient(Constants.ClientName, a =>
        {
            a.BaseAddress = new Uri(tokenManagementConfiguration.StsBaseUrl);
        });

        return services;
    }

    public static void RegisterAuthentication(this IServiceCollection services, Action<AuthenticationConfig> options)
    {
        var identityServerConfig = new AuthenticationConfig();

        options.Invoke(identityServerConfig);

        services.AddScoped(_ => identityServerConfig);
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configureOption =>
            {
                configureOption.Authority = identityServerConfig.Authority;
                configureOption.Audience = identityServerConfig.Audience;
                configureOption.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = identityServerConfig.Authority,
                    ValidAudience = identityServerConfig.Audience,
                    ValidateIssuer = identityServerConfig.ValidateIssuer,
                    ValidateAudience = identityServerConfig.ValidateAudience,
                    ValidateLifetime = identityServerConfig.ValidateLifetime,
                    ClockSkew = TimeSpan.FromSeconds(identityServerConfig.ClockSkew)
                };
            });

        services.AddAuthentication("token")
            .AddOAuth2Introspection("token", configureOption =>
            {
                configureOption.Authority = identityServerConfig.Authority;
                configureOption.ClientId = identityServerConfig.ClientId;
                configureOption.ClientSecret = identityServerConfig.ClientSecret;
            });

        services.AddAuthorization(authorizationOptions =>
        {
            authorizationOptions.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        services.AddAuthorization();
    }
}