using Microsoft.Data.SqlClient;
using System.Data;
using Framework.Core.Logging;

namespace Framework.Idempotency.Sql
{
    public class SqlDuplicateMessageHandler(SqlConfiguration sqlConfiguration, ILogger logger)
        : IDuplicateMessageHandler
    {
        public async Task<bool> HasMessageBeenProcessedBefore(string eventId)
        {
            using (var connection = new SqlConnection(sqlConfiguration.ConnectionString))
            {
                var commandText = $"SELECT count(*) FROM [{sqlConfiguration.TableName}] WHERE [{sqlConfiguration.FieldName}] = '{eventId}'";

                var command = new SqlCommand(commandText, connection);

                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                var queryResult = await command.ExecuteScalarAsync();

                if (queryResult == null) return false;

                if (int.TryParse(queryResult.ToString(), out var count))
                    return count > 0;

                return false;
            }
        }

        public async Task MarkMessageAsProcessed(string eventId)
        {
            try
            {
                using (var connection = new SqlConnection(sqlConfiguration.ConnectionString))
                {
                    var command =
                        $"INSERT INTO [{sqlConfiguration.TableName}] ([{sqlConfiguration.FieldName}],[{sqlConfiguration.ReceivedDate}]) VALUES (@{sqlConfiguration.FieldName},@{sqlConfiguration.ReceivedDate})";

                    var sqlCommand = new SqlCommand(command, connection);

                    sqlCommand.Parameters.Add($"@{sqlConfiguration.FieldName}", SqlDbType.NVarChar).Value = eventId;
                    sqlCommand.Parameters.Add($"@{sqlConfiguration.ReceivedDate}", SqlDbType.DateTime2).Value =
                        DateTime.UtcNow;

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    await sqlCommand.ExecuteNonQueryAsync();
                }
            }
            catch (Exception e)
            {
                logger.WriteException(e);
            }
        }
    }
}