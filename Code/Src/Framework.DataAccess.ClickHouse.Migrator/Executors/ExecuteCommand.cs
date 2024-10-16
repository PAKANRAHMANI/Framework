using Octonica.ClickHouseClient;

namespace Framework.DataAccess.ClickHouse.Migrator.Executors
{
    internal static class ExecuteCommand
    {
        public static async Task Execute(ClickHouseConfiguration clickHouseConfiguration, string command)
        {
            try
            {
                var connectionString = new ClickHouseConnectionStringBuilder
                {
                    Host = clickHouseConfiguration.Host,
                    Port = (ushort)clickHouseConfiguration.Port,
                    User = clickHouseConfiguration.Username,
                    Password = clickHouseConfiguration.Password,
                    Database = clickHouseConfiguration.Database,
                    ReadWriteTimeout = clickHouseConfiguration.ReadWriteTimeout,
                    CommandTimeout = clickHouseConfiguration.CommandTimeout
                };

                using var connection = new ClickHouseConnection(connectionString);

                await connection.OpenAsync();

                using var clickHouseCommand = connection.CreateCommand(command);

                await clickHouseCommand.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                //TODO:sentryService.CaptureException(e);
                throw;
            }
        }
    }
}
