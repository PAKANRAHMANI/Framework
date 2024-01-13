using ClickHouse.Ado;
using ClickHouse.Net;
using Framework.DataAccess.ClickHouse.Connections;
using Framework.DataAccess.ClickHouse.Engines;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.DataAccess.ClickHouse
{
    public static class ClickHouseExtensions
    {
        public static IServiceCollection RegisterClickHouse(this IServiceCollection services, Action<ConnectionSetting> setting)
        {
            services.AddClickHouse();

            var clickHouseSetting = new ConnectionSetting();

            setting.Invoke(clickHouseSetting);

            var connectionSetting = new ConnectionStringBuilder()
                .WithHost(clickHouseSetting.Host)
                .WithPort(clickHouseSetting.Port)
                .WithCheckCompressedHash(clickHouseSetting.CheckCompressedHash)
                .WithCompress(clickHouseSetting.Compress)
                .WithCompressor(clickHouseSetting.Compressor)
                .WithDatabaseName(clickHouseSetting.DatabaseName)
                .WithSocketTimeout(clickHouseSetting.SocketTimeout)
                .WithUsername(clickHouseSetting.Username)
                .WithPassword(clickHouseSetting.Password)
                .Build();

            services.AddTransient(p => new ClickHouseConnectionSettings(connectionSetting));

            services.AddTransient<IClickHouse, ClickHouse>();

            services.AddTransient<IKafkaEngineTemplate, KafkaEngineTemplate>();

            return services;
        }
    }
}
