using Microsoft.Extensions.DependencyInjection;

namespace Framework.Idempotency.Sql
{
    public static class SqlIdempotencyExtension
    {
        public static IServiceCollection UseInboxPatternWithSql(this IServiceCollection services, Action<SqlConfiguration> config)
        {
            var sqlConfiguration = new SqlConfiguration();

            config.Invoke(sqlConfiguration);

            services.AddSingleton(sqlConfiguration);

            services.AddSingleton<IDuplicateMessageHandler, SqlDuplicateMessageHandler>();

            return services;
        }
    }
}
