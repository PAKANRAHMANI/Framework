using Framework.DataAccess.CH.Connections;
using Framework.DataAccess.CH.Engines;
using Microsoft.Extensions.DependencyInjection;


namespace Framework.DataAccess.CH
{
    public static class ClickHouseExtensions
    {
        public static IServiceCollection RegisterClickHouse(this IServiceCollection services, Action<ConnectionSetting> setting)
        {
            var clickHouseSetting = new ConnectionSetting();

            setting.Invoke(clickHouseSetting);

            var connectionSetting = new ConnectionStringBuilder()
                .WithHost(clickHouseSetting.Host)
                .WithPort(clickHouseSetting.Port)
                .WithCompress(clickHouseSetting.Compress)
                .WithDatabaseName(clickHouseSetting.DatabaseName)
                .WithUsername(clickHouseSetting.Username)
                .WithPassword(clickHouseSetting.Password);

            services.AddSingleton(connectionSetting);

            services.AddSingleton<IClickHouse, ClickHouse>();

            services.AddSingleton<IKafkaEngineTemplate, KafkaEngineTemplate>();

            return services;
        }
    }
}
