using Octonica.ClickHouseClient;

namespace Framework.DataAccess.ClickHouse.Migrator.Executors
{
    public static class ExecuteCommand
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

                await using var connection = new ClickHouseConnection(connectionString);

                await connection.OpenAsync();

                await using var clickHouseCommand = connection.CreateCommand(command);

                await clickHouseCommand.ExecuteNonQueryAsync();

                await connection.CloseAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
