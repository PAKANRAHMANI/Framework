using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Framework.Idempotency.Sql
{
    public class SqlDuplicateMessageHandler : IDuplicateMessageHandler
    {
        private readonly SqlConfiguration _sqlConfiguration;

        public SqlDuplicateMessageHandler(SqlConfiguration sqlConfiguration)
        {
            _sqlConfiguration = sqlConfiguration;
        }
        public async Task<bool> HasMessageBeenProcessedBefore(Guid eventId)
        {
            using (var connection = new SqlConnection(_sqlConfiguration.ConnectionString))
            {
                var commandText = $"SELECT count(*) FROM [{_sqlConfiguration.TableName}] WHERE [{_sqlConfiguration.FieldName}] = '{eventId}'";

                var command = new SqlCommand(commandText, connection);

                var queryResult = await command.ExecuteScalarAsync();

                if (queryResult == null) return false;

                if (int.TryParse(queryResult.ToString(), out var count))
                    return count > 0;

                return false;
            }
        }

        public async Task MarkMessageAsProcessed(Guid eventId, DateTime receivedDate)
        {
            using (var connection = new SqlConnection(_sqlConfiguration.ConnectionString))
            {
                var command = $"INSERT INTO [{_sqlConfiguration.TableName}] ([{_sqlConfiguration.FieldName}],[{_sqlConfiguration.ReceivedDate}]) VALUES (@{_sqlConfiguration.FieldName},@{_sqlConfiguration.ReceivedDate})";

                var sqlCommand = new SqlCommand(command, connection);

                sqlCommand.Parameters.Add($"@{_sqlConfiguration.FieldName}", SqlDbType.NVarChar).Value = eventId;
                sqlCommand.Parameters.Add($"@{_sqlConfiguration.ReceivedDate}", SqlDbType.DateTime2).Value = receivedDate;

                await sqlCommand.ExecuteNonQueryAsync();
            }
        }
    }
}